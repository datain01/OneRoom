using UnityEngine;
using UnityEngine.UI;

public class SpeakerImageChange : MonoBehaviour
{
    public Image speakerButtonImage; // UI 버튼의 Image 컴포넌트
    public Sprite speakerOnSprite;   // 음악 재생 중일 때 사용할 이미지
    public Sprite speakerOffSprite;  // 음악 정지 중일 때 사용할 이미지
    public PanelSpeaker panelSpeaker; // PanelSpeaker 스크립트를 참조

    private void Start()
    {
        if (panelSpeaker != null)
        {
            // 초기 이미지 설정
            UpdateSpeakerImage();
        }
    }

    private void Update()
    {
        // 상태 변화가 있을 때마다 이미지를 업데이트
        UpdateSpeakerImage();
    }

    private void UpdateSpeakerImage()
    {
        bool isPlaying = false;

        // 음악 재생 상태 확인
        foreach (var bgmAudioSource in panelSpeaker.bgmAudioSources)
        {
            if (bgmAudioSource.isPlaying)
            {
                isPlaying = true;
                break;
            }
        }

        // 이미지 변경
        if (isPlaying)
        {
            speakerButtonImage.sprite = speakerOnSprite;
        }
        else
        {
            speakerButtonImage.sprite = speakerOffSprite;
        }
    }
}
