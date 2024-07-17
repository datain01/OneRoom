using UnityEngine;

public class WarningManager : MonoBehaviour
{
    public GameObject panelWarningPrefab; // PanelWarning 프리팹 참조
    private GameObject panelWarningInstance;

    // 패널을 표시하는 메서드
    public void ShowWarningPanel()
    {
        // 이미 패널이 활성화된 경우 패널을 새로 만들지 않습니다.
        if (panelWarningInstance != null) return;

        // 캔버스를 동적으로 찾아서 PanelWarning 인스턴스를 캔버스의 자식으로 설정
        Canvas parentCanvas = FindObjectOfType<Canvas>();
        if (parentCanvas == null)
        {
            Debug.LogError("캔버스를 찾을 수 없습니다. 캔버스가 씬에 존재하는지 확인하세요.");
            return;
        }

        panelWarningInstance = Instantiate(panelWarningPrefab, parentCanvas.transform);
    }

}
