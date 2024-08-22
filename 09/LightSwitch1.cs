using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class LightSwitch1 : MonoBehaviour
{
    public Light2D globalLight; // Global Light 2D 오브젝트
    public Button switchButton; // UI 버튼
    public LightAnimation lightAnimation; // LightAnimation 스크립트를 참조

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
    }

    void UpdateLight()
    {
        globalLight.intensity = isLightOn ? 1.0f : 0.2f; // 밝기 조절

        // LightAnimation 스크립트의 상태 업데이트 메서드 호출
        if (lightAnimation != null)
        {
            lightAnimation.UpdateLightState(isLightOn);
        }
    }
}
