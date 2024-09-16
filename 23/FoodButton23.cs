using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FoodButton23 : MonoBehaviour
{
    public string characterTag; // Inspector에서 설정
    public ChaController23 characterController; // 캐릭터 컨트롤러 참조
    public LikeDisplay likeDisplay; // LikeDisplay 참조
    public AudioSource audioSource; // 효과음을 재생할 AudioSource
    public StopwatchManager stopwatchManager; // StopwatchManager 참조

    private Button button;
    [SerializeField] private int clickAllowance = 0; // 활성화 가능 횟수
    [SerializeField] private float lastActivationTime = 0; // 마지막 활성화 시간

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        button.interactable = false; // 초기에는 버튼을 비활성화
        StartCoroutine(TrackTimeAndReset());
    }

    private IEnumerator TrackTimeAndReset()
    {
        float previousElapsedTime = stopwatchManager.elapsedTime;

        while (true)
        {
            yield return new WaitForSeconds(0.1f); // 더 빈번한 체크로 스톱워치 리셋 감지

            if (stopwatchManager.elapsedTime < previousElapsedTime) // 리셋 감지
            {
                ResetButton();
            }

            if (stopwatchManager.isRunning)
            {
                float timeSinceLastActivation = stopwatchManager.elapsedTime - lastActivationTime;
                if (timeSinceLastActivation >= 3.0f)
                {
                    lastActivationTime += 3.0f;
                    clickAllowance++; // 클릭 횟수 증가
                    button.interactable = clickAllowance > 0; // 버튼 활성화 조건 체크
                }
            }
            previousElapsedTime = stopwatchManager.elapsedTime;
        }
    }

    private void OnButtonClick()
    {
        if (clickAllowance > 0 && characterController != null)
        {
            clickAllowance--; // 사용 가능 횟수 감소
            button.interactable = clickAllowance > 0; // 활성화 상태 업데이트

            // Eat 애니메이션 재생
            characterController.EatCharacter();
            StartCoroutine(HandleEatAnimation());

            // 효과음 재생
            PlayClickSound();
        }
    }

    private IEnumerator HandleEatAnimation()
    {
        IncreaseLike(characterTag);
        yield return new WaitForSeconds(characterController.eatTime);
    }

    private void IncreaseLike(string characterTag)
    {
        int currentLikes = PlayerPrefs.GetInt(characterTag + "_like", 0);
        currentLikes += 10; // 좋아요 증가
        PlayerPrefs.SetInt(characterTag + "_like", currentLikes);
        PlayerPrefs.Save();
        if (likeDisplay != null && likeDisplay.characterTag == characterTag)
        {
            likeDisplay.UpdateLikeDisplay();
        }
    }

    private void PlayClickSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private void ResetButton()
    {
        clickAllowance = 0; // 활성화 가능 횟수 초기화
        lastActivationTime = 0; // 마지막 활성화 시간 초기화
        button.interactable = false; // 버튼 비활성화
        Debug.Log("Button reset due to stopwatch reset.");
    }
}
