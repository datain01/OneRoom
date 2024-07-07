using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelOptionManager : MonoBehaviour
{
    public GameObject panelOption; // PanelOption을 참조
    public TMP_Dropdown resolutionDropdown; // 해상도 드롭다운을 참조
    public Button fullscreenButton; // 전체화면/창모드 전환 버튼을 참조
    public Sprite fullscreenIcon; // 전체화면 아이콘
    public Sprite windowedIcon; // 창모드 아이콘
    private bool isFullscreen = true; // 현재 전체화면 상태를 저장

    private void Start()
    {
        // PanelOption을 비활성화 상태로 설정
        panelOption.SetActive(false);

        // 드롭다운 이벤트 리스너 추가
        resolutionDropdown.onValueChanged.AddListener(delegate { SetResolution(resolutionDropdown.value); });

        // 전체화면/창모드 버튼 이벤트 리스너 추가
        fullscreenButton.onClick.AddListener(ToggleFullscreen);

        // 드롭다운 옵션 설정
        SetDropdownOptions();

        // 초기 아이콘 설정
        UpdateFullscreenIcon();
    }

    private void Update()
    {
        // ESC 키를 누르면 PanelOption 활성화/비활성화
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            panelOption.SetActive(!panelOption.activeSelf);
        }
    }

    private void SetDropdownOptions()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>
        {
            new TMP_Dropdown.OptionData("1920x1080"),
            new TMP_Dropdown.OptionData("1600x900"),
            new TMP_Dropdown.OptionData("1366x768"),
            new TMP_Dropdown.OptionData("1280x720")
        };
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
    }

    private void SetResolution(int index)
    {
        switch (index)
        {
            case 0:
                Screen.SetResolution(1920, 1080, isFullscreen);
                break;
            case 1:
                Screen.SetResolution(1600, 900, isFullscreen);
                break;
            case 2:
                Screen.SetResolution(1366, 768, isFullscreen);
                break;
            case 3:
                Screen.SetResolution(1280, 720, isFullscreen);
                break;
        }
    }

    private void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
        Screen.fullScreen = isFullscreen;
        UpdateFullscreenIcon();
    }

    private void UpdateFullscreenIcon()
    {
        if (isFullscreen)
        {
            fullscreenButton.GetComponent<Image>().sprite = fullscreenIcon;
        }
        else
        {
            fullscreenButton.GetComponent<Image>().sprite = windowedIcon;
        }
    }
}
