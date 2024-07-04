using UnityEngine;

public class FoodDraggerSec : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        // 드롭된 위치 체크
        Collider2D collider = Physics2D.OverlapPoint(GetMouseWorldPosition());
        if (collider != null && collider.CompareTag("CharacterSec"))
        {
            // Food 오브젝트 파괴
            Destroy(gameObject);

            // Food 재생성 및 색상 변경 요청
            FoodManagerSec foodMaangerSec = FindObjectOfType<FoodManagerSec>();
            foodMaangerSec.StartCoroutine(foodMaangerSec.RespawnFoodAndChangeColor(collider.GetComponent<SpriteRenderer>()));
        }
        else
        {
            // 원래 위치로 복귀
            transform.position = initialPosition;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
