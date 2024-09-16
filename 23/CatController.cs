using System.Collections;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public ChaController23 characterOneController; // CharacterOne의 ChaController23 참조
    public ChaController23 characterSecController; // CharacterSec의 ChaController23 참조
    public float stopDuration = 3.0f; // 멈춤 시간
    public AudioSource sfxAudioSource; // SFXCat 오브젝트에 붙은 AudioSource 참조

    private void OnMouseDown()
    {
        // Cat 클릭 시 효과음 재생
        PlayCatSFX();
        
        // 캐릭터 이동 멈춤 및 Idle 상태로 전환
        StartCoroutine(StopCharactersTemporarily());
    }

    private void PlayCatSFX()
    {
        if (sfxAudioSource != null)
        {
            sfxAudioSource.Play(); // 효과음 재생
        }
        else
        {
            Debug.LogWarning("SFX AudioSource가 할당되지 않았습니다.");
        }
    }

    private IEnumerator StopCharactersTemporarily()
    {
        // 캐릭터 이동 멈춤 및 Idle 상태로 전환
        characterOneController.StopAndIdle();
        characterSecController.StopAndIdle();

        // 멈춤 시간 대기
        yield return new WaitForSeconds(stopDuration);

        // 멈춤 후 이동 재개
        characterOneController.StartCoroutine(characterOneController.WaitAndMove());
        characterSecController.StartCoroutine(characterSecController.WaitAndMove());
    }
}
