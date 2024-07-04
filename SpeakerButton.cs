using UnityEngine;

public class SpeakerButton : MonoBehaviour
{
    public GameObject panelSpeakerPrefab;
    private GameObject currentPanel;

    public void OnSpeakerButtonClick()
    {
        if (currentPanel == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                currentPanel = Instantiate(panelSpeakerPrefab, canvas.transform);
                PanelSpeaker panelSpeaker = currentPanel.GetComponent<PanelSpeaker>();
                if (panelSpeaker != null)
                {
                    Debug.Log("Music folder path: " + panelSpeaker.musicFolderPath);
                }
            }
            else
            {
                Debug.LogError("Canvas not found in the scene.");
            }
        }
        else
        {
            Destroy(currentPanel);
            currentPanel = null;
        }
    }
}
