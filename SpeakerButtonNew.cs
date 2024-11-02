using UnityEngine;

public class SpeakerButtonNew : MonoBehaviour
{
    public GameObject panelSpeaker;
    public BoxCollider2D[] boxCollidersToDisable;
    public SpriteRenderer speakerSprite; // 2D 스프라이트 렌더러 (이름: Speaker)

    private void Awake()
    {
        if (panelSpeaker != null)
        {
            panelSpeaker.GetComponent<PanelSpeaker>().InitializeSpeaker();
            SetPanelVisibility(false); // 게임 시작 시 패널을 숨김
        }
    }

    // 스프라이트를 클릭했을 때 호출되는 함수
    private void OnMouseDown()
    {
        OnSpeakerButtonClick();
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
            canvasGroup.blocksRaycasts = false; // 패널을 처음 활성화할 때 일시적으로 Raycast 막음
            canvasGroup.interactable = isVisible;

            // 일정 시간 뒤에 Raycast 활성화
            if (isVisible)
            {
                Invoke(nameof(EnableRaycast), 0.1f); // 0.1초 뒤에 Raycast 가능
            }
        }

        // BoxCollider2D를 활성화/비활성화
        foreach (BoxCollider2D collider in boxCollidersToDisable)
        {
            collider.enabled = !isVisible;
        }
    }

    // 일정 시간 뒤에 Raycast 다시 활성화
    private void EnableRaycast()
    {
        CanvasGroup canvasGroup = panelSpeaker.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }
    }
}
