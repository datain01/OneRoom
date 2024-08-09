using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonFind : MonoBehaviour, IPointerDownHandler
{
    public Button findButton;        // 버튼을 할당받을 변수
    public GameObject SFXFind;       // SFXFind 오브젝트를 할당받을 변수

    private AudioSource audioSource; // SFXFind의 AudioSource를 참조할 변수

    private void Start()
    {
        // SFXFind 오브젝트에서 AudioSource 컴포넌트를 가져옴
        if (SFXFind != null)
        {
            audioSource = SFXFind.GetComponent<AudioSource>();
        }
    }

    // 버튼이 클릭되는 순간 호출되는 메서드
    public void OnPointerDown(PointerEventData eventData)
    {
        if (audioSource != null)
        {
            audioSource.Play(); // 오디오 클립 재생
        }
        else
        {
            Debug.LogWarning("AudioSource가 할당되지 않았습니다.");
        }
    }
}
