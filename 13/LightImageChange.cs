using UnityEngine;
using UnityEngine.UI;

public class LightImageChange : MonoBehaviour
{
    public Image lightButtonImage;           // UI 버튼의 Image 컴포넌트
    public Sprite lightOffImage;             // LightOff 상태일 때 사용할 이미지
    public Sprite lightOnImage;              // LightOn 상태일 때 사용할 이미지

    public void UpdateLightState(bool isLightOn)
    {
        // LightOn 상태에 따라 이미지를 변경
        if (isLightOn)
        {
            lightButtonImage.sprite = lightOnImage;
        }
        else
        {
            lightButtonImage.sprite = lightOffImage;
        }
    }
}
