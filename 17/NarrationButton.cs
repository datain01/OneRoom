using UnityEngine;
using System.Collections;

public class NarrationButton : MonoBehaviour
{
    // 두 가지 프리팹을 할당할 변수
    public GameObject narrationPrefab1;
    public GameObject narrationPrefab2;

    // 패널 UI가 포함될 부모 캔버스
    public Transform canvasTransform;

    // 현재 활성화된 인스턴스를 추적하기 위한 변수
    private GameObject currentInstance;

    // PanelOption 오브젝트 참조
    public GameObject panelOption;

    private void Update()
    {
        // PanelOption이 활성화되면 현재 인스턴스 파괴
        if (panelOption != null && panelOption.activeSelf)
        {
            DestroyCurrentInstance();
        }
    }

    // ButtonPic을 클릭했을 때 실행될 함수
    public void OnButtonClick()
    {
        // 이미 활성화된 인스턴스가 있으면 파괴
        DestroyCurrentInstance();

        // 프리팹을 랜덤하게 선택
        GameObject selectedPrefab = Random.Range(0, 2) == 0 ? narrationPrefab1 : narrationPrefab2;

        // 선택된 프리팹을 캔버스의 자식으로 인스턴스화하고, 인스턴스화된 오브젝트를 변수에 저장
        currentInstance = Instantiate(selectedPrefab, canvasTransform);

        // 3초 후에 인스턴스화된 오브젝트를 파괴하는 코루틴 실행
        StartCoroutine(DestroyAfterDelay(currentInstance, 3.0f));
    }

    // 현재 인스턴스 파괴 메서드
    private void DestroyCurrentInstance()
    {
        if (currentInstance != null)
        {
            Destroy(currentInstance);
            currentInstance = null; // 현재 인스턴스 변수 초기화
        }
    }

    // 인스턴스화된 오브젝트를 3초 후에 파괴하는 코루틴
    private IEnumerator DestroyAfterDelay(GameObject instance, float delay)
    {
        // 지정된 시간만큼 대기
        yield return new WaitForSeconds(delay);

        // 인스턴스가 여전히 유효하면 파괴
        if (instance != null)
        {
            Destroy(instance);

            // currentInstance를 null로 설정하여 다음에 새로운 인스턴스를 생성할 수 있도록 함
            if (currentInstance == instance)
            {
                currentInstance = null;
            }
        }
    }
}
