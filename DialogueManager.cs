using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject panelBubblePrefab; // PanelBubble 프리팹을 참조
    public DialogueData dialogueData; // DialogueData 스크립터블 오브젝트를 참조
    private GameObject character; // CharacterOne 오브젝트를 참조
    private Canvas canvas; // UI 캔버스를 참조

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("CharacterOne");
        canvas = GameObject.FindObjectOfType<Canvas>();

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

        StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // 5초 간격

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

        // BubbleFollow 스크립트 설정
        BubbleFollow bubbleFollow = bubble.GetComponent<BubbleFollow>();
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
    }
}
