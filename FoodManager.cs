using UnityEngine;
using System.Collections;

public class FoodManager : MonoBehaviour
{
    public GameObject foodPrefab;
    public Transform foodSpawn;
    public LikeSave likeSave; // LikeSave 스크립트를 참조
    public LikeDisplay likeDisplay; // LikeDisplay 스크립트를 참조
    public ChaController characterController; // ChaController 스크립트를 참조

    private GameObject currentFood;

    void Start()
    {
        SpawnFood();
    }

    public void SpawnFood()
    {
        currentFood = Instantiate(foodPrefab, foodSpawn.position, Quaternion.identity);
    }

    public IEnumerator RespawnFoodAndPlayEatAnimation()
    {
        // Eat 애니메이션 재생
        if (characterController != null)
        {
            characterController.EatCharacter();
        }

        yield return new WaitForSeconds(characterController.eatTime); // 애니메이션이 재생되는 동안 대기

        // Food 재생성
        SpawnFood();

        // Like 값 증가
        IncreaseLike("CharacterOne");
    }

    private void IncreaseLike(string characterTag)
    {
        int currentLikes = PlayerPrefs.GetInt(characterTag + "_like", 0);
        currentLikes += 5; // +5 증가
        PlayerPrefs.SetInt(characterTag + "_like", currentLikes);
        PlayerPrefs.Save();
        Debug.Log(characterTag + " likes: " + currentLikes);

        // LikeDisplay 업데이트
        if (likeDisplay != null && likeDisplay.characterTag == characterTag)
        {
            likeDisplay.UpdateLikeDisplay();
        }
    }
}
