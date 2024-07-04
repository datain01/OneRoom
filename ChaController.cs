using System.Collections;
using UnityEngine;

public class ChaController : MonoBehaviour
{
    public float moveSpeed = 2.0f; // 이동 속도
    public float waitTime = 2.0f;  // 대기 시간
    public PolygonCollider2D boundaryCollider; // 다이아몬드 범위를 나타내는 PolygonCollider2D

    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isDragged = false;

    private void Start()
    {
        SetNewTargetPosition();
    }

    private void Update()
    {
        if (!isDragged)
        {
            if (isMoving)
            {
                MoveCharacter();
            }
            else
            {
                StartCoroutine(WaitAndMove());
            }
        }
    }

    private void SetNewTargetPosition()
    {
        if (boundaryCollider != null)
        {
            Vector3 randomPosition;
            do
            {
                float randomX = Random.Range(boundaryCollider.bounds.min.x, boundaryCollider.bounds.max.x);
                float randomY = Random.Range(boundaryCollider.bounds.min.y, boundaryCollider.bounds.max.y);
                randomPosition = new Vector3(randomX, randomY, transform.position.z);
            } while (!IsWithinBoundary(randomPosition));

            targetPosition = randomPosition;
        }
        else
        {
            targetPosition = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), transform.position.z);
        }
    }

    private void MoveCharacter()
    {
        float step = moveSpeed * Time.deltaTime; // 한 프레임 당 이동할 거리
        Vector3 nextPosition = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (IsWithinBoundary(nextPosition))
        {
            transform.position = nextPosition;
        }

        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            isMoving = false;
        }
    }

    private IEnumerator WaitAndMove()
    {
        yield return new WaitForSeconds(waitTime);
        SetNewTargetPosition();
        isMoving = true;
    }

    private void OnMouseDown()
    {
        isDragged = true;
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPosition = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);

        if (IsWithinBoundary(newPosition))
        {
            transform.position = newPosition;
        }
    }

    private void OnMouseUp()
    {
        isDragged = false;
        SetNewTargetPosition();
        isMoving = true;
    }

    private bool IsWithinBoundary(Vector3 position)
    {
        return boundaryCollider.OverlapPoint(position);
    }
}
