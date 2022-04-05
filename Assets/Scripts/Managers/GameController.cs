using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject checkpoint;
    [SerializeField] private GameObject scoreDisplay; // used to disable timer display
    [SerializeField] private GameObject endDisplay;
	public TMP_Text scoreText;
    public TMP_Text finalScore;
    public TMP_Text highScore;
	private int score;

    void Awake()
    {
        scoreDisplay.SetActive(true);
		score = 0;
        highScore.SetText(PlayerPrefs.GetInt("highScore", 0).ToString());
    }

    public void updateScore()
    {
        score += 50;
        scoreText.SetText(score.ToString());
    }

    public void endGame()
    {
        Time.timeScale = 0f;
        finalScore.SetText(score.ToString());
        if(score > PlayerPrefs.GetInt("highScore"))
        {
            PlayerPrefs.SetInt("highScore", score);
            highScore.SetText(score.ToString());
        }

        scoreDisplay.SetActive(false);
        endDisplay.SetActive(true);
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
}
