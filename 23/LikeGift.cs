using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LikeGift : MonoBehaviour
{
    public string characterTag; // CharacterOne, CharacterSec
    public GameObject imagePrefab; // UI에 표시할 이미지 Prefab
    public Transform uiParent; // 이미지를 표시할 부모 (Canvas 같은)
    private string imageFolderPath;

    // milestone 저장 (리셋 가능)
    private int lastMilestone = 0;

    private void Start()
    {
        // 경로 설정
        string basePath = Path.Combine(Application.persistentDataPath, "Image");
        imageFolderPath = Path.Combine(basePath, characterTag);

        // 폴더가 없으면 생성
        if (!Directory.Exists(imageFolderPath))
        {
            Directory.CreateDirectory(imageFolderPath);
        }

        // 이전에 표시된 milestone 불러오기
        lastMilestone = PlayerPrefs.GetInt(characterTag + "_milestone", 0);

        // 호감도 변화 체크를 위해 지속적으로 확인
        StartCoroutine(CheckForLikeMilestone());
    }

    // 호감도 달성 시 이미지 표시
    private IEnumerator CheckForLikeMilestone()
    {
        while (true)
        {
            int currentLikes = PlayerPrefs.GetInt(characterTag + "_like", 0);
            int milestone = (currentLikes / 10) * 10;

            // milestone에 도달했으나 아직 이미지가 표시되지 않았을 때만 이미지 표시
            if (milestone > lastMilestone)
            {
                string imagePath = Path.Combine(imageFolderPath, $"{milestone}.png");
                if (File.Exists(imagePath))
                {
                    StartCoroutine(ShowGiftImage(imagePath));
                    lastMilestone = milestone;  // milestone 업데이트
                    PlayerPrefs.SetInt(characterTag + "_milestone", lastMilestone); // milestone 저장
                    PlayerPrefs.Save();  // PlayerPrefs 저장
                }
            }

            yield return new WaitForSeconds(1f);  // 1초 간격으로 체크
        }
    }

    private IEnumerator ShowGiftImage(string imagePath)
    {
        if (File.Exists(imagePath))
        {
            byte[] fileData = File.ReadAllBytes(imagePath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);

            Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            GameObject imageInstance = Instantiate(imagePrefab, uiParent);
            Image imgComponent = imageInstance.GetComponent<Image>();

            if (imgComponent != null)
            {
                imgComponent.sprite = newSprite;
                AspectRatioFitter aspectFitter = imageInstance.GetComponent<AspectRatioFitter>();
                if (aspectFitter == null)
                {
                    aspectFitter = imageInstance.AddComponent<AspectRatioFitter>();
                }
                aspectFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
                aspectFitter.aspectRatio = (float)tex.width / tex.height;

                Time.timeScale = 0;
            }

            yield return new WaitUntil(() => Input.anyKeyDown);

            Time.timeScale = 1;
            Destroy(imageInstance);
        }
    }

    // milestone을 리셋하는 메서드
    public void ResetMilestone()
    {
        lastMilestone = 0;  // milestone 초기화
        PlayerPrefs.SetInt(characterTag + "_milestone", lastMilestone); // milestone 저장
        PlayerPrefs.Save();
        Debug.Log("Milestone reset for " + characterTag);
    }
}
