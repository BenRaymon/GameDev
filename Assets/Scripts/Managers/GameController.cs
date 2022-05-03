using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject scoreDisplay; // used to disable timer display
    [SerializeField] private GameObject endDisplay;
	public TMP_Text scoreText;
    public TMP_Text finalScore;
    public TMP_Text highScore;
	private int score;
    public bool canUpdateScore = false;

    void Awake()
    {
        scoreDisplay.SetActive(true);
		score = 0;
        highScore.SetText(PlayerPrefs.GetInt("highScore", 0).ToString()); // Uses player perferences to keep track of highscores. Default value set to 0.
    }

    public void updateScore(float xPos)
    {
        score = xPos > score ? (int)xPos : score;
        scoreText.SetText(score.ToString());
    }

    public void toggleScoring()
    {
        canUpdateScore = !canUpdateScore;
    }

    public void endGame()
    {
        Time.timeScale = 0f;
        finalScore.SetText(score.ToString());
        // Checks for new highscore and sets it if true
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
