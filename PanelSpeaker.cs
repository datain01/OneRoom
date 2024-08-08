using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class PanelSpeaker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject playlistPanel;
    public GameObject musicItemPrefab;
    public Button buttonPlayPause;
    public Button nextButton;
    public Button prevButton;
    public Button shuffleButton;
    public Button repeatButton;
    public Image buttonImage;
    public Sprite playIcon;
    public Sprite pauseIcon;
    public Image shuffleButtonImage;
    public Image repeatButtonImage;
    public Sprite shuffleIcon;
    public Sprite noShuffleIcon;
    public Sprite repeatIcon;
    public Sprite noRepeatIcon;
    public Slider bgmSlider;
    public Button bgmMuteButton;
    public Button sfxMuteButton;
    public Sprite muteIcon;
    public Sprite unmuteIcon;
    public Sprite sfxMuteIcon;
    public Sprite sfxUnmuteIcon;
    public Slider sfxSlider;
    public Slider musicProgressSlider;
    public CanvasGroup canvasGroup;

    private List<string> playlist = new List<string>();
    private List<Button> buttons = new List<Button>();
    private List<int> playedIndices = new List<int>();
    private int currentTrackIndex = 0;
    private bool isShuffling = false;
    private bool isRepeating = false;
    private string selectedFilePath;
    private List<AudioSource> bgmAudioSources = new List<AudioSource>();
    private List<AudioSource> sfxAudioSources = new List<AudioSource>();
    private bool isMuted = false;
    private bool isSfxMuted = false;
    public string musicFolderPath { get; private set; }

    private const string BgmVolumePrefKey = "BgmVolume";
    private const string BgmMutePrefKey = "BgmMute";
    private const string SfxVolumePrefKey = "SfxVolume";
    private const string SfxMutePrefKey = "SfxMute";
    private const string ShufflePrefKey = "Shuffle";
    private const string RepeatPrefKey = "Repeat";

    private Button currentPlayingButton;
    private bool isSliderDragging = false;
    private Coroutine trackEndCoroutine;

    private void Start()
    {
        buttonPlayPause.onClick.AddListener(TogglePlayPause);
        nextButton.onClick.AddListener(PlayNextTrack);
        prevButton.onClick.AddListener(PlayPreviousTrack);
        shuffleButton.onClick.AddListener(ToggleShuffle);
        repeatButton.onClick.AddListener(ToggleRepeat);
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        bgmMuteButton.onClick.AddListener(ToggleBgmMute);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        sfxMuteButton.onClick.AddListener(ToggleSfxMute);
        musicProgressSlider.onValueChanged.AddListener(OnMusicProgressSliderChanged);

        // 셔플 및 반복 상태를 PlayerPrefs에서 불러오기
        isShuffling = PlayerPrefs.GetInt(ShufflePrefKey, 0) == 1;
        isRepeating = PlayerPrefs.GetInt(RepeatPrefKey, 0) == 1;
        shuffleButtonImage.sprite = isShuffling ? shuffleIcon : noShuffleIcon;
        repeatButtonImage.sprite = isRepeating ? repeatIcon : noRepeatIcon;
    }

    private void Update()
    {
        foreach (var bgmAudioSource in bgmAudioSources)
        {
            if (bgmAudioSource.isPlaying && !isSliderDragging)
            {
                musicProgressSlider.value = bgmAudioSource.time / bgmAudioSource.clip.length;
            }
        }
    }

    public void InitializeSpeaker()
    {
        GameObject[] bgmObjects = GameObject.FindGameObjectsWithTag("BGM");
        foreach (GameObject bgmObject in bgmObjects)
        {
            AudioSource bgmAudioSource = bgmObject.GetComponent<AudioSource>();
            if (bgmAudioSource == null)
            {
                bgmAudioSource = bgmObject.AddComponent<AudioSource>();
            }
            bgmAudioSources.Add(bgmAudioSource);
        }

        GameObject[] sfxObjects = GameObject.FindGameObjectsWithTag("SFX");
        foreach (GameObject sfxObject in sfxObjects)
        {
            AudioSource sfxAudioSource = sfxObject.GetComponent<AudioSource>();
            if (sfxAudioSource == null)
            {
                sfxAudioSource = sfxObject.AddComponent<AudioSource>();
            }
            sfxAudioSources.Add(sfxAudioSource);
        }

        musicFolderPath = Path.Combine(Application.persistentDataPath, "Music");

        if (!Directory.Exists(musicFolderPath))
        {
            Directory.CreateDirectory(musicFolderPath);
        }

        Debug.Log("Music folder path: " + musicFolderPath);

        LoadMusicFiles();

        foreach (var bgmAudioSource in bgmAudioSources)
        {
            float savedBgmVolume = PlayerPrefs.GetFloat(BgmVolumePrefKey, bgmAudioSource.volume);
            bool savedBgmMute = PlayerPrefs.GetInt(BgmMutePrefKey, 0) == 1;

            bgmSlider.value = savedBgmVolume;
            bgmAudioSource.volume = savedBgmVolume;

            isMuted = savedBgmMute;
            bgmAudioSource.mute = isMuted;
            UpdateBgmMuteIcon();
        }

        foreach (var sfxAudioSource in sfxAudioSources)
        {
            float savedSfxVolume = PlayerPrefs.GetFloat(SfxVolumePrefKey, sfxAudioSource.volume);
            bool savedSfxMute = PlayerPrefs.GetInt(SfxMutePrefKey, 0) == 1;

            sfxSlider.value = savedSfxVolume;
            sfxAudioSource.volume = savedSfxVolume;

            isSfxMuted = savedSfxMute;
            sfxAudioSource.mute = isSfxMuted;
            UpdateSfxMuteIcon();
        }
    }

    private void LoadMusicFiles()
    {
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
            fileNameText.text = ShortenText(Path.GetFileNameWithoutExtension(filePath), 20);
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
        foreach (var bgmAudioSource in bgmAudioSources)
        {
            if (bgmAudioSource.isPlaying)
            {
                PauseMusic();
            }
            else
            {
                if (bgmAudioSource.clip != null && bgmAudioSource.time > 0)
                {
                    UnPauseMusic();
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
        }
    }

    public void PlaySelectedMusic(string filePath, Button button)
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            selectedFilePath = filePath;

            // 셔플 리스트 초기화
            playedIndices.Clear();

            StartCoroutine(PlayTrack(selectedFilePath));
            buttonImage.sprite = pauseIcon;

            if (currentPlayingButton != null)
            {
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
                foreach (var bgmAudioSource in bgmAudioSources)
                {
                    bgmAudioSource.clip = DownloadHandlerAudioClip.GetContent(www);
                    bgmAudioSource.Play();

                    if (trackEndCoroutine != null)
                    {
                        StopCoroutine(trackEndCoroutine);
                    }
                    trackEndCoroutine = StartCoroutine(OnTrackEnd(bgmAudioSource));
                }
            }
        }
    }

    private IEnumerator OnTrackEnd(AudioSource audioSource)
    {
        while (audioSource.isPlaying || audioSource.time < audioSource.clip.length)
        {
            yield return null;
        }

        if (isRepeating)
        {
            audioSource.time = 0;
            audioSource.Play();
        }
        else
        {
            PlayNextTrack();
        }
    }

    public void PauseMusic()
    {
        foreach (var bgmAudioSource in bgmAudioSources)
        {
            bgmAudioSource.Pause();
        }
        buttonImage.sprite = playIcon;
    }

    public void UnPauseMusic()
    {
        foreach (var bgmAudioSource in bgmAudioSources)
        {
            bgmAudioSource.UnPause();
        }
        buttonImage.sprite = pauseIcon;
    }

    public void PlayNextTrack()
    {
        if (playlist.Count > 0)
        {
            if (isShuffling)
            {
                if (playedIndices.Count == playlist.Count)
                {
                    playedIndices.Clear();
                }

                List<int> availableIndices = new List<int>();
                for (int i = 0; i < playlist.Count; i++)
                {
                    if (!playedIndices.Contains(i))
                    {
                        availableIndices.Add(i);
                    }
                }

                int nextIndex = availableIndices[Random.Range(0, availableIndices.Count)];
                playedIndices.Add(nextIndex);
                currentTrackIndex = nextIndex;
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
        PlayerPrefs.SetInt(ShufflePrefKey, isShuffling ? 1 : 0);
        shuffleButtonImage.sprite = isShuffling ? shuffleIcon : noShuffleIcon;

        // 셔플 상태 변경 시에도 재생된 곡 리스트 초기화
        if (isShuffling)
        {
            playedIndices.Clear();
        }
    }

    public void ToggleRepeat()
    {
        isRepeating = !isRepeating;
        PlayerPrefs.SetInt(RepeatPrefKey, isRepeating ? 1 : 0);
        repeatButtonImage.sprite = isRepeating ? repeatIcon : noRepeatIcon;
    }

    private void SetBgmVolume(float volume)
    {
        foreach (var bgmAudioSource in bgmAudioSources)
        {
            if (bgmAudioSource != null)
            {
                bgmAudioSource.volume = volume;
                PlayerPrefs.SetFloat(BgmVolumePrefKey, volume);
            }
        }
    }

    private void ToggleBgmMute()
    {
        isMuted = !isMuted;
        foreach (var bgmAudioSource in bgmAudioSources)
        {
            if (bgmAudioSource != null)
            {
                bgmAudioSource.mute = isMuted;
                PlayerPrefs.SetInt(BgmMutePrefKey, isMuted ? 1 : 0);
            }
        }
        UpdateBgmMuteIcon();
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
        foreach (var sfxAudioSource in sfxAudioSources)
        {
            if (sfxAudioSource != null)
            {
                sfxAudioSource.volume = volume;
                PlayerPrefs.SetFloat(SfxVolumePrefKey, volume);
            }
        }
    }

    private void ToggleSfxMute()
    {
        isSfxMuted = !isSfxMuted;
        foreach (var sfxAudioSource in sfxAudioSources)
        {
            if (sfxAudioSource != null)
            {
                sfxAudioSource.mute = isSfxMuted;
                PlayerPrefs.SetInt(SfxMutePrefKey, isSfxMuted ? 1 : 0);
            }
        }
        UpdateSfxMuteIcon();
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

    private void OnMusicProgressSliderChanged(float value)
    {
        foreach (var bgmAudioSource in bgmAudioSources)
        {
            if (bgmAudioSource.clip != null)
            {
                bgmAudioSource.time = value * bgmAudioSource.clip.length;
                if (!bgmAudioSource.isPlaying)
                {
                    bgmAudioSource.Play();
                    buttonImage.sprite = pauseIcon;
                }

                if (trackEndCoroutine != null)
                {
                    StopCoroutine(trackEndCoroutine);
                }
                trackEndCoroutine = StartCoroutine(OnTrackEnd(bgmAudioSource));
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerPress == musicProgressSlider.gameObject)
        {
            isSliderDragging = true;
            foreach (var bgmAudioSource in bgmAudioSources)
            {
                bgmAudioSource.Pause();
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerPress == musicProgressSlider.gameObject)
        {
            isSliderDragging = false;
            foreach (var bgmAudioSource in bgmAudioSources)
            {
                bgmAudioSource.time = musicProgressSlider.value * bgmAudioSource.clip.length;
                bgmAudioSource.Play();
                buttonImage.sprite = pauseIcon;

                if (trackEndCoroutine != null)
                {
                    StopCoroutine(trackEndCoroutine);
                }
                trackEndCoroutine = StartCoroutine(OnTrackEnd(bgmAudioSource));
            }
        }
    }
}
