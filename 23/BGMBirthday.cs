using System;
using UnityEngine;

public class BGMBirthday : MonoBehaviour
{
    public AudioSource birthdayBGM; // 재생할 BGM 오디오 소스

    private bool hasPlayed = false; // BGM이 이미 재생되었는지 확인하기 위한 플래그

    private void Start()
    {
        // 현재 날짜를 가져오기
        DateTime today = DateTime.Now;

        // 특정 날짜 (10월 1일, 3월 31일)인지 확인
        if ((today.Month == 10 && today.Day == 1 || 
             today.Month == 3 && today.Day == 31 || 
             today.Month == 12 && today.Day == 17 || 
             today.Month == 5 && today.Day == 31) && !hasPlayed)
        {
            PlayBirthdayBGM(); // BGM 재생
        }
    }

    // BGM 재생 메서드
    private void PlayBirthdayBGM()
    {
        if (birthdayBGM != null)
        {
            birthdayBGM.Play(); // BGM 재생
            hasPlayed = true; // BGM이 재생되었음을 표시
            Debug.Log("Special day! BGM 재생 중...");
        }
        else
        {
            Debug.LogError("Birthday BGM이 할당되지 않았습니다.");
        }
    }
}
