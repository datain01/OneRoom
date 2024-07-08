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
    public Button buttonClose;
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

    private List<string> playlist = new List<string>();
    private List<Button> buttons = new List<Button>();
    private int currentTrackIndex = 0;
    private bool isShuffling = false;
    private bool isRepeating = false;
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
    private bool isSliderDragging = false;
    private Coroutine trackEndCoroutine;

    private void Start()
    {
        buttonPlayPause.onClick.AddListener(TogglePlayPause);
        nextButton.onClick.AddListener(PlayNextTrack);
        prevButton.onClick.AddListener(PlayPreviousTrack);
        shuffleButton.onClick.AddListener(ToggleShuffle);
        repeatButton.onClick.AddListener(ToggleRepeat);
        buttonClose.onClick.AddListener(ClosePanelSpeaker);
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        bgmMuteButton.onClick.AddListener(ToggleBgmMute);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        sfxMuteButton.onClick.AddListener(ToggleSfxMute);
        musicProgressSlider.onValueChanged.AddListener(OnMusicProgressSliderChanged);
    }

    private void Update()
    {
        if (bgmAudioSource.isPlaying && !isSliderDragging)
        {
            musicProgressSlider.value = bgmAudioSource.time / bgmAudioSource.clip.length;
        }
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

        if (!Directory.Exists(musicFolderPath))
        {
            Directory.CreateDirectory(musicFolderPath);
        }

        Debug.Log("Music folder path: " + musicFolderPath);

        LoadMusicFiles();

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

        shuffleButtonImage.sprite = noShuffleIcon;
        repeatButtonImage.sprite = noRepeatIcon;
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

    public void PlaySelectedMusic(string filePath, Button button)
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            selectedFilePath = filePath;
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
                bgmAudioSource.clip = DownloadHandlerAudioClip.GetContent(www);
                bgmAudioSource.Play();

                if (trackEndCoroutine != null)
                {
                    StopCoroutine(trackEndCoroutine);
                }
                trackEndCoroutine = StartCoroutine(OnTrackEnd());
            }
        }
    }

    private IEnumerator OnTrackEnd()
    {
        while (bgmAudioSource.isPlaying || bgmAudioSource.time < bgmAudioSource.clip.length)
        {
            yield return null;
        }

        if (isRepeating)
        {
            bgmAudioSource.time = 0;
            bgmAudioSource.Play();
        }
        else
        {
            PlayNextTrack();
        }
    }

    public void PauseMusic()
    {
        bgmAudioSource.Pause();
        buttonImage.sprite = playIcon;
    }

    public void UnPauseMusic()
    {
        bgmAudioSource.UnPause();
        buttonImage.sprite = pauseIcon;
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
        gameObject.SetActive(false);
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

    private void OnMusicProgressSliderChanged(float value)
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
            trackEndCoroutine = StartCoroutine(OnTrackEnd());
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerPress == musicProgressSlider.gameObject)
        {
            isSliderDragging = true;
            bgmAudioSource.Pause();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerPress == musicProgressSlider.gameObject)
        {
            isSliderDragging = false;
            bgmAudioSource.time = musicProgressSlider.value * bgmAudioSource.clip.length;
            bgmAudioSource.Play();
            buttonImage.sprite = pauseIcon;

            if (trackEndCoroutine != null)
            {
                StopCoroutine(trackEndCoroutine);
            }
            trackEndCoroutine = StartCoroutine(OnTrackEnd());
        }
    }
}
