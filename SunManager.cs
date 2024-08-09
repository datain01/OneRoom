using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class SunManager : MonoBehaviour
{
    public Light2D lightSun; // Light2D 오브젝트
    public SpriteRenderer sky; // sky 스프라이트
    public Sprite[] skySprites; // 아침, 정오, 저녁, 밤 스프라이트 배열

    private float morningIntensity = 0.5f;
    private float noonIntensity = 1.0f;
    private float eveningIntensity = 0.7f;
    private float nightIntensity = 0.2f;
    private Color morningColor = new Color(1.0f, 0.5f, 0.3f); // 오렌지색
    private Color noonColor = Color.white; // 흰색
    private Color eveningColor = new Color(1.0f, 0.3f, 0.3f); // 붉은색
    private Color nightColor = new Color(0.2f, 0.2f, 0.5f); // 어두운 파란색

    private Sprite currentSprite;

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

        if (hour >= 6 && hour < 12) // 아침
        {
            SetLightAndSprite(morningColor, morningIntensity, skySprites[0]);
        }
        else if (hour >= 12 && hour < 16) // 정오
        {
            SetLightAndSprite(noonColor, noonIntensity, skySprites[1]);
        }
        else if (hour >= 16 && hour < 18) // 저녁
        {
            SetLightAndSprite(eveningColor, eveningIntensity, skySprites[2]);
        }
        else // 밤
        {
            SetLightAndSprite(nightColor, nightIntensity, skySprites[3]);
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
