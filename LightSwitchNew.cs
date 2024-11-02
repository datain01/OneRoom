using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitchNew : MonoBehaviour
{
    public Light2D globalLight; // Global Light 2D 오브젝트
    public SpriteRenderer switchSprite; // 2D 스프라이트 렌더러

    public bool isLightOn = true;

    void Start()
    {
        // 초기 상태 설정
        UpdateLight();
    }

    // 스프라이트를 클릭했을 때 호출되는 함수
    void OnMouseDown()
    {
        ToggleLight();
    }

    void ToggleLight()
    {
        isLightOn = !isLightOn;
        UpdateLight();
    }

    void UpdateLight()
    {
        globalLight.intensity = isLightOn ? 1.0f : 0.2f; // 밝기 조절
    }
}
