using UnityEngine;
using System.Collections;

public class FoodManagerSec : MonoBehaviour
{
    public GameObject foodSecPrefab;
    public Transform foodSpawnSec;

    private GameObject currentFood;

    void Start()
    {
        SpawnFood();
    }

    public void SpawnFood()
    {
        currentFood = Instantiate(foodSecPrefab, foodSpawnSec.position, Quaternion.identity);
    }

    public IEnumerator RespawnFoodAndChangeColor(SpriteRenderer characterRenderer)
    {
        // CharacterOne의 색을 빨간색으로 변경
        characterRenderer.color = Color.blue;

        yield return new WaitForSeconds(1f);

        // CharacterOne의 색을 하얀색으로 되돌림
        characterRenderer.color = Color.magenta;

        // Food 재생성
        SpawnFood();
    }
}
