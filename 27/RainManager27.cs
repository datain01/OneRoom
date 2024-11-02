using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System;

public class RainManager27 : MonoBehaviour
{
    public Light2D lightSun;
    public SpriteRenderer sky;
    public SpriteRenderer skyEffect; // 비 효과 스프라이트 렌더러 추가
    public Sprite[] skySprites; // 맑은 날의 아침, 정오, 석양, 저녁, 밤 스프라이트 배열
    public Sprite[] rainSprites; // 비 오는 날의 아침, 정오, 석양, 저녁, 밤 스프라이트 배열
    public Sprite[] rainEffectSprites; // 비 효과 스프라이트 배열
    public float rainEffectSpeed = 0.1f;

    public enum WeatherState { Clear, Rain }
    public WeatherState currentWeather = WeatherState.Clear;

    private Coroutine weatherEffectCoroutine;
    private int currentHour = -1;
    private string lastCheckedDate;

    void Start()
    {
        LoadWeatherState(); // 마지막으로 저장된 상태 로드
        UpdateSunlight();
        UpdateWeatherEffect();
    }

    void Update()
    {
        DateTime now = DateTime.Now;
        int hour = now.Hour;
        string currentDate = now.ToString("yyyy-MM-dd");

        // 날짜가 변경되었는지 확인
        if (currentDate != lastCheckedDate)
        {
            DetermineWeather(); // 날짜 변경 시 새로운 날씨 결정
            lastCheckedDate = currentDate;
        }

        // 시간 변화 시 즉시 하늘 업데이트
        if (hour != currentHour)
        {
            currentHour = hour;
            UpdateSunlight();
        }
    }

    public void SetWeatherState(WeatherState newWeather)
    {
        Debug.Log($"Setting weather state to {newWeather}");
        currentWeather = newWeather;
        SaveWeatherState();
        UpdateSunlight();
        UpdateWeatherEffect();
    }

    void DetermineWeather()
    {
        currentWeather = (UnityEngine.Random.Range(0, 10) < 1) ? WeatherState.Rain : WeatherState.Clear;
        // 50% 확률로 날씨 변경
        // currentWeather = (UnityEngine.Random.Range(0, 2) == 0) ? WeatherState.Rain : WeatherState.Clear;

        SaveWeatherState();
        UpdateSunlight();
        UpdateWeatherEffect();
    }

    void UpdateSunlight()
    {
        Sprite currentSprite;

        // 현재 시간대에 맞는 스프라이트 선택
        if (currentHour >= 6 && currentHour < 10) // 아침
        {
            currentSprite = (currentWeather == WeatherState.Rain) ? rainSprites[0] : skySprites[0];
            SetLightAndSprite(0.5f, currentSprite); // 아침 빛 설정
        }
        else if (currentHour >= 10 && currentHour < 16) // 정오
        {
            currentSprite = (currentWeather == WeatherState.Rain) ? rainSprites[1] : skySprites[1];
            SetLightAndSprite(1.0f, currentSprite); // 정오 빛 설정
        }
        else if (currentHour >= 16 && currentHour < 18) // 석양
        {
            currentSprite = (currentWeather == WeatherState.Rain) ? rainSprites[2] : skySprites[2];
            SetLightAndSprite(0.7f, currentSprite); // 석양 빛 설정
        }
        else if (currentHour >= 18 && currentHour < 20) // 저녁
        {
            currentSprite = (currentWeather == WeatherState.Rain) ? rainSprites[3] : skySprites[3];
            SetLightAndSprite(0.5f, currentSprite); // 저녁 빛 설정
        }
        else // 밤
        {
            currentSprite = (currentWeather == WeatherState.Rain) ? rainSprites[4] : skySprites[4];
            SetLightAndSprite(0.2f, currentSprite); // 밤 빛 설정
       }
    }

    void SetLightAndSprite(float intensity, Sprite newSprite)
    {
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

        // 비 오는 날씨일 때 비 효과 실행
        if (currentWeather == WeatherState.Rain)
        {
            weatherEffectCoroutine = StartCoroutine(PlayWeatherEffect(rainEffectSprites, rainEffectSpeed));
        }
        else
        {
            skyEffect.sprite = null; // 맑은 날씨일 때 비 효과 제거
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

    void SaveWeatherState()
    {
        // 현재 날씨 상태와 날짜 저장
        PlayerPrefs.SetInt("SavedWeather", (int)currentWeather);
        PlayerPrefs.SetString("LastCheckedDate", DateTime.Now.ToString("yyyy-MM-dd"));
    }

    void LoadWeatherState()
    {
        // 마지막으로 저장된 날씨 상태와 날짜 로드
        lastCheckedDate = PlayerPrefs.GetString("LastCheckedDate", DateTime.Now.ToString("yyyy-MM-dd"));
        currentWeather = (WeatherState)PlayerPrefs.GetInt("SavedWeather", 0);
        
       
        UpdateSunlight(); // 저장된 상태를 즉시 반영
        UpdateWeatherEffect();
    }
}
