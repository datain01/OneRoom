using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerAnimation18 : MonoBehaviour
{
    public Button speakerButton; // Speaker 버튼 UI
    public List<Sprite> imageList; // 애니메이션으로 사용할 이미지 리스트
    public float frameRate = 0.1f; // 이미지가 바뀌는 시간 간격 (초 단위)

    private Image speakerImage; // Speaker 버튼의 Image 컴포넌트
    private bool isAnimating = false; // 애니메이션이 재생 중인지 여부

    private void Start()
    {
        // Speaker 버튼의 Image 컴포넌트를 가져옴
        speakerImage = speakerButton.GetComponent<Image>();

        if (speakerButton == null || speakerImage == null)
        {
            Debug.LogError("Speaker 버튼 또는 Image 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // 버튼 클릭 이벤트에 애니메이션 시작 함수 추가
        speakerButton.onClick.AddListener(StartAnimation);
    }

    private void StartAnimation()
    {
        if (!isAnimating)
        {
            StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        isAnimating = true;

        // 이미지 리스트를 순서대로 재생
        foreach (var sprite in imageList)
        {
            speakerImage.sprite = sprite; // 현재 이미지로 변경
            yield return new WaitForSeconds(frameRate); // 설정된 시간 대기
        }

        isAnimating = false; // 애니메이션이 끝나면 재생 상태를 false로 설정
    }
}
