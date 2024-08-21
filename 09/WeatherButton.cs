using UnityEngine;
using UnityEngine.UI;

public class WeatherButton : MonoBehaviour
{
    public Image weatherButtonImage;          // Weather 버튼의 Image 컴포넌트
    public Sprite normalImage;                // Normal 상태일 때의 기본 이미지
    public Sprite rainImage;                  // Rain 상태일 때의 기본 이미지
    public Sprite snowImage;                  // Snow 상태일 때의 기본 이미지

    public SunManager9 sunManager;            // SunManager9 스크립트를 참조
    private int currentState = 0;             // 현재 날씨 상태를 나타내는 인덱스

    void Start()
    {
        UpdateWeather(); // 초기 상태 업데이트
    }

    public void OnButtonClick()
    {
        ToggleWeather();
    }

    void ToggleWeather()
    {
        // 상태 순환: 0 -> 1 -> 2 -> 0 -> ...
        currentState = (currentState + 1) % 3;
        UpdateWeather();
    }

    void UpdateWeather()
    {
        switch (currentState)
        {
            case 0: // Normal 상태
                weatherButtonImage.sprite = normalImage;
                sunManager.SetWeatherState(SunManager9.WeatherState.Normal);
                break;
            case 1: // Rain 상태
                weatherButtonImage.sprite = rainImage;
                sunManager.SetWeatherState(SunManager9.WeatherState.Rain);
                break;
            case 2: // Snow 상태
                weatherButtonImage.sprite = snowImage;
                sunManager.SetWeatherState(SunManager9.WeatherState.Snow);
                break;
        }
    }
}
