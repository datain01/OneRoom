using UnityEngine;
using UnityEngine.UI;

public class WeatherButton22 : MonoBehaviour
{
    public Image weatherButtonImage;          // Weather 버튼의 Image 컴포넌트
    public Sprite normalImage;                // Normal 상태일 때의 기본 이미지
    public Sprite rainImage;                  // Rain 상태일 때의 기본 이미지
    public Sprite snowImage;                  // Snow 상태일 때의 기본 이미지
    public Sprite cloudyImage;                // Cloudy 상태일 때의 기본 이미지

    public SunManager22 sunManager;           // SunManager22 스크립트를 참조
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
        // 상태 순환: 0 -> 1 -> 2 -> 3 -> 0 -> ...
        currentState = (currentState + 1) % 4;
        UpdateWeather();
    }

    void UpdateWeather()
    {
        switch (currentState)
        {
            case 0: // Normal 상태
                weatherButtonImage.sprite = normalImage;
                sunManager.SetWeatherState(SunManager22.WeatherState.Normal);
                break;
            case 1: // Rain 상태
                weatherButtonImage.sprite = rainImage;
                sunManager.SetWeatherState(SunManager22.WeatherState.Rain);
                break;
            case 2: // Snow 상태
                weatherButtonImage.sprite = snowImage;
                sunManager.SetWeatherState(SunManager22.WeatherState.Snow);
                break;
            case 3: // Cloudy 상태
                weatherButtonImage.sprite = cloudyImage;
                sunManager.SetWeatherState(SunManager22.WeatherState.Cloudy);
                break;
        }
    }
}
