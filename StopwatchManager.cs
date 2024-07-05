using UnityEngine;
using System.Collections;

public class StopwatchManager : MonoBehaviour
{
    private bool isRunning = false;
    private float elapsedTime = 0f;

    public StopwatchDisplay stopwatchDisplay;

    private void Start()
    {
        isRunning = true;
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            stopwatchDisplay.UpdateDisplay(elapsedTime);
        }
    }

    public void StartPauseStopwatch()
    {
        isRunning = !isRunning;
    }

    public void ResetStopwatch()
    {
        isRunning = false;
        elapsedTime = 0f;
        stopwatchDisplay.UpdateDisplay(elapsedTime);
    }
}
