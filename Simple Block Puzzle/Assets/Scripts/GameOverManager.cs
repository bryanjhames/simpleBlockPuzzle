using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;

    public void ShowGameOverScreenDelayed(int finalScore, int highScore, float delay = 2f)
    {
        StartCoroutine(DelayedShow(finalScore, highScore, delay));
    }

    private IEnumerator DelayedShow(int finalScore, int highScore, float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowGameOverScreen(finalScore, highScore);
    }

    public void ShowGameOverScreen(int finalScore, int highScore)
    {
        gameOverPanel.SetActive(true);
        StartCoroutine(CountUpScore(finalScore, highScore));
    }

    private IEnumerator CountUpScore(int finalScore, int highScore)
    {
        int displayedScore = 0;
        int displayedHighScore = 0;
        float duration = 1.5f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            displayedScore = Mathf.FloorToInt(Mathf.Lerp(0, finalScore, t / duration));
            displayedHighScore = Mathf.FloorToInt(Mathf.Lerp(0, highScore, t / duration));

            currentScoreText.text = $"{displayedScore}";
            highScoreText.text = $"{displayedHighScore}";

            yield return null;
        }

        currentScoreText.text = $"{finalScore}";
        highScoreText.text = $"{highScore}";
    }

    public void RestartGame()
    {
        AudioManager.Instance.StopAllSounds();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
