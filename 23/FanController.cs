using UnityEngine;

public class FanController : MonoBehaviour
{
    private Animator animator;
    private bool isReadyForNextClick = true; // 클릭에 반응할 준비가 되었는지 확인
    public AudioSource audioSource;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        if (isReadyForNextClick)
        {
            isReadyForNextClick = false; // 다음 클릭을 기다리는 동안 반응하지 않도록 설정
            animator.SetTrigger("NextState");
            Debug.Log("Fan clicked and animation state change triggered.");
            // 효과음 재생
            PlayClickSound();
        }
    }

    // 애니메이션 이벤트에서 호출될 메서드
    public void OnAnimationComplete()
    {
        isReadyForNextClick = true; // 다음 클릭에 반응할 준비 완료
        Debug.Log("Animation complete, ready for next click.");
    }

    private void PlayClickSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
