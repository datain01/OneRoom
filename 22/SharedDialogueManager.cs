using System.Collections;
using UnityEngine;

public class SharedDialogueManager : MonoBehaviour
{
    public static SharedDialogueManager instance;

    public DialogueData dialogueData1; // DialogueManager22에서 사용할 DialogueData
    public DialogueData dialogueTalk1; // DialogueManager22에서 사용할 DialogueTalk
    public DialogueData dialogueData2; // DialogueManagerSec22에서 사용할 DialogueData
    public DialogueData dialogueTalk2; // DialogueManagerSec22에서 사용할 DialogueTalk

    public DialogueManager22 dialogueManager; // DialogueManager22 참조
    public DialogueManagerSec22 dialogueManagerSec; // DialogueManagerSec22 참조

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        while (true)
        {
            float waitTime = Random.Range(5f, 15f); // 5~15초 간격으로 대사 출력
            yield return new WaitForSeconds(waitTime);

            bool useDialogueTalk = IsDialogueTalkSelected();
            int index = -1;

            if (useDialogueTalk)
            {
                index = GetRandomIndex(dialogueTalk1); // 동일한 인덱스 사용
                if (index != -1)
                {
                    Debug.Log($"[DialogueTalk] - Index: {index}"); // 로그 출력
                    StartCoroutine(DisplayDialogueAtSameTime(index)); // 두 캐릭터가 동시에 대사 출력
                }
            }
            else
            {
                Debug.Log("[DialogueData]"); // 로그 출력
                StartCoroutine(DisplayDialogueWithDelay()); // 대사 출력에 딜레이 추가
            }
        }
    }

    // 두 캐릭터가 같은 타이밍에 DialogueTalk에서 같은 인덱스의 대사 출력
    private IEnumerator DisplayDialogueAtSameTime(int index)
    {
        dialogueManager.DisplayDialogueFromTalk(index);
        dialogueManagerSec.DisplayDialogueFromTalk(index);
        yield return null; // 즉시 실행되도록 처리
    }

    // DialogueData를 출력할 때 랜덤한 딜레이를 추가
    private IEnumerator DisplayDialogueWithDelay()
    {
        // 두 캐릭터 중 하나에 0~5초 딜레이 추가
        float delay = Random.Range(0f, 5f);
        Debug.Log($"[DialogueData] - Delay: {delay} seconds for DialogueManagerSec22");

        dialogueManager.DisplayDialogueFromData(); // 첫 번째 캐릭터 즉시 출력
        yield return new WaitForSeconds(delay); // 두 번째 캐릭터에 딜레이 추가
        dialogueManagerSec.DisplayDialogueFromData(); // 두 번째 캐릭터 딜레이 후 출력
    }

    public int GetRandomIndex(DialogueData dialogue)
    {
        if (dialogue.dialogues.Count == 0)
        {
            Debug.LogError("대사 리스트가 비어 있습니다.");
            return -1;
        }
        return Random.Range(0, dialogue.dialogues.Count);
    }

    public bool IsDialogueTalkSelected()
    {
        // 50% 확률로 DialogueTalk을 출력할지 결정
        return Random.value < 0.5f;
    }
}
