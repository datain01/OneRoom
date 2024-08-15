using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnowAnimation : MonoBehaviour
{
    public List<Sprite> snowSprites; // 이미지들을 담을 리스트
    public SpriteRenderer targetRenderer; // 스프라이트를 표시할 SpriteRenderer 컴포넌트
    public float frameInterval = 0.5f; // 이미지 변경 간격 (초)

    private void Start()
    {
        if (snowSprites.Count > 0)
        {
            StartCoroutine(PlaySnowAnimation());
        }
    }

    private IEnumerator PlaySnowAnimation()
    {
        int index = 0;
        while (true)
        {
            targetRenderer.sprite = snowSprites[index];
            index = (index + 1) % snowSprites.Count; // 리스트의 끝에서 처음으로 순환
            yield return new WaitForSeconds(frameInterval);
        }
    }
}
