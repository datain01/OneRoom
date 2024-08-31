using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LightAnimation15 : MonoBehaviour
{
    public Image lightButtonImage;             // UI 버튼의 Image 컴포넌트
    public Sprite[] fireplaceOffSprites;       // LightOff 상태일 때 사용할 애니메이션 스프라이트 배열
    public Sprite[] fireplaceOnSprites;        // LightOn 상태일 때 사용할 애니메이션 스프라이트 배열
    public float animationSpeed = 0.1f;        // 애니메이션 속도 (0.1초 간격으로 변경)

    private Coroutine currentAnimationCoroutine = null;  // 현재 실행 중인 애니메이션 코루틴

    public void UpdateLightState(bool isLightOn)
    {
        // 이전 애니메이션 코루틴이 실행 중이라면 중지
        if (currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine);
        }

        // LightOn 및 LightOff 상태에 따라 새로운 애니메이션 시작
        if (isLightOn)
        {
            // Light가 켜지면 On 애니메이션 시작
            currentAnimationCoroutine = StartCoroutine(PlayAnimation(fireplaceOnSprites));
        }
        else
        {
            // Light가 꺼지면 Off 애니메이션 시작
            currentAnimationCoroutine = StartCoroutine(PlayAnimation(fireplaceOffSprites));
        }
    }

    IEnumerator PlayAnimation(Sprite[] sprites)
    {
        int currentFrame = 0;

        while (true)
        {
            lightButtonImage.sprite = sprites[currentFrame];
            currentFrame = (currentFrame + 1) % sprites.Length;
            yield return new WaitForSeconds(animationSpeed);
        }
    }
}
