using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreTextInGame;     // ?????
    public TextMeshProUGUI scoreTextFinal;      // ??????
    public TextMeshProUGUI highScoreText;       // ???

    private int score = 0;
    private int highScore = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();
    }

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

    public void UpdateUI()
    {
        if (scoreTextInGame != null)
            scoreTextInGame.text = "Score: " + score;

        if (scoreTextFinal != null)
            scoreTextFinal.text = "Score: " + score;

        if (highScoreText != null)
            highScoreText.text = "High Score: " + highScore;
    }

    public int GetScore() => score;
    public int GetHighScore() => highScore;
}
