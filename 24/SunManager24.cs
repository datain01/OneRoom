using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System;

public class SunManager24 : MonoBehaviour
{
    public Light2D lightSun; // Light2D 오브젝트
    public SpriteRenderer sky; // sky 스프라이트
    public SpriteRenderer skyEffect; // Snow 효과를 위한 2D 스프라이트 렌더러
    public Sprite[] skySprites; // 아침, 정오, 노을, 저녁, 밤 스프라이트 배열
    public Sprite[] snowSprites; // 눈 올 때 사용할 스프라이트 배열
    public Sprite[] snowEffectSprites; // 눈 효과 스프라이트 배열
    public float snowEffectSpeed = 0.1f; // 눈 애니메이션 속도

    public enum WeatherState { Normal, Snow }
    public WeatherState currentWeather = WeatherState.Normal;

    private float morningIntensity = 0.5f;
    private float noonIntensity = 1.0f;
    private float sunsetIntensity = 0.7f;
    private float eveningIntensity = 0.5f;
    private float nightIntensity = 0.2f;

    private Color morningColor = new Color(1.0f, 0.9f, 0.7f); // 흰색에 가까운 오렌지/노란색
    private Color noonColor = Color.white; // 흰색
    private Color sunsetColor = new Color(1.0f, 0.3f, 0.3f); // 붉은색
    private Color eveningColor = new Color(0.7f, 0.4f, 0.6f); // 저녁빛
    private Color nightColor = new Color(0.3f, 0.3f, 0.6f); // 덜 어두운 파란색

    private Coroutine weatherEffectCoroutine; // 현재 재생 중인 날씨 효과 애니메이션 코루틴
    private int currentHour = -1; // 현재 시간대를 저장하는 변수

    void Start()
    {
        UpdateSunlight();
        UpdateWeatherEffect(); // 시작할 때 날씨 효과 업데이트
    }

    void Update()
    {
        DateTime now = DateTime.Now;
        int hour = now.Hour;

        if (hour != currentHour)
        {
            currentHour = hour;
            UpdateSunlight();
        }
    }

    public void SetWeatherState(WeatherState newWeatherState)
    {
        currentWeather = newWeatherState;
        UpdateSunlight(); // 날씨 변경 시 즉시 업데이트
        UpdateWeatherEffect(); // 날씨 효과 업데이트
    }

    void UpdateSunlight()
    {
        Sprite[] currentSprites = GetCurrentSprites();

        if (currentHour >= 6 && currentHour < 10) // 아침
        {
            SetLightAndSprite(morningColor, morningIntensity, currentSprites[0]);
        }
        else if (currentHour >= 10 && currentHour < 16) // 정오
        {
            SetLightAndSprite(noonColor, noonIntensity, currentSprites[1]);
        }
        else if (currentHour >= 16 && currentHour < 18) // 노을
        {
            SetLightAndSprite(sunsetColor, sunsetIntensity, currentSprites[2]);
        }
        else if (currentHour >= 18 && currentHour < 20) // 저녁
        {
            SetLightAndSprite(eveningColor, eveningIntensity, currentSprites[3]);
        }
        else // 밤
        {
            SetLightAndSprite(nightColor, nightIntensity, currentSprites[4]);
        }
    }

    Sprite[] GetCurrentSprites()
    {
        switch (currentWeather)
        {
            case WeatherState.Snow:
                return snowSprites;
            case WeatherState.Normal:
            default:
                return skySprites;
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

    void UpdateWeatherEffect()
    {
        if (weatherEffectCoroutine != null)
        {
            StopCoroutine(weatherEffectCoroutine);
        }

        switch (currentWeather)
        {
            case WeatherState.Snow:
                weatherEffectCoroutine = StartCoroutine(PlayWeatherEffect(snowEffectSprites, snowEffectSpeed));
                break;
            case WeatherState.Normal:
            default:
                skyEffect.sprite = null; // Normal 상태에서는 효과 제거
                break;
        }
    }

    IEnumerator PlayWeatherEffect(Sprite[] effectSprites, float effectSpeed)
    {
        int currentFrame = 0;

        while (true)
        {
            skyEffect.sprite = effectSprites[currentFrame];
            currentFrame = (currentFrame + 1) % effectSprites.Length;
            yield return new WaitForSeconds(effectSpeed);
        }
    }
}
