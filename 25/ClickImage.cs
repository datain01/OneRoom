using UnityEngine;

public class ClickImage : MonoBehaviour
{
    // 원래 스프라이트와 클릭 시 보여줄 스프라이트를 저장하는 변수
    public Sprite originalSprite;
    public Sprite clickedSprite;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // 이 오브젝트에 붙어 있는 SpriteRenderer 컴포넌트를 가져옴
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 처음에 원래 스프라이트를 설정
        spriteRenderer.sprite = originalSprite;
    }

    void OnMouseDown()
    {
        // 마우스 클릭 시 클릭된 스프라이트로 변경
        spriteRenderer.sprite = clickedSprite;
    }

    void OnMouseUp()
    {
        // 마우스를 떼면 다시 원래 스프라이트로 변경
        spriteRenderer.sprite = originalSprite;
    }
}
