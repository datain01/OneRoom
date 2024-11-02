using UnityEngine;
using System.Collections;

public class TimeLikeReset : MonoBehaviour
{
    public LikeDisplay likeDisplay; // LikeDisplay 스크립트를 참조
    public string characterTag = "CharacterSec"; // 리셋할 캐릭터 태그
    public StopwatchManager stopwatchManager; // StopwatchManager 스크립트 참조

    private float resetInterval = 1200.0f; // 3초 주기로 리셋
    private float nextResetTime = 1200.0f; // 다음 리셋 시점

    private void Update()
    {
        // StopwatchManager에서 경과 시간을 확인하고, nextResetTime을 넘으면 리셋
        if (stopwatchManager.elapsedTime >= nextResetTime)
        {
            ResetLikes();
            nextResetTime += resetInterval; // 다음 리셋 시점을 3초 후로 설정
        }
    }

    private void ResetLikes()
    {
        // CharacterSec 태그를 가진 모든 오브젝트에 대해 호감도를 0으로 리셋
        GameObject[] characters = GameObject.FindGameObjectsWithTag(characterTag);
        foreach (GameObject character in characters)
        {
            PlayerPrefs.SetInt(characterTag + "_like", 0); // 호감도 0으로 설정
            PlayerPrefs.Save();
            Debug.Log(characterTag + " likes reset to 0");

            // LikeDisplay 업데이트
            if (likeDisplay != null && likeDisplay.characterTag == characterTag)
            {
                likeDisplay.UpdateLikeDisplay();
            }
        }
    }
}
