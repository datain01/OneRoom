using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManagerSec : MonoBehaviour
{
    public GameObject panelBubblePrefab; // PanelBubble 프리팹을 참조
    public DialogueData dialogueData; // DialogueData 스크립터블 오브젝트를 참조
    public GameObject panelSpeaker; // PanelSpeaker 오브젝트를 인스펙터에서 할당

    private GameObject character; // CharacterSec 오브젝트를 참조
    private Canvas canvas; // UI 캔버스를 참조

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("CharacterSec");
        canvas = GameObject.FindObjectOfType<Canvas>();

        if (character == null)
        {
            Debug.LogError("CharacterSec 태그를 가진 오브젝트를 찾을 수 없습니다.");
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


        StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        while (true)
        {
            float waitTime = Random.Range(5f, 15f); // 5초에서 15초 사이의 랜덤한 간격
            yield return new WaitForSeconds(waitTime);

            string randomDialogue = GetRandomDialogue();
            DisplayBubble(randomDialogue);
        }
    }

    private string GetRandomDialogue()
    {
        if (dialogueData.dialogues.Count == 0)
        {
            Debug.LogError("대사 리스트가 비어 있습니다.");
            return "";
        }
        int randomIndex = Random.Range(0, dialogueData.dialogues.Count);
        return dialogueData.dialogues[randomIndex];
    }

    private void DisplayBubble(string dialogue)
    {
        if (string.IsNullOrEmpty(dialogue)) return;

        // 캐릭터 위에 위치하도록 설정
        Vector3 offset = new Vector3(40f, 120f, 0); // UI 오프셋

        GameObject bubble = Instantiate(panelBubblePrefab, canvas.transform);

        // BubbleFollowSec 스크립트 설정
        BubbleFollowSec bubbleFollow = bubble.GetComponent<BubbleFollowSec>();
        if (bubbleFollow != null)
        {
            bubbleFollow.offset = offset;
        }

        TMP_Text dialogueText = bubble.GetComponentInChildren<TMP_Text>();
        if (dialogueText != null)
        {
            dialogueText.text = dialogue;
        }

        // 2초 후에 삭제
        Destroy(bubble, 2f);
        
        // PanelSpeaker를 최상단으로 이동
        panelSpeaker.transform.SetAsLastSibling();
    }
}
