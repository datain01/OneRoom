using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager17 : MonoBehaviour
{
    public GameObject panelBubblePrefab; // PanelBubble 프리팹을 참조
    public DialogueData dialogueData; // DialogueData 스크립터블 오브젝트를 참조
    public GameObject panelSpeaker; // PanelSpeaker 오브젝트를 인스펙터에서 할당
    public GameObject panelOption; // PanelOption 오브젝트를 인스펙터에서 할당
    public Canvas canvas; // UI 캔버스를 참조

    private GameObject character; // CharacterOne 오브젝트를 참조

    public GameObject narration1; // 이미 인스펙터에서 할당된 Narration1 오브젝트
    public GameObject narration2; // 이미 인스펙터에서 할당된 Narration2 오브젝트

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

        if (narration1 == null || narration2 == null)
        {
            Debug.LogError("Narration1 또는 Narration2 오브젝트가 인스펙터에 할당되지 않았습니다.");
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
        Vector3 offset = new Vector3(1.0f, 1.5f, 0f); // UI 오프셋
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

        // PanelBubble을 항상 Narration1과 Narration2 아래로 이동
        if (narration1 != null && narration2 != null)
        {
            int maxSiblingIndex = Mathf.Max(narration1.transform.GetSiblingIndex(), narration2.transform.GetSiblingIndex());
            bubble.transform.SetSiblingIndex(maxSiblingIndex + 1); // Narration1과 Narration2 바로 위로 이동시켜 Bubble이 최상위에 오지 않도록 보장
        }

        // UI 요소들을 최상단으로 이동
        MoveUIElementsToFront();

        // 2초 후에 삭제
        Destroy(bubble, 2f);
    }

    private void MoveUIElementsToFront()
    {
        // Narration1, Narration2, panelSpeaker, panelOption을 UI 계층 구조에서 최상단으로 이동
        if (narration1 != null)
        {
            narration1.transform.SetAsLastSibling();
        }

        if (narration2 != null)
        {
            narration2.transform.SetAsLastSibling();
        }

        panelSpeaker.transform.SetAsLastSibling();
        panelOption.transform.SetAsLastSibling();
    }
}
