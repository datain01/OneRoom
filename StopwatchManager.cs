using UnityEngine;
using System.Collections;

public class StopwatchManager : MonoBehaviour
{
    public bool isRunning = false;
    public float elapsedTime = 0f;

    public StopwatchDisplay stopwatchDisplay;
    public AudioSource audioSource; // 효과음을 재생할 AudioSource

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

    public void SetRunning(bool running)
    {
        isRunning = running;
    }

    public void StartPauseStopwatch()
    {
        isRunning = !isRunning;

        // 효과음 재생
        PlayClickSound();
    }

    public void ResetStopwatch()
    {
        isRunning = false;
        elapsedTime = 0f;
        stopwatchDisplay.UpdateDisplay(elapsedTime);

        // 효과음 재생
        PlayClickSound();
    }

    private void PlayClickSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
