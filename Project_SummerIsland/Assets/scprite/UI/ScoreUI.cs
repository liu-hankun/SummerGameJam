using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

 
    public TextMeshProUGUI highScoreText;       // 历史最高分

    private int score = 0;
    private int highScore = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // 读取历史最高分
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();
    }

    void OnApplicationQuit()
    {
        // 确保写盘
        PlayerPrefs.Save();
    }

    /// <summary>加分，并自动维护历史最高分与 UI。</summary>
    public void AddScore(int amount)
    {
        score += amount;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        UpdateUI();
    }

    /// <summary>把当前分数重置为 0（开始新局时可调用）。</summary>
    public void ResetScore()
    {
        score = 0;
        UpdateUI();
    }

    /// <summary>清空历史最高分（可绑定到调试按钮）。</summary>
    public void ClearHighScore()
    {
        highScore = 0;
        PlayerPrefs.DeleteKey("HighScore");
        UpdateUI();
    }

    public void UpdateUI()
    {

        if (highScoreText != null)
            highScoreText.text = "High Score: " + highScore;
    }

    public int GetScore() => score;
    public int GetHighScore() => highScore;
}
