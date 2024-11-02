using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FoodButton27 : MonoBehaviour
{
    public string characterTag; // Inspector에서 설정
    public ChaController characterController; // 캐릭터 컨트롤러 참조
    public LikeDisplay likeDisplay; // LikeDisplay 참조
    public AudioSource audioSource; // 효과음을 재생할 AudioSource
    public int likeIncreaseAmount = 5; // 인스펙터에서 설정할 호감도 증가 값

    public DialogueData dialogueFood; // DialogueFood 데이터 참조
    public GameObject panelBubblePrefab; // PanelBubble 프리팹 참조
    public Canvas canvas; // UI 캔버스 참조

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (characterController != null)
        {
            // Eat 애니메이션 재생
            characterController.EatCharacter();
            StartCoroutine(HandleEatAnimation());

            // 효과음 재생
            PlayClickSound();

            // 대사 출력
            DisplayRandomDialogue();
        }
    }

    private IEnumerator HandleEatAnimation()
    {
        // Like 값 증가
        IncreaseLike(characterTag);
        // 애니메이션 재생 시간 동안 대기
        yield return new WaitForSeconds(characterController.eatTime);
    }

    private void IncreaseLike(string characterTag)
    {
        int currentLikes = PlayerPrefs.GetInt(characterTag + "_like", 0);
        currentLikes += likeIncreaseAmount; // 인스펙터에서 설정한 값만큼 증가
        PlayerPrefs.SetInt(characterTag + "_like", currentLikes);
        PlayerPrefs.Save();
        Debug.Log(characterTag + " likes: " + currentLikes);

        // LikeDisplay 업데이트
        if (likeDisplay != null && likeDisplay.characterTag == characterTag)
        {
            likeDisplay.UpdateLikeDisplay();
        }
    }

    private void PlayClickSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private void DisplayRandomDialogue()
    {
        if (dialogueFood == null || dialogueFood.dialogues.Count == 0)
        {
            Debug.LogError("DialogueFood 데이터가 비어 있거나 참조되지 않았습니다.");
            return;
        }

        // 대사 목록 중 하나를 랜덤으로 선택
        string randomDialogue = GetRandomDialogue();

        // 버블 생성 및 대사 출력
        DisplayBubble(randomDialogue);
    }

    private string GetRandomDialogue()
    {
        int randomIndex = Random.Range(0, dialogueFood.dialogues.Count);
        return dialogueFood.dialogues[randomIndex];
    }

    private void DisplayBubble(string dialogue)
    {
        if (string.IsNullOrEmpty(dialogue)) return;

        // 캐릭터 위에 위치하도록 설정
        Vector3 offset = new Vector3(1.3f, 1.5f, 0f); // UI 오프셋
        Vector3 spawnPosition = characterController.transform.position + offset;

        GameObject bubble = Instantiate(panelBubblePrefab, spawnPosition, Quaternion.identity, canvas.transform);

        if (bubble == null)
        {
            Debug.LogError("PanelBubblePrefab 인스턴스화 실패. 프리팹이 제대로 할당되었는지 확인하십시오.");
            return;
        }

        // BubbleFollow 스크립트 설정
        BubbleFollowSec bubbleFollow = bubble.GetComponent<BubbleFollowSec>();
        if (bubbleFollow != null)
        {
            bubbleFollow.offset = offset;
            bubbleFollow.targetTag = characterTag; // 적절한 태그를 설정합니다.
        }

        TMP_Text dialogueText = bubble.GetComponentInChildren<TMP_Text>();
        if (dialogueText != null)
        {
            dialogueText.text = dialogue;
        }
        else
        {
            Debug.LogError("TMP_Text 컴포넌트를 찾을 수 없습니다. PanelBubblePrefab 내에 TMP_Text가 있는지 확인하십시오.");
        }

        // 2초 후에 삭제
        Destroy(bubble, 2f);
    }
}
