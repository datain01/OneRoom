using UnityEngine;

public class SpeakerButton : MonoBehaviour
{
    public GameObject panelSpeaker;

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

    private void SetPanelVisibility(bool isVisible)
    {
        CanvasGroup canvasGroup = panelSpeaker.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = isVisible ? 1 : 0;
            canvasGroup.blocksRaycasts = isVisible;
            canvasGroup.interactable = isVisible;
        }
    }
}
