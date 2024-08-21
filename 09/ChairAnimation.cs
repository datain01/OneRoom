using System.Collections;
using UnityEngine;

public class ChairAnimation : MonoBehaviour
{
    public Sprite[] spriteArray;  // 스프라이트 배열을 인스펙터에서 할당
    public float animationSpeed = 0.1f;  // 애니메이션 속도 (0.1초 간격으로 변경)

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  // SpriteRenderer를 가져옴
    }

    void OnMouseDown()
    {
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        for (int i = 0; i < spriteArray.Length; i++)
        {
            spriteRenderer.sprite = spriteArray[i];  // 스프라이트 변경
            yield return new WaitForSeconds(animationSpeed);  // 다음 스프라이트로 넘어가기 전에 잠시 대기
        }
    }
}
