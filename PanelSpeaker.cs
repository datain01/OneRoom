using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.Networking;

public class PanelSpeaker : MonoBehaviour
{
    public GameObject playlistPanel;
    public GameObject musicItemPrefab;
    public Button buttonPlayPause;
    public Button nextButton;
    public Button prevButton;
    public Button shuffleButton;
    public AudioSource audioSource;
    public Image buttonImage; // 버튼 아이콘 이미지
    public Sprite playIcon; // 재생 아이콘
    public Sprite pauseIcon; // 일시정지 아이콘

    private List<string> playlist = new List<string>();
    private int currentTrackIndex = 0;
    private bool isShuffling = false;
    public string musicFolderPath { get; private set; }

    private void Start()
    {
        musicFolderPath = Path.Combine(Application.persistentDataPath, "Music");

        // 음악 폴더가 없으면 생성
        if (!Directory.Exists(musicFolderPath))
        {
            Directory.CreateDirectory(musicFolderPath);
        }

        Debug.Log("Music folder path: " + musicFolderPath); // 경로 확인을 위한 디버그 로그

        LoadMusicFiles();

        buttonPlayPause.onClick.AddListener(TogglePlayPause);
        nextButton.onClick.AddListener(PlayNextTrack);
        prevButton.onClick.AddListener(PlayPreviousTrack);
        shuffleButton.onClick.AddListener(ToggleShuffle);
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
        
        // MusicItem의 RectTransform을 Playlist Panel의 RectTransform에 맞추기
        RectTransform newItemRect = newItem.GetComponent<RectTransform>();
        RectTransform playlistRect = playlistPanel.GetComponent<RectTransform>();
        newItemRect.sizeDelta = new Vector2(playlistRect.rect.width, newItemRect.sizeDelta.y);

        TextMeshProUGUI fileNameText = newItem.GetComponentInChildren<TextMeshProUGUI>();
        if (fileNameText != null)
        {
            fileNameText.text = Path.GetFileName(filePath);
        }
    }

    public void TogglePlayPause()
    {
        if (audioSource.isPlaying)
        {
            PauseMusic();
        }
        else
        {
            PlayMusic();
        }
    }

    public void PlayMusic()
    {
        if (playlist.Count > 0)
        {
            StartCoroutine(PlayTrack(playlist[currentTrackIndex]));
            buttonImage.sprite = pauseIcon;
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
            currentTrackIndex = (currentTrackIndex + 1) % playlist.Count;
            PlayMusic();
        }
    }

    public void PlayPreviousTrack()
    {
        if (playlist.Count > 0)
        {
            currentTrackIndex = (currentTrackIndex - 1 + playlist.Count) % playlist.Count;
            PlayMusic();
        }
    }

    public void ToggleShuffle()
    {
        isShuffling = !isShuffling;
    }
}
