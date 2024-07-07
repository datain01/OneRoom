using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject panelBubblePrefab; // PanelBubble 프리팹을 참조
    public DialogueData dialogueData; // DialogueData 스크립터블 오브젝트를 참조
    public GameObject panelSpeaker; // PanelSpeaker 오브젝트를 인스펙터에서 할당
    public GameObject panelOption; // PanelOption 오브젝트를 인스펙터에서 할당
    public Canvas canvas; // UI 캔버스를 참조

    private GameObject character; // CharacterOne 오브젝트를 참조

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("CharacterOne");

        if (character == null)
        {
            Debug.LogError("CharacterOne 태그를 가진 오브젝트를 찾을 수 없습니다.");
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
        Vector3 offset = new Vector3(0.3f, 1.3f, 0f); // UI 오프셋
        Vector3 spawnPosition = character.transform.position + offset;

        GameObject bubble = Instantiate(panelBubblePrefab, spawnPosition, Quaternion.identity, canvas.transform);

        // BubbleFollow 스크립트 설정
        BubbleFollow bubbleFollow = bubble.GetComponent<BubbleFollow>();
        if (bubbleFollow != null)
        {
            bubbleFollow.offset = offset;
            bubbleFollow.targetTag = "CharacterOne"; // 적절한 태그를 설정합니다.
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

        panelOption.transform.SetAsLastSibling();
        
    }
}
