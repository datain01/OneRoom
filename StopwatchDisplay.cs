using UnityEngine;
using TMPro;

public class StopwatchDisplay : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    public void UpdateDisplay(float elapsedTime)
    {
        int hours = Mathf.FloorToInt(elapsedTime / 3600);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        timeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}
