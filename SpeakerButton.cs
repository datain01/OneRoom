using UnityEngine;

public class SpeakerButton : MonoBehaviour
{
    public GameObject panelSpeaker; // 패널 스피커 오브젝트를 프리팹이 아닌 기존 씬에 있는 오브젝트로 참조합니다.

    private void Awake()
    {
        if (panelSpeaker != null)
        {
            panelSpeaker.GetComponent<PanelSpeaker>().InitializeSpeaker(); // 패널 스피커 초기화
            panelSpeaker.SetActive(false); // 게임 시작 시 패널을 비활성화합니다.
        }
    }

    public void OnSpeakerButtonClick()
    {
        if (panelSpeaker != null)
        {
            panelSpeaker.SetActive(!panelSpeaker.activeSelf); // 패널을 활성화/비활성화 토글합니다.
        }
    }
}
