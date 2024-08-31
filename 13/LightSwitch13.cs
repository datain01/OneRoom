using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class LightSwitch13 : MonoBehaviour
{
    public Light2D globalLight; // Global Light 2D 오브젝트
    public Button switchButton; // UI 버튼
    public LightImageChange lightImageChange; // LightImageChange 스크립트를 참조
    public AudioSource sfxAudioSource; // 효과음을 재생할 AudioSource

    public bool isLightOn = true;

    void Start()
    {
        // 버튼 클릭 이벤트에 메서드 연결
        switchButton.onClick.AddListener(ToggleLight);

        // 초기 상태 설정
        UpdateLight();
    }

    void ToggleLight()
    {
        isLightOn = !isLightOn;
        UpdateLight();

        // 효과음 재생
        PlaySFX();
    }

    void UpdateLight()
    {
        // 밝기 조절
        globalLight.intensity = isLightOn ? 1.0f : 0.2f; 

        // LightImageChange 스크립트의 상태 업데이트 메서드 호출
        if (lightImageChange != null)
        {
            lightImageChange.UpdateLightState(isLightOn);
        }
    }

    void PlaySFX()
    {
        if (sfxAudioSource != null)
        {
            sfxAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("SFX AudioSource is not assigned.");
        }
    }
}
