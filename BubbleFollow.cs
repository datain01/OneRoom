using UnityEngine;

public class BubbleFollow : MonoBehaviour
{
    public string targetTag = "CharacterOne"; // 따라갈 대상의 태그
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
        }
        else
        {
            Debug.LogError("태그가 CharacterOne인 오브젝트를 찾을 수 없습니다.");
        }
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.position) + offset;
            rectTransform.position = screenPosition;
        }
    }
}
