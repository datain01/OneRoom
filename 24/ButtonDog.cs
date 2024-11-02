using UnityEngine;
using UnityEngine.UI;

public class ButtonDog : MonoBehaviour
{
    public SunManager24 sunManager; // SunManager24 스크립트 참조
    public Button buttonDog; // 버튼 오브젝트 참조

    void Start()
    {
        if (buttonDog != null)
        {
            buttonDog.onClick.AddListener(ToggleWeather); // 버튼 클릭 시 ToggleWeather 메서드 호출
        }
    }

    void ToggleWeather()
    {
        if (sunManager.currentWeather == SunManager24.WeatherState.Normal)
        {
            sunManager.SetWeatherState(SunManager24.WeatherState.Snow); // 날씨를 Snow로 변경
        }
        else
        {
            sunManager.SetWeatherState(SunManager24.WeatherState.Normal); // 날씨를 Normal로 변경
        }
    }
}
