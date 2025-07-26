using TMPro;
using UnityEngine;

public class UIOrderPanel : MonoBehaviour
{
    public static UIOrderPanel Instance;

    public TextMeshProUGUI orderText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void UpdateDisplay(string currentOrder, int score)
    {
        if (orderText != null)
            orderText.text = $"Order: {currentOrder}";

        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    public void UpdateTimerDisplay(float time)
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(time);
            timerText.text = $"Time: {seconds}s";
        }
    }
}
