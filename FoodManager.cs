using UnityEngine;
using System.Collections;

public class FoodManager : MonoBehaviour
{
    public GameObject foodPrefab;
    public Transform foodSpawn;

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
    }
}
