using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class PanelSpeaker : MonoBehaviour
{
    public GameObject playlistPanel; // Content 오브젝트를 할당
    public GameObject musicItemPrefab;
    public Button buttonPlayPause;
    public Button nextButton;
    public Button prevButton;
    public Button shuffleButton;
    public Button buttonClose; // 닫기 버튼
    public Image buttonImage; // 재생/일시정지 버튼 아이콘 이미지
    public Sprite playIcon; // 재생 아이콘
    public Sprite pauseIcon; // 일시정지 아이콘
    public Image shuffleButtonImage; // 셔플 버튼 아이콘 이미지
    public Sprite shuffleIcon; // 셔플 아이콘
    public Sprite noShuffleIcon; // 비셔플 아이콘
    public Slider bgmSlider; // BGM 볼륨 조절 슬라이더
    public Button bgmMuteButton; // BGM 뮤트 버튼
    public Sprite muteIcon; // 뮤트 아이콘
    public Sprite unmuteIcon; // 비뮤트 아이콘

    private List<string> playlist = new List<string>();
    private List<Button> buttons = new List<Button>();
    private int currentTrackIndex = 0;
    private bool isShuffling = false;
    private string selectedFilePath;
    private AudioSource audioSource;
    private bool isMuted = false;
    public string musicFolderPath { get; private set; }

    private const string VolumePrefKey = "BgmVolume";
    private const string MutePrefKey = "BgmMute";

    private Button currentPlayingButton;

    private void Start()
    {
        buttonPlayPause.onClick.AddListener(TogglePlayPause);
        nextButton.onClick.AddListener(PlayNextTrack);
        prevButton.onClick.AddListener(PlayPreviousTrack);
        shuffleButton.onClick.AddListener(ToggleShuffle);
        buttonClose.onClick.AddListener(ClosePanelSpeaker); // Close 버튼 이벤트 추가
        bgmSlider.onValueChanged.AddListener(SetBgmVolume); // 슬라이더 이벤트 추가
        bgmMuteButton.onClick.AddListener(ToggleMute); // 뮤트 버튼 이벤트 추가
    }

    public void InitializeSpeaker()
    {
        GameObject bgmObject = GameObject.FindGameObjectWithTag("BGM");
        if (bgmObject != null)
        {
            audioSource = bgmObject.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = bgmObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Debug.LogError("BGM 오브젝트를 찾을 수 없습니다. BGM 태그가 설정된 오브젝트를 확인해주세요.");
        }

        musicFolderPath = Path.Combine(Application.persistentDataPath, "Music");

        // 음악 폴더가 없으면 생성
        if (!Directory.Exists(musicFolderPath))
        {
            Directory.CreateDirectory(musicFolderPath);
        }

        Debug.Log("Music folder path: " + musicFolderPath); // 경로 확인을 위한 디버그 로그

        LoadMusicFiles();

        // PlayerPrefs에서 볼륨과 뮤트 상태를 불러옴
        if (audioSource != null)
        {
            float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, audioSource.volume);
            bool savedMute = PlayerPrefs.GetInt(MutePrefKey, 0) == 1;

            bgmSlider.value = savedVolume;
            audioSource.volume = savedVolume;

            isMuted = savedMute;
            audioSource.mute = isMuted;
            UpdateMuteIcon();
        }

        // 셔플 버튼을 비셔플 상태로 초기화
        shuffleButtonImage.sprite = noShuffleIcon;
    }

    private void LoadMusicFiles()
    {
        // 지정된 폴더에서 .mp3 파일을 검색하여 플레이리스트에 추가
        string[] musicFiles = Directory.GetFiles(musicFolderPath, "*.mp3");
        foreach (string filePath in musicFiles)
        {
            AddMusicToPlaylist(filePath);
        }
    }

    private void AddMusicToPlaylist(string filePath)
    {
        if (!File.Exists(filePath) || Path.GetExtension(filePath) != ".mp3")
        {
            Debug.LogError("Invalid file selected.");
            return;
        }

        playlist.Add(filePath);
        GameObject newItem = Instantiate(musicItemPrefab, playlistPanel.transform);

        TextMeshProUGUI fileNameText = newItem.GetComponentInChildren<TextMeshProUGUI>();
        if (fileNameText != null)
        {
            fileNameText.text = ShortenText(Path.GetFileNameWithoutExtension(filePath), 20); // 20 글자로 제한하고 .mp3 제외
        }

        Button itemButton = newItem.GetComponent<Button>();
        if (itemButton != null)
        {
            itemButton.onClick.AddListener(() => PlaySelectedMusic(filePath, itemButton));
            buttons.Add(itemButton);
        }
    }

    private string ShortenText(string text, int maxLength)
    {
        if (text.Length > maxLength)
        {
            return text.Substring(0, maxLength) + "...";
        }
        return text;
    }

    public void TogglePlayPause()
    {
        if (audioSource.isPlaying)
        {
            PauseMusic();
        }
        else
        {
            if (string.IsNullOrEmpty(selectedFilePath) && playlist.Count > 0)
            {
                PlaySelectedMusic(playlist[0], buttons.Count > 0 ? buttons[0] : null);
            }
            else
            {
                PlaySelectedMusic(selectedFilePath, currentPlayingButton);
            }
        }
    }

    public void PlaySelectedMusic(string filePath, Button button)
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            selectedFilePath = filePath;
            StartCoroutine(PlayTrack(selectedFilePath));
            buttonImage.sprite = pauseIcon;

            if (currentPlayingButton != null)
            {
                // 이전 재생 버튼을 원래 상태로 되돌림
                currentPlayingButton.interactable = true;
            }

            if (button != null)
            {
                currentPlayingButton = button;
                currentPlayingButton.interactable = false;
                EventSystem.current.SetSelectedGameObject(button.gameObject);
            }
        }
    }

    private IEnumerator PlayTrack(string path)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.Play();
            }
        }
    }

    public void PauseMusic()
    {
        audioSource.Pause();
        buttonImage.sprite = playIcon;
    }

    public void PlayNextTrack()
    {
        if (playlist.Count > 0)
        {
            if (isShuffling)
            {
                currentTrackIndex = Random.Range(0, playlist.Count);
            }
            else
            {
                currentTrackIndex = (currentTrackIndex + 1) % playlist.Count;
            }
            selectedFilePath = playlist[currentTrackIndex];
            PlaySelectedMusic(selectedFilePath, buttons.Count > currentTrackIndex ? buttons[currentTrackIndex] : null);
        }
    }

    public void PlayPreviousTrack()
    {
        if (playlist.Count > 0)
        {
            if (isShuffling)
            {
                currentTrackIndex = Random.Range(0, playlist.Count);
            }
            else
            {
                currentTrackIndex = (currentTrackIndex - 1 + playlist.Count) % playlist.Count;
            }
            selectedFilePath = playlist[currentTrackIndex];
            PlaySelectedMusic(selectedFilePath, buttons.Count > currentTrackIndex ? buttons[currentTrackIndex] : null);
        }
    }

    public void ToggleShuffle()
    {
        isShuffling = !isShuffling;
        shuffleButtonImage.sprite = isShuffling ? shuffleIcon : noShuffleIcon;
    }

    private void ClosePanelSpeaker()
    {
        gameObject.SetActive(false); // 패널을 비활성화합니다.
    }

    private void SetBgmVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            PlayerPrefs.SetFloat(VolumePrefKey, volume);
        }
    }

    private void ToggleMute()
    {
        if (audioSource != null)
        {
            isMuted = !isMuted;
            audioSource.mute = isMuted;
            PlayerPrefs.SetInt(MutePrefKey, isMuted ? 1 : 0);
            UpdateMuteIcon();
        }
    }

    private void UpdateMuteIcon()
    {
        if (isMuted)
        {
            bgmMuteButton.image.sprite = muteIcon;
        }
        else
        {
            bgmMuteButton.image.sprite = unmuteIcon;
        }
    }
}
