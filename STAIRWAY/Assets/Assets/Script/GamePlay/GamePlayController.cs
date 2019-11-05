using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GamePlayController : MonoBehaviour {
    public static GamePlayController instance;
    public static bool gameOver,pause;
    public static int score, highScore;
    [SerializeField]
    public Text scoreText,finalScoreText;
    [SerializeField]
    Text timeLeftText;
    [SerializeField]
    public GameObject gameOverPanel,pausePanel,panel;
    GameObject fadeImage;
    float timeA, timeB,deltaTime,timeLeft;

    GameObject[] textObjects;
    
    void MakeInstance()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        MakeInstance();
        
        GameController.instance.ChangeLanguage(null);
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        ResetTimer();
        gameOver = false;
        score = 0;
        scoreText.text = score.ToString();
        pause = false;

        fadeImage= panel.transform.GetChild(0).gameObject;

        //textObjects = GameObject.FindGameObjectsWithTag("Text");
        //GameController.instance.ChangeLanguage(textObjects);

        GameController.instance.ChangeAudio(true);
    }
    void Update()
    {
        if (!gameOver&&ChangePivot.isMoving)
        {
            ShowTimer();
        }
        
       
    }
    public void ShowGameOverPanel()
    {  
            gameOverPanel.SetActive(true);
            finalScoreText.text = GameController.GetHighScore().ToString();
            SpawnCircle.instance.objNow.transform.position = new Vector3(-10, 0, 0);
            SpawnCircle.instance.objAhead.transform.position = new Vector3(-10, 0, 0);
            SpawnCircle.pivotQueue.Enqueue(SpawnCircle.instance.objNow);
            SpawnCircle.pivotQueue.Enqueue(SpawnCircle.instance.objAhead);
    }
    public void ShowPausePanel()
    {
        if (gameOver != true)
        {
            pausePanel.SetActive(true);
            ChangePivot.isMoving = false;
            Time.timeScale = 0f;
            pause = true;
        }
    }
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        ChangePivot.isMoving = true;
        Time.timeScale = 1f;
        pause = false;
    }

    public IEnumerator FadeScene(string sceneName)
    {
        panel.SetActive(true);
        yield return StartCoroutine(MyCoroutine.MyWaitForSeconds(1));

        SceneManager.LoadScene(sceneName);

        fadeImage.GetComponent<Animator>().SetTrigger("StartAnimation");
        yield return StartCoroutine(MyCoroutine.MyWaitForSeconds(1));
        panel.SetActive(false);
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        
        if (GameController.instance.currentAudioClip!=GameController.instance.audioClip[0])
        {
            GameController.instance.ChangeAudio(GameController.instance.audioClip[0]);
        }
        StartCoroutine(FadeScene("MainMenuScene"));
    }

    public void UpdateHighScore()//Set new HighScore
    {
        int tmpScore = GameController.GetHighScore();
        if (score > tmpScore)
        {
            string tmpName = GameController.instance.GetPlayerName();
            GameController.instance.SetHighScore(score);

            GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreBoard>().AddNewHighscore(tmpName, score);
            //print("New HighScore");
        }
        //print("Score: " + score + "   " + "Highscore: " + tmpScore);
    }
    public void  Restart()
    {
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        ChangePivot.instance.RestartPlayer();
        SpawnCircle.instance.RestartSpawn();
        CameraMovement.instance.RestartCamera();
        gameOver = false;
        pause = false;
        ResetTimer();
        score = 0;
        scoreText.text = score.ToString();

        ChangePivot.instance.ball1.GetComponent<TrailRenderer>().enabled = true;
        ChangePivot.instance.ball2.GetComponent<TrailRenderer>().enabled = true;

    }
    public void ResetTimer()
    {
        timeA = Time.time;
    }
    void ShowTimer()
    {
        timeB = Time.time;
        deltaTime = timeB - timeA;
        timeLeft = 6f - deltaTime;
        timeLeft *= 100f;
        timeLeft=((int)timeLeft) /100f;
        timeLeftText.text = timeLeft.ToString();
        if (timeLeft < 0.01f)
        {
            gameOver = true;
            UpdateHighScore();

            ChangePivot.instance.ball1.GetComponent<TrailRenderer>().enabled = false;
            ChangePivot.instance.ball2.GetComponent<TrailRenderer>().enabled = false;

            //GameController.instance.ChangeAudio(false);
            if (GameController.instance.currentAudioClip==GameController.instance.audioClip[1])
            {
                GameController.instance.ChangeAudio(GameController.instance.audioClip[0]);
            }
            timeLeft = 0;
        }
    }
}
