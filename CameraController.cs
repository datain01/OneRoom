using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dragSpeed = 2.0f;
    public float zoomSpeed = 2.0f;
    public float minZoom = 5.0f;
    public float maxZoom = 20.0f;
    public GameObject panelSpeaker; // PanelSpeaker 오브젝트를 인스펙터에서 할당

    private Vector3 dragOrigin;

    void Update()
    {
        HandleMouseDrag();
        HandleMouseZoom();
    }

    private void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            return;
        }

        if (!Input.GetMouseButton(1)) return;

        Vector3 direction = dragOrigin - Camera.main.ScreenToViewportPoint(Input.mousePosition);

        Camera.main.transform.position += direction * dragSpeed;
    }

    private void HandleMouseZoom()
    {
        if (panelSpeaker != null && panelSpeaker.activeSelf)
        {
            return; // PanelSpeaker가 활성화되어 있으면 확대/축소를 비활성화합니다.
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float size = Camera.main.orthographicSize;
        size -= scroll * zoomSpeed;
        size = Mathf.Clamp(size, minZoom, maxZoom);

        Camera.main.orthographicSize = size;
    }
}
