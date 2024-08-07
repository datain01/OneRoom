using UnityEngine;

public class BubbleFollowSec : MonoBehaviour
{
    public string targetTag = "CharacterSec"; // 따라갈 대상의 태그
    public Vector3 offset; // 위치 오프셋

    private Transform target; // 따라갈 대상의 Transform
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // 태그로 대상 찾기
        GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObject != null)
        {
            target = targetObject.transform;
            UpdatePosition(); // 초기 위치 설정
        }
        else
        {
            Debug.LogError("태그가 " + targetTag + "인 오브젝트를 찾을 수 없습니다.");
        }
    }

    private void Update()
    {
        if (target != null)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        Vector3 worldPosition = target.position + offset;
        rectTransform.position = worldPosition;
    }
}
