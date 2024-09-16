using UnityEngine;
using TMPro;

public class DialogueManagerSec22 : MonoBehaviour
{
    public GameObject panelBubblePrefab;
    public GameObject panelSpeaker;
    public GameObject panelOption;
    public Canvas canvas;
    private GameObject character;

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("CharacterSec");
        if (character == null || canvas == null || panelSpeaker == null || panelOption == null)
        {
            Debug.LogError("필수 오브젝트가 누락되었습니다.");
            return;
        }
    }

    public void DisplayDialogueFromData()
    {
        string dialogue = GetRandomDialogue();
        DisplayBubble(dialogue);
    }

    public void DisplayDialogueFromTalk(int index)
    {
        string dialogue = GetDialogueFromTalk(index);
        DisplayBubble(dialogue);
    }

    private string GetRandomDialogue()
    {
        int index = SharedDialogueManager.instance.GetRandomIndex(SharedDialogueManager.instance.dialogueData2);
        if (index == -1) return "";
        return SharedDialogueManager.instance.dialogueData2.dialogues[index];
    }

    private string GetDialogueFromTalk(int index)
    {
        if (index < 0 || index >= SharedDialogueManager.instance.dialogueTalk2.dialogues.Count)
        {
            Debug.LogError("잘못된 인덱스입니다.");
            return "";
        }
        return SharedDialogueManager.instance.dialogueTalk2.dialogues[index];
    }

    private void DisplayBubble(string dialogue)
    {
        if (string.IsNullOrEmpty(dialogue)) return;

        Vector3 offset = new Vector3(0.3f, 1.3f, 0f);
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
}
