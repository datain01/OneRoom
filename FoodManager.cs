using UnityEngine;
using System.Collections;

public class FoodManager : MonoBehaviour
{
    public GameObject foodPrefab;
    public Transform foodSpawn;
    public LikeSave likeSave; // LikeSave 스크립트를 참조
    public LikeDisplay likeDisplay; // LikeDisplay 스크립트를 참조

    private GameObject currentFood;

    void Start()
    {
        SpawnFood();
    }

    public void SpawnFood()
    {
        currentFood = Instantiate(foodPrefab, foodSpawn.position, Quaternion.identity);
    }

    public IEnumerator RespawnFoodAndChangeColor(SpriteRenderer characterRenderer)
    {
        // CharacterOne의 색을 빨간색으로 변경
        characterRenderer.color = Color.red;

        yield return new WaitForSeconds(1f);

        // CharacterOne의 색을 하얀색으로 되돌림
        characterRenderer.color = Color.white;

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
