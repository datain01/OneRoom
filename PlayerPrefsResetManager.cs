using UnityEngine;

public class PlayerPrefsResetManager : MonoBehaviour
{
    public GameObject panelWarningInstance;
    private LikeDisplay likeDisplay;    // CharacterOne의 LikeDisplay 참조
    private LikeDisplay likeDisplaySec; // CharacterSec의 LikeDisplaySec 참조

    private const string BgmVolumePrefKey = "BgmVolume";
    private const string BgmMutePrefKey = "BgmMute";
    private const string SfxVolumePrefKey = "SfxVolume";
    private const string SfxMutePrefKey = "SfxMute";

    private LikeGift likeGiftOne;       // CharacterOne의 LikeGift 참조
    private LikeGift likeGiftSec;       // CharacterSec의 LikeGift 참조

    private void Start()
    {
        // Like 태그로 LikeDisplay를 찾기
        GameObject likeDisplayObject = GameObject.FindGameObjectWithTag("Like");
        if (likeDisplayObject != null)
        {
            likeDisplay = likeDisplayObject.GetComponent<LikeDisplay>();
        }
        else
        {
            Debug.LogError("Like 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }

        // LikeSec 태그로 LikeDisplaySec를 찾기
        GameObject likeDisplaySecObject = GameObject.FindGameObjectWithTag("LikeSec");
        if (likeDisplaySecObject != null)
        {
            likeDisplaySec = likeDisplaySecObject.GetComponent<LikeDisplay>();
        }
        else
        {
            Debug.LogError("LikeSec 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }

        // LikeGiftOne 태그로 LikeGiftOne을 찾기
        GameObject likeGiftOneObject = GameObject.FindGameObjectWithTag("LikeGiftOne");
        if (likeGiftOneObject != null)
        {
            likeGiftOne = likeGiftOneObject.GetComponent<LikeGift>();
        }
        else
        {
            Debug.LogError("LikeGiftOne 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }

        // LikeGiftSec 태그로 LikeGiftSec을 찾기
        GameObject likeGiftSecObject = GameObject.FindGameObjectWithTag("LikeGiftSec");
        if (likeGiftSecObject != null)
        {
            likeGiftSec = likeGiftSecObject.GetComponent<LikeGift>();
        }
        else
        {
            Debug.LogError("LikeGiftSec 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }
    }

    public void ResetPlayerPrefs()
    {
        // BGM, SFX 볼륨 및 뮤트 상태를 임시 변수에 저장
        float bgmVolume = PlayerPrefs.GetFloat(BgmVolumePrefKey, 1.0f);
        bool bgmMute = PlayerPrefs.GetInt(BgmMutePrefKey, 0) == 1;
        float sfxVolume = PlayerPrefs.GetFloat(SfxVolumePrefKey, 1.0f);
        bool sfxMute = PlayerPrefs.GetInt(SfxMutePrefKey, 0) == 1;

        // PlayerPrefs 초기화
        PlayerPrefs.DeleteAll();

        // 저장된 값을 다시 설정
        PlayerPrefs.SetFloat(BgmVolumePrefKey, bgmVolume);
        PlayerPrefs.SetInt(BgmMutePrefKey, bgmMute ? 1 : 0);
        PlayerPrefs.SetFloat(SfxVolumePrefKey, sfxVolume);
        PlayerPrefs.SetInt(SfxMutePrefKey, sfxMute ? 1 : 0);

        // PlayerPrefs 강제 저장
        PlayerPrefs.Save();

        // LikeDisplay 업데이트
        if (likeDisplay != null)
        {
            likeDisplay.UpdateLikeDisplay();
        }

        if (likeDisplaySec != null)
        {
            likeDisplaySec.UpdateLikeDisplay();
        }

        // milestone 리셋 (LikeGift 스크립트에 있는 리셋 메서드 호출)
        if (likeGiftOne != null)
        {
            likeGiftOne.ResetMilestone();
        }

        if (likeGiftSec != null)
        {
            likeGiftSec.ResetMilestone();
        }

        // panelWarningInstance 파괴
        if (panelWarningInstance != null)
        {
            Destroy(panelWarningInstance);
            panelWarningInstance = null;
        }

        // 초기화 후 다시 값을 로드하거나 UI를 업데이트하는 코드 추가 가능
        Debug.Log("PlayerPrefs 초기화 완료. BGM, SFX 볼륨 및 뮤트 상태는 유지됨.");
    }

    // ButtonNo에 연결할 메서드
    public void CloseWarningPanel()
    {
        if (panelWarningInstance != null)
        {
            Destroy(panelWarningInstance);
        }
    }

    private void Update()
    {
        // ESC 키를 누르면 panelWarningInstance가 활성화된 경우 이를 제거
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelWarningInstance != null)
            {
                Destroy(panelWarningInstance);
                panelWarningInstance = null;
            }
        }
    }
}
