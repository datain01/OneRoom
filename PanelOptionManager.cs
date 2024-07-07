using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

public class PanelOptionManager : MonoBehaviour
{
    public GameObject panelOption; // PanelOption을 참조
    public TMP_Dropdown resolutionDropdown; // 해상도 드롭다운을 참조
    public TMP_Dropdown programDropdown; // 실행 중인 프로그램 드롭다운을 참조
    public Button fullscreenButton; // 전체화면/창모드 전환 버튼을 참조
    public Sprite fullscreenIcon; // 전체화면 아이콘
    public Sprite windowedIcon; // 창모드 아이콘
    private bool isFullscreen = true; // 현재 전체화면 상태를 저장
    public StopwatchManager stopwatchManager; // 스톱워치 매니저 참조
    private string selectedProgram = "none"; // 선택된 프로그램

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    private static readonly HashSet<string> systemProcessNames = new HashSet<string>
    {
        "Idle", "System", "Registry", "smss", "csrss", "wininit", "services", "lsass", "svchost", "winlogon", "dwm", "spoolsv", "taskhost", "explorer", "SearchIndexer", "MsMpEng", "dllhost", "conhost", "sihost", "taskeng", "ctfmon", "audiodg", "rundll32"
    };

    private void Start()
    {
        // PanelOption을 비활성화 상태로 설정
        panelOption.SetActive(false);

        // 드롭다운 이벤트 리스너 추가
        resolutionDropdown.onValueChanged.AddListener(delegate { SetResolution(resolutionDropdown.value); });
        programDropdown.onValueChanged.AddListener(delegate { SetProgram(programDropdown.value); });

        // 전체화면/창모드 버튼 이벤트 리스너 추가
        fullscreenButton.onClick.AddListener(ToggleFullscreen);

        // 드롭다운 옵션 설정
        SetResolutionDropdownOptions();
        SetProgramDropdownOptions();

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

        // 포커스된 프로그램 확인
        CheckFocusedProgram();
    }

    private void SetResolutionDropdownOptions()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>
        {
            new TMP_Dropdown.OptionData("1920x1080"),
            new TMP_Dropdown.OptionData("1600x900"),
            new TMP_Dropdown.OptionData("1366x768"),
            new TMP_Dropdown.OptionData("1280x720"),
            new TMP_Dropdown.OptionData("1024x576"),
            new TMP_Dropdown.OptionData("960x540"),
            new TMP_Dropdown.OptionData("800x450"),
            new TMP_Dropdown.OptionData("640x360"),
            new TMP_Dropdown.OptionData("480x270"),
            new TMP_Dropdown.OptionData("320x180")
        };
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
    }

    private void SetProgramDropdownOptions()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData> { new TMP_Dropdown.OptionData("none") };

        var runningProcesses = GetTaskbarProcesses();
        var filteredProcesses = runningProcesses
            .Where(p => !systemProcessNames.Contains(p.ProcessName)) // 시스템 프로세스 제외
            .GroupBy(p => p.ProcessName)
            .Select(g => new TMP_Dropdown.OptionData(g.Key))
            .OrderBy(p => p.text)
            .ToList();

        options.AddRange(filteredProcesses);
        programDropdown.ClearOptions();
        programDropdown.AddOptions(options);
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
            case 4:
                Screen.SetResolution(1024, 576, isFullscreen);
                break;
            case 5:
                Screen.SetResolution(960, 540, isFullscreen);
                break;
            case 6:
                Screen.SetResolution(800, 450, isFullscreen);
                break;
            case 7:
                Screen.SetResolution(640, 360, isFullscreen);
                break;
            case 8:
                Screen.SetResolution(480, 270, isFullscreen);
                break;
            case 9:
                Screen.SetResolution(320, 180, isFullscreen);
                break;
        }
    }

    private void SetProgram(int index)
    {
        selectedProgram = programDropdown.options[index].text;
        if (selectedProgram != "none")
        {
            stopwatchManager.SetRunning(false);
        }
    }

    private List<Process> GetTaskbarProcesses()
    {
        List<Process> taskbarProcesses = new List<Process>();
        EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
        {
            if (IsWindowVisible(hWnd) && GetWindowTextLength(hWnd) > 0)
            {
                uint processId;
                GetWindowThreadProcessId(hWnd, out processId);
                if (processId != 0)
                {
                    try
                    {
                        Process process = Process.GetProcessById((int)processId);
                        if (!systemProcessNames.Contains(process.ProcessName))
                        {
                            taskbarProcesses.Add(process);
                        }
                    }
                    catch (ArgumentException) { }
                }
            }
            return true;
        }, IntPtr.Zero);

        return taskbarProcesses;
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    private void CheckFocusedProgram()
    {
        if (selectedProgram == "none") return;

        IntPtr hWnd = GetForegroundWindow();
        uint processId;
        GetWindowThreadProcessId(hWnd, out processId);

        if (processId == 0) return;

        Process foregroundProcess = Process.GetProcessById((int)processId);
        if (foregroundProcess.ProcessName == selectedProgram)
        {
            stopwatchManager.SetRunning(true);
        }
        else
        {
            stopwatchManager.SetRunning(false);
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
