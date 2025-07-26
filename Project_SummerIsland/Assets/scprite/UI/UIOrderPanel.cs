using UnityEngine;
using TMPro;

public class UIOrderPanel : MonoBehaviour
{
    public static UIOrderPanel Instance;

    public TextMeshProUGUI orderText;
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateDisplay(string currentOrder, int score)
    {
        if (orderText != null)
            orderText.text = $"任务：{currentOrder}";

        if (scoreText != null)
            scoreText.text = $"分数：{score}";
    }
}
