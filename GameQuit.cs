using UnityEngine;
using UnityEngine.UI;

public class GameQuit : MonoBehaviour
{
    public Button buttonQuit; // 인스펙터에서 할당할 버튼 참조

    private void Start()
    {
        // 인스펙터에서 할당된 버튼이 null이 아닌 경우 클릭 이벤트를 설정
        if (buttonQuit != null)
        {
            buttonQuit.onClick.AddListener(QuitGame);
        }
    }

    // 게임 종료 메서드
    public void QuitGame()
    {
        #if UNITY_EDITOR
            // Unity 에디터에서 실행할 때는 종료하지 않고 Play 모드를 해제
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // 빌드된 게임에서는 애플리케이션 종료
            Application.Quit();
        #endif
    }
}
