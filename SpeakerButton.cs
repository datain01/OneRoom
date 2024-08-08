using UnityEngine;
using UnityEngine.UI; // 추가: Graphic 컴포넌트를 사용하기 위해

public class SpeakerButton : MonoBehaviour
{
    public GameObject panelSpeaker;
    public BoxCollider2D[] boxCollidersToDisable;

    private void Awake()
    {
        if (panelSpeaker != null)
        {
            panelSpeaker.GetComponent<PanelSpeaker>().InitializeSpeaker();
            SetPanelVisibility(false); // 게임 시작 시 패널을 숨김
        }
    }

    public void OnSpeakerButtonClick()
    {
        if (panelSpeaker != null)
        {
            bool isVisible = panelSpeaker.GetComponent<CanvasGroup>().alpha > 0;
            SetPanelVisibility(!isVisible);
        }
    }

    public void SetPanelVisibility(bool isVisible)
    {
        CanvasGroup canvasGroup = panelSpeaker.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = isVisible ? 1 : 0;
            canvasGroup.blocksRaycasts = isVisible;
            canvasGroup.interactable = isVisible;
        }

        // BoxCollider2D를 활성화/비활성화
        foreach (BoxCollider2D collider in boxCollidersToDisable)
        {
            collider.enabled = !isVisible;
        }

        // panelSpeaker와 자식 컴포넌트들의 raycastTarget 설정
        SetRaycastTarget(panelSpeaker, isVisible);
    }

    private void SetRaycastTarget(GameObject parent, bool isTarget)
    {
        Graphic[] graphics = parent.GetComponentsInChildren<Graphic>();
        foreach (Graphic graphic in graphics)
        {
            graphic.raycastTarget = isTarget;
        }
    }
}
