using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class LightSwitch : MonoBehaviour
{
    public Light2D globalLight; // Global Light 2D 오브젝트
    public Button switchButton; // UI 버튼

    private bool isLightOn = true;

    void Start()
    {
        // 버튼 클릭 이벤트에 메서드 연결
        switchButton.onClick.AddListener(ToggleLight);
    }

    void ToggleLight()
    {
        isLightOn = !isLightOn;
        globalLight.intensity = isLightOn ? 1.0f : 0.0f; // 밝기 조절
    }
}
