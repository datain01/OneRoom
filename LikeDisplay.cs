using UnityEngine;
using TMPro;

public class LikeDisplay : MonoBehaviour
{
    public string characterTag; // 캐릭터 태그
    public TextMeshProUGUI likeText; // TextMeshPro UI 요소

    private void Start()
    {
        UpdateLikeDisplay();
    }

    public void UpdateLikeDisplay()
    {
        int currentLikes = PlayerPrefs.GetInt(characterTag + "_like", 0);
        likeText.text = currentLikes;
    }
}
