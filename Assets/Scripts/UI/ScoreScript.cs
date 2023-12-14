using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // ссылка на текстовое поле для отображения счета
    [SerializeField] private TextMeshProUGUI scoreText2;
    [SerializeField] private TextMeshProUGUI bestScoreText; // ссылка на текстовое поле для отображения лучшего результата

    private int scoreNum = 0;
    private int highScore = 0;

    private void Update()
    {
        HighScore();
    }

    public void IncreaseScore()
    {
        scoreNum++;
        scoreText.text = "Score: " + scoreNum;
        scoreText2.text = "Score: " + scoreNum;
    }

    public void HighScore()
    {
        highScore = scoreNum;
        scoreText.text = "Score: " + highScore.ToString();

        if(PlayerPrefs.GetInt("score") <= highScore)
        {
            PlayerPrefs.SetInt("score", highScore);
        }
        bestScoreText.text = "Best Score: " + PlayerPrefs.GetInt("score").ToString();
    }

    

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
