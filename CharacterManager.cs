using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public int playerCount; // 인스펙터에서 입력할 플레이어 수
    public GameObject characterOne; // CharacterOne 오브젝트
    public GameObject characterSec; // CharacterSec 오브젝트

    void Start()
    {
        // 플레이어 수가 1명일 경우 CharacterSec 오브젝트 비활성화
        if (playerCount == 1 && characterSec != null)
        {
            characterSec.SetActive(false);
        }
    }
}
