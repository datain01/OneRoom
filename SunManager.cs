using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class SunManager : MonoBehaviour
{
    public Light2D lightSun; // Light2D 오브젝트
    private float morningIntensity = 0.5f;
    private float noonIntensity = 1.0f;
    private float eveningIntensity = 0.7f;
    private float nightIntensity = 0.2f;
    private Color morningColor = new Color(1.0f, 0.5f, 0.3f); // 오렌지색
    private Color noonColor = Color.white; // 흰색
    private Color eveningColor = new Color(1.0f, 0.3f, 0.3f); // 붉은색
    private Color nightColor = new Color(0.2f, 0.2f, 0.5f); // 어두운 파란색

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
            lightSun.color = Color.Lerp(morningColor, noonColor, t);
            lightSun.intensity = Mathf.Lerp(morningIntensity, noonIntensity, t);
        }
        else if (hour >= 12 && hour < 16) // 정오
        {
            lightSun.color = Color.Lerp(noonColor, eveningColor, t);
            lightSun.intensity = Mathf.Lerp(noonIntensity, eveningIntensity, t);
        }
        else if (hour >= 16 && hour < 18) // 저녁
        {
            lightSun.color = Color.Lerp(eveningColor, nightColor, t);
            lightSun.intensity = Mathf.Lerp(eveningIntensity, nightIntensity, t);
        }
        else // 밤
        {
            int nextHour = (hour + 1) % 24;
            Color nextColor = (nextHour >= 6 && nextHour < 12) ? morningColor : nightColor;
            float nextIntensity = (nextHour >= 6 && nextHour < 12) ? morningIntensity : nightIntensity;

            lightSun.color = Color.Lerp(nightColor, nextColor, t);
            lightSun.intensity = Mathf.Lerp(nightIntensity, nextIntensity, t);
        }
    }
}
