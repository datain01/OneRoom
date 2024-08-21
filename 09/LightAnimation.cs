using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LightAnimation : MonoBehaviour
{
    public Image lightButtonImage;           // UI 버튼의 Image 컴포넌트
    public Sprite fireplaceOff;              // LightOff 상태일 때 사용할 이미지
    public Sprite[] fireplaceOnSprites;      // LightOn 상태일 때 사용할 애니메이션 스프라이트 배열
    public float animationSpeed = 0.1f;      // 애니메이션 속도 (0.1초 간격으로 변경)

    private bool isAnimating = false;        // 애니메이션이 실행 중인지 확인하기 위한 변수

    public void UpdateLightState(bool isLightOn)
    {
        if (isLightOn)
        {
            // Light가 켜지면 애니메이션 시작
            if (!isAnimating)
            {
                StartCoroutine(PlayLightOnAnimation());
            }
        }
        else
        {
            // Light가 꺼지면 애니메이션 중지 및 Off 이미지로 변경
            StopAllCoroutines();
            lightButtonImage.sprite = fireplaceOff;
            isAnimating = false;
        }
    }

    IEnumerator PlayLightOnAnimation()
    {
        isAnimating = true;
        int currentFrame = 0;

        while (true)
        {
            lightButtonImage.sprite = fireplaceOnSprites[currentFrame];
            currentFrame = (currentFrame + 1) % fireplaceOnSprites.Length;
            yield return new WaitForSeconds(animationSpeed);
        }
    }
}
