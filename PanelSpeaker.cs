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
    public Button repeatButton; // 반복 재생 버튼
    public Button buttonClose; // 닫기 버튼
    public Image buttonImage; // 재생/일시정지 버튼 아이콘 이미지
    public Sprite playIcon; // 재생 아이콘
    public Sprite pauseIcon; // 일시정지 아이콘
    public Image shuffleButtonImage; // 셔플 버튼 아이콘 이미지
    public Image repeatButtonImage; // 반복 재생 버튼 아이콘 이미지
    public Sprite shuffleIcon; // 셔플 아이콘
    public Sprite noShuffleIcon; // 비셔플 아이콘
    public Sprite repeatIcon; // 반복 재생 아이콘
    public Sprite noRepeatIcon; // 비반복 재생 아이콘
    public Slider bgmSlider; // BGM 볼륨 조절 슬라이더
    public Button bgmMuteButton; // BGM 뮤트 버튼
    public Button sfxMuteButton; // SFX 뮤트 버튼
    public Sprite muteIcon; // 뮤트 아이콘
    public Sprite unmuteIcon; // 비뮤트 아이콘
    public Sprite sfxMuteIcon; // SFX 뮤트 아이콘
    public Sprite sfxUnmuteIcon; // SFX 비뮤트 아이콘
    public Slider sfxSlider; // SFX 볼륨 조절 슬라이더

    private List<string> playlist = new List<string>();
    private List<Button> buttons = new List<Button>();
    private int currentTrackIndex = 0;
    private bool isShuffling = false;
    private bool isRepeating = false; // 반복 재생 상태
    private string selectedFilePath;
    private AudioSource bgmAudioSource;
    private AudioSource sfxAudioSource;
    private bool isMuted = false;
    private bool isSfxMuted = false;
    public string musicFolderPath { get; private set; }

    private const string BgmVolumePrefKey = "BgmVolume";
    private const string BgmMutePrefKey = "BgmMute";
    private const string SfxVolumePrefKey = "SfxVolume";
    private const string SfxMutePrefKey = "SfxMute";

    private Button currentPlayingButton;

    private void Start()
    {
        buttonPlayPause.onClick.AddListener(TogglePlayPause);
        nextButton.onClick.AddListener(PlayNextTrack);
        prevButton.onClick.AddListener(PlayPreviousTrack);
        shuffleButton.onClick.AddListener(ToggleShuffle);
        repeatButton.onClick.AddListener(ToggleRepeat); // 반복 재생 버튼 이벤트 추가
        buttonClose.onClick.AddListener(ClosePanelSpeaker); // Close 버튼 이벤트 추가
        bgmSlider.onValueChanged.AddListener(SetBgmVolume); // BGM 슬라이더 이벤트 추가
        bgmMuteButton.onClick.AddListener(ToggleBgmMute); // BGM 뮤트 버튼 이벤트 추가
        sfxSlider.onValueChanged.AddListener(SetSfxVolume); // SFX 슬라이더 이벤트 추가
        sfxMuteButton.onClick.AddListener(ToggleSfxMute); // SFX 뮤트 버튼 이벤트 추가
    }

    public void InitializeSpeaker()
    {
        GameObject bgmObject = GameObject.FindGameObjectWithTag("BGM");
        if (bgmObject != null)
        {
            bgmAudioSource = bgmObject.GetComponent<AudioSource>();
            if (bgmAudioSource == null)
            {
                bgmAudioSource = bgmObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Debug.LogError("BGM 오브젝트를 찾을 수 없습니다. BGM 태그가 설정된 오브젝트를 확인해주세요.");
        }

        GameObject sfxObject = GameObject.FindGameObjectWithTag("SFX");
        if (sfxObject != null)
        {
            sfxAudioSource = sfxObject.GetComponent<AudioSource>();
            if (sfxAudioSource == null)
            {
                sfxAudioSource = sfxObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Debug.LogError("SFX 오브젝트를 찾을 수 없습니다. SFX 태그가 설정된 오브젝트를 확인해주세요.");
        }

        musicFolderPath = Path.Combine(Application.persistentDataPath, "Music");

        // 음악 폴더가 없으면 생성
        if (!Directory.Exists(musicFolderPath))
        {
            Directory.CreateDirectory(musicFolderPath);
        }

        Debug.Log("Music folder path: " + musicFolderPath); // 경로 확인을 위한 디버그 로그

        LoadMusicFiles();

        // PlayerPrefs에서 BGM 볼륨과 뮤트 상태를 불러옴
        if (bgmAudioSource != null)
        {
            float savedBgmVolume = PlayerPrefs.GetFloat(BgmVolumePrefKey, bgmAudioSource.volume);
            bool savedBgmMute = PlayerPrefs.GetInt(BgmMutePrefKey, 0) == 1;

            bgmSlider.value = savedBgmVolume;
            bgmAudioSource.volume = savedBgmVolume;

            isMuted = savedBgmMute;
            bgmAudioSource.mute = isMuted;
            UpdateBgmMuteIcon();
        }

        // PlayerPrefs에서 SFX 볼륨과 뮤트 상태를 불러옴
        if (sfxAudioSource != null)
        {
            float savedSfxVolume = PlayerPrefs.GetFloat(SfxVolumePrefKey, sfxAudioSource.volume);
            bool savedSfxMute = PlayerPrefs.GetInt(SfxMutePrefKey, 0) == 1;

            sfxSlider.value = savedSfxVolume;
            sfxAudioSource.volume = savedSfxVolume;

            isSfxMuted = savedSfxMute;
            sfxAudioSource.mute = isSfxMuted;
            UpdateSfxMuteIcon();
        }

        // 셔플 버튼을 비셔플 상태로 초기화
        shuffleButtonImage.sprite = noShuffleIcon;

        // 반복 재생 버튼을 비반복 상태로 초기화
        repeatButtonImage.sprite = noRepeatIcon;
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
        if (bgmAudioSource.isPlaying)
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
                bgmAudioSource.clip = DownloadHandlerAudioClip.GetContent(www);
                bgmAudioSource.Play();

                // 반복 재생을 위해 OnTrackEnd 코루틴 시작
                StartCoroutine(OnTrackEnd(bgmAudioSource.clip.length));
            }
        }
    }

    private IEnumerator OnTrackEnd(float trackLength)
    {
        yield return new WaitForSeconds(trackLength);
        if (isRepeating)
        {
            bgmAudioSource.Play();
            StartCoroutine(OnTrackEnd(trackLength)); // 다시 재생
        }
        else
        {
            PlayNextTrack(); // 다음 곡 재생
        }
    }

    public void PauseMusic()
    {
        bgmAudioSource.Pause();
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

    public void ToggleRepeat()
    {
        isRepeating = !isRepeating;
        repeatButtonImage.sprite = isRepeating ? repeatIcon : noRepeatIcon;
    }

    private void ClosePanelSpeaker()
    {
        gameObject.SetActive(false); // 패널을 비활성화합니다.
    }

    private void SetBgmVolume(float volume)
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = volume;
            PlayerPrefs.SetFloat(BgmVolumePrefKey, volume);
        }
    }

    private void ToggleBgmMute()
    {
        if (bgmAudioSource != null)
        {
            isMuted = !isMuted;
            bgmAudioSource.mute = isMuted;
            PlayerPrefs.SetInt(BgmMutePrefKey, isMuted ? 1 : 0);
            UpdateBgmMuteIcon();
        }
    }

    private void UpdateBgmMuteIcon()
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

    private void SetSfxVolume(float volume)
    {
        if (sfxAudioSource != null)
        {
            sfxAudioSource.volume = volume;
            PlayerPrefs.SetFloat(SfxVolumePrefKey, volume);
        }
    }

    private void ToggleSfxMute()
    {
        if (sfxAudioSource != null)
        {
            isSfxMuted = !isSfxMuted;
            sfxAudioSource.mute = isSfxMuted;
            PlayerPrefs.SetInt(SfxMutePrefKey, isSfxMuted ? 1 : 0);
            UpdateSfxMuteIcon();
        }
    }

    private void UpdateSfxMuteIcon()
    {
        if (isSfxMuted)
        {
            sfxMuteButton.image.sprite = sfxMuteIcon;
        }
        else
        {
            sfxMuteButton.image.sprite = sfxUnmuteIcon;
        }
    }
}
