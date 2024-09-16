using UnityEngine;

public class LikeSave : MonoBehaviour
{
    public string characterTag; // 캐릭터 태그
    public LikeDisplay likeDisplay; // LikeDisplay 스크립트를 참조

    // private void OnMouseDown()
    // {
    //     // like 값 증가
    //     int currentLikes = PlayerPrefs.GetInt(characterTag + "_like", 0);
    //     currentLikes++;
    //     PlayerPrefs.SetInt(characterTag + "_like", currentLikes);
    //     PlayerPrefs.Save();
    //     Debug.Log(characterTag + " likes: " + currentLikes);

    //     // LikeDisplay 업데이트
    //     if (likeDisplay != null)
    //     {
    //         likeDisplay.UpdateLikeDisplay();
    //     }

    //     // 캐릭터에 LikeCharacter 메서드 호출
    //     GameObject character = GameObject.FindWithTag(characterTag);
    //     if (character != null)
    //     {
    //         ChaController controller = character.GetComponent<ChaController>();
    //         if (controller != null)
    //         {
    //             controller.LikeCharacter();
    //         }
    //     }
    // }
}
