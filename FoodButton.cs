using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FoodButton : MonoBehaviour
{
    public string characterTag; // Inspector에서 설정
    public ChaController characterController; // 캐릭터 컨트롤러 참조
    public LikeDisplay likeDisplay; // LikeDisplay 참조
    public AudioSource audioSource; // 효과음을 재생할 AudioSource
    public int likeIncreaseAmount = 5; // 인스펙터에서 설정할 호감도 증가 값

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (characterController != null)
        {
            // Eat 애니메이션 재생
            characterController.EatCharacter();
            StartCoroutine(HandleEatAnimation());

            // 효과음 재생
            PlayClickSound();
        }
    }

    private IEnumerator HandleEatAnimation()
    {
        // Like 값 증가
        IncreaseLike(characterTag);
        // 애니메이션 재생 시간 동안 대기
        yield return new WaitForSeconds(characterController.eatTime);
    }

    private void IncreaseLike(string characterTag)
    {
        int currentLikes = PlayerPrefs.GetInt(characterTag + "_like", 0);
        currentLikes += likeIncreaseAmount; // 인스펙터에서 설정한 값만큼 증가
        PlayerPrefs.SetInt(characterTag + "_like", currentLikes);
        PlayerPrefs.Save();
        Debug.Log(characterTag + " likes: " + currentLikes);

        // LikeDisplay 업데이트
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
}
