using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager23 : MonoBehaviour
{
    public GameObject panelBubblePrefab;
    public DialogueData dialogueData;
    public DialogueData dialogueBirthday;
    public DialogueData dialogueSummer;
    public DialogueData dialogueWinter;
    public GameObject panelSpeaker;
    public GameObject panelOption;
    public Canvas canvas;

    private GameObject character;
    private List<string> currentDialogues = new List<string>();

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("CharacterOne");
        if (character == null)
        {
            Debug.LogError("CharacterOne 태그를 가진 오브젝트를 찾을 수 없습니다.");
            return;
        }

        if (canvas == null || panelSpeaker == null || panelOption == null)
        {
            Debug.LogError("필수 UI 컴포넌트가 할당되지 않았습니다.");
            return;
        }

        UpdateDialogueList();
        StartCoroutine(DisplayDialogue());
    }

    private void UpdateDialogueList()
    {
        currentDialogues.Clear();
        currentDialogues.AddRange(dialogueData.dialogues); // 기본 대사 리스트 추가

        DateTime now = DateTime.Now;
        int month = now.Month;
        int day = now.Day;

        // 생일 대사 리스트 추가
        if ((month == 12 && day == 17) || (month == 5 && day == 31))
        {
            currentDialogues.AddRange(dialogueBirthday.dialogues);
        }

        // 계절 대사 리스트 추가
        if (month >= 6 && month <= 8) // 여름
        {
            currentDialogues.AddRange(dialogueSummer.dialogues);
        }
        else if (month == 12 || month == 1 || month == 2) // 겨울
        {
            currentDialogues.AddRange(dialogueWinter.dialogues);
        }
    }

    private IEnumerator DisplayDialogue()
{
        while (true)
        {
            float waitTime = UnityEngine.Random.Range(50f, 60f);
            yield return new WaitForSeconds(waitTime);

            string randomDialogue = GetRandomDialogue();
            DisplayBubble(randomDialogue);
        }
    }

    private string GetRandomDialogue()
    {
        if (currentDialogues.Count == 0)
        {
            return "No dialogues available.";
        }

        int randomIndex = UnityEngine.Random.Range(0, currentDialogues.Count); 
        return currentDialogues[randomIndex];
    }



    private void DisplayBubble(string dialogue)
    {
        Vector3 offset = new Vector3(1.5f, 1.8f, 0f);
        Vector3 spawnPosition = character.transform.position + offset;

        GameObject bubble = Instantiate(panelBubblePrefab, spawnPosition, Quaternion.identity, canvas.transform);
        TMP_Text dialogueText = bubble.GetComponentInChildren<TMP_Text>();
        if (dialogueText != null)
        {
            dialogueText.text = dialogue;
        }

        Destroy(bubble, 3f);
        panelSpeaker.transform.SetAsLastSibling();
        panelOption.transform.SetAsLastSibling();
    }

    public void DisplayRandomDialogue()
    {
        string dialogue = GetRandomDialogue();
        DisplayBubble(dialogue);
    }
}
