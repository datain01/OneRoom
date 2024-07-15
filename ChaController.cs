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
    private Animator animator;
    private bool isRight = false; // 기본값: 왼쪽

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetInitialPosition();
        animator.SetBool("isWalking", false);
        animator.SetBool("isRight", isRight); // 기본값을 좌측으로 설정
        StartCoroutine(WaitAndMove()); // 시작 시 대기 상태로 설정
    }

    private void Update()
    {
        if (!isDragged)
        {
            if (isMoving)
            {
                MoveCharacter();
            }
        }
    }

    private void SetInitialPosition()
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

            transform.position = randomPosition;
        }
        else
        {
            transform.position = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), transform.position.z);
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
            animator.SetBool("isWalking", false);
            StartCoroutine(WaitAndMove());
        }
        else
        {
            bool newIsRight = targetPosition.x > transform.position.x;
            if (isRight != newIsRight)
            {
                isRight = newIsRight;
                animator.SetBool("isRight", isRight);
            }
            animator.SetBool("isWalking", true);
        }
    }

    private IEnumerator WaitAndMove()
    {
        yield return new WaitForSeconds(waitTime);
        SetNewTargetPosition();
        isMoving = true;
        animator.SetBool("isWalking", true);
        isRight = targetPosition.x > transform.position.x;
        animator.SetBool("isRight", isRight);
    }

    private void OnMouseDown()
    {
        isDragged = true;
        isMoving = false;
        animator.SetBool("isWalking", false);
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
        animator.SetBool("isWalking", true);
        isRight = targetPosition.x > transform.position.x;
        animator.SetBool("isRight", isRight);
    }

    private bool IsWithinBoundary(Vector3 position)
    {
        return boundaryCollider.OverlapPoint(position);
    }

    public void StopAndRestart()
    {
        StopAllCoroutines(); // 모든 코루틴을 멈추고
        StartCoroutine(RestartAfterWait()); // 대기 후 다시 시작
    }

    private IEnumerator RestartAfterWait()
    {
        yield return new WaitForSeconds(waitTime);
        SetNewTargetPosition();
        isMoving = true;
        animator.SetBool("isWalking", true);
        isRight = targetPosition.x > transform.position.x;
        animator.SetBool("isRight", isRight);
    }
}
