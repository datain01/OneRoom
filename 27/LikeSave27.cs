using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LikeSave27 : MonoBehaviour
{
    public string characterTag; // 캐릭터 태그
    public LikeDisplay likeDisplay; // LikeDisplay 스크립트를 참조

    public GameObject panelBubblePrefab; // PanelBubble 프리팹을 참조
    public DialogueData dialogueData; // 기본 DialogueData 스크립터블 오브젝트
    public DialogueData dialogueData0, dialogueData50, dialogueData100, dialogueData250, dialogueData400, dialogueData600; // 호감도별 추가 대사 목록
    public GameObject panelSpeaker; // PanelSpeaker 오브젝트를 인스펙터에서 할당
    public GameObject panelOption; // PanelOption 오브젝트를 인스펙터에서 할당
    public Canvas canvas; // UI 캔버스를 참조

    private GameObject character; // Character 오브젝트를 참조
    private List<string> combinedDialogues = new List<string>(); // 현재 호감도에 따른 대사 목록

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag(characterTag);

        if (character == null)
        {
            Debug.LogError(characterTag + " 태그를 가진 오브젝트를 찾을 수 없습니다.");
            return;
        }

        if (canvas == null)
        {
            Debug.LogError("Canvas 오브젝트를 찾을 수 없습니다.");
            return;
        }

        if (panelSpeaker == null)
        {
            Debug.LogError("PanelSpeaker 오브젝트가 인스펙터에 할당되지 않았습니다.");
            return;
        }

        if (panelOption == null)
        {
            Debug.LogError("PanelOption 오브젝트가 인스펙터에 할당되지 않았습니다.");
            return;
        }

        UpdateCombinedDialogues(); // 대사 목록 초기화
    }

    private void OnMouseDown()
    {
        // 호감도 증가 처리
        IncreaseLike();

        // 대사 표시 처리
        DisplayRandomDialogue();
    }

    private void IncreaseLike()
    {
        int currentLikes = PlayerPrefs.GetInt(characterTag + "_like", 0);
        currentLikes++;
        PlayerPrefs.SetInt(characterTag + "_like", currentLikes);
        PlayerPrefs.Save();
        Debug.Log(characterTag + " likes: " + currentLikes);

        // LikeDisplay 업데이트
        if (likeDisplay != null)
        {
            likeDisplay.UpdateLikeDisplay();
        }

        // 캐릭터에 LikeCharacter 메서드 호출
        ChaController controller = character.GetComponent<ChaController>();
        if (controller != null)
        {
            controller.LikeCharacter();
        }

        // 대사 목록 업데이트
        UpdateCombinedDialogues();
    }

    private void DisplayRandomDialogue()
    {
        string randomDialogue = GetRandomDialogue();
        if (!string.IsNullOrEmpty(randomDialogue))
        {
            DisplayBubble(randomDialogue);
        }
    }

    private string GetRandomDialogue()
    {
        if (combinedDialogues.Count == 0)
        {
            Debug.LogError("대사 리스트가 비어 있습니다.");
            return "";
        }
        int randomIndex = Random.Range(0, combinedDialogues.Count);
        return combinedDialogues[randomIndex];
    }

    private void DisplayBubble(string dialogue)
    {
        if (string.IsNullOrEmpty(dialogue)) return;

        // 캐릭터 위에 위치하도록 설정
        Vector3 offset = new Vector3(1.3f, 1.5f, 0f); // UI 오프셋
        Vector3 spawnPosition = character.transform.position + offset;

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
        else
        {
            Debug.LogError("BubbleFollow 스크립트를 찾을 수 없습니다. 프리팹에 BubbleFollow 컴포넌트가 있는지 확인하십시오.");
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

        // PanelSpeaker와 panelOption을 최상단으로 이동
        panelSpeaker.transform.SetAsLastSibling();
        panelOption.transform.SetAsLastSibling();

        Debug.Log("Displayed dialogue: " + dialogue);
    }

    private void UpdateCombinedDialogues()
    {
        combinedDialogues.Clear();
        combinedDialogues.AddRange(dialogueData.dialogues);

        int currentLikes = PlayerPrefs.GetInt(characterTag + "_like", 0);

        if (currentLikes >= 0 && currentLikes < 50)
        {
            combinedDialogues.AddRange(dialogueData0.dialogues);
        }
        else if (currentLikes >= 50 && currentLikes < 100)
        {
            combinedDialogues.AddRange(dialogueData50.dialogues);
        }
        else if (currentLikes >= 100 && currentLikes < 250)
        {
            combinedDialogues.AddRange(dialogueData100.dialogues);
        }
        else if (currentLikes >= 250 && currentLikes < 400)
        {
            combinedDialogues.AddRange(dialogueData250.dialogues);
        }
        else if (currentLikes >= 400 && currentLikes < 600)
        {
            combinedDialogues.AddRange(dialogueData400.dialogues);
        }
        else if (currentLikes >= 600)
        {
            combinedDialogues.AddRange(dialogueData600.dialogues);
        }
    }
}
