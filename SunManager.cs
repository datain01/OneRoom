using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class SunManager : MonoBehaviour
{
    public Light2D lightSun; // Light2D 오브젝트
    public SpriteRenderer sky; // sky 스프라이트
    public Sprite[] skySprites; // 아침, 정오, 노을, 저녁, 밤 스프라이트 배열

    private float morningIntensity = 0.5f;
    private float noonIntensity = 0.7f;
    private float sunsetIntensity = 0.7f;
    private float eveningIntensity = 0.5f;
    private float nightIntensity = 0.3f;

    // 수정된 아침과 밤 컬러
    private Color morningColor = new Color(1.0f, 0.9f, 0.7f); // 흰색에 가까운 오렌지/노란색
    private Color noonColor = Color.white; // 흰색
    private Color sunsetColor = new Color(1.0f, 0.3f, 0.3f); // 붉은색
    private Color eveningColor = new Color(0.4f, 0.4f, 0.7f); // 저녁빛
    private Color nightColor = new Color(0.3f, 0.3f, 0.6f); // 덜 어두운 파란색

    void Start()
    {
        UpdateSunlight();
    }

    void Update()
    {
        UpdateSunlight();
    }

    void UpdateSunlight()
    {
        DateTime now = DateTime.Now;
        int hour = now.Hour;

        if (hour >= 6 && hour < 10) // 아침
        {
            SetLightAndSprite(morningColor, morningIntensity, skySprites[0]);
        }
        else if (hour >= 10 && hour < 16) // 정오
        {
            SetLightAndSprite(noonColor, noonIntensity, skySprites[1]);
        }
        else if (hour >= 16 && hour < 18) // 노을
        {
            SetLightAndSprite(sunsetColor, sunsetIntensity, skySprites[2]);
        }
        else if (hour >= 18 && hour < 20) // 저녁
        {
            SetLightAndSprite(eveningColor, eveningIntensity, skySprites[3]);
        }
        else // 밤
        {
            SetLightAndSprite(nightColor, nightIntensity, skySprites[4]);
        }
    }

    void SetLightAndSprite(Color color, float intensity, Sprite newSprite)
    {
        lightSun.color = color;
        lightSun.intensity = intensity;

        if (sky.sprite != newSprite)
        {
            sky.sprite = newSprite;
        }
    }
}
