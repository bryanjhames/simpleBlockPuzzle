using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance;

    public int score;
    public int highScore;

    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    public Image scoreProgressBar;


    private void Awake()
    {
        Instance = this;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();
    }

    public void AddPoints(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void ApplyMultiplier(int linesCleared)
    {
        int bonus = linesCleared * linesCleared * 10;
        score += bonus;
        UpdateUI();
    }

    public void CheckHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
        UpdateUI();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateUI();
    }

    public void UpdateUI()
    {
        scoreText.text = $"{score}";
        highScoreText.text = $"{highScore}";

        // Progress Bar logic
        float progress = 0f;
        if (highScore > 0)
            progress = Mathf.Clamp01((float)score / highScore);

        scoreProgressBar.fillAmount = progress;
    }

    public int GetCurrentScore()
    {
        return score;
    }

    public int GetHighScore()
    {
        return highScore;
    }
}
