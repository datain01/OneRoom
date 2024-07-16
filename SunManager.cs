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
        float t = (now.Minute + now.Second / 60f) / 60f; // 시간 내의 비율

        if (hour >= 6 && hour < 12) // 아침
        {
            SetLightAndSprite(morningColor, noonColor, morningIntensity, noonIntensity, skySprites[0]);
        }
        else if (hour >= 12 && hour < 18) // 정오
        {
            SetLightAndSprite(noonColor, eveningColor, noonIntensity, eveningIntensity, skySprites[1]);
        }
        else if (hour >= 18 && hour < 21) // 저녁
        {
            SetLightAndSprite(eveningColor, nightColor, eveningIntensity, nightIntensity, skySprites[2]);
        }
        else // 밤
        {
            int nextHour = (hour + 1) % 24;
            Color nextColor = (nextHour >= 6 && nextHour < 12) ? morningColor : nightColor;
            float nextIntensity = (nextHour >= 6 && nextHour < 12) ? morningIntensity : nightIntensity;
            Sprite nextSprite = (nextHour >= 6 && nextHour < 12) ? skySprites[0] : skySprites[3];

            SetLightAndSprite(nightColor, nextColor, nightIntensity, nextIntensity, skySprites[3]);
        }
    }

    void SetLightAndSprite(Color startColor, Color endColor, float startIntensity, float endIntensity, Sprite newSprite)
    {
        DateTime now = DateTime.Now;
        float t = (now.Minute + now.Second / 60f) / 60f;

        lightSun.color = Color.Lerp(startColor, endColor, t);
        lightSun.intensity = Mathf.Lerp(startIntensity, endIntensity, t);

        if (sky.sprite != newSprite)
        {
            sky.sprite = newSprite;
        }
    }
}
