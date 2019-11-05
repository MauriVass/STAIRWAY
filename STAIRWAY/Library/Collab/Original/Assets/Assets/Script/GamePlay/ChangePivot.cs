using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePivot : MonoBehaviour
{
    PlayerMovement pm;//Get reference of the "PlayerMovement" script in order to use its variables
    [SerializeField]
    SpawnPivot spawner;

    int score;
    [SerializeField]
    Text scoreText;

    void Start()
    {
        //spawner = GameObject.Find("CirclesHolder").GetComponent<SpawnPivot>();
        pm = GetComponent<PlayerMovement>();

        score = 0;
        scoreText.text = "SCORE: " + score;
    }

    void Update()
    {
        PlayerFall();
    }

    public void ChangePivotFunction()
    {
        if (!GamePlayController.gameOver)
        {
            if (pm.active == pm.ball1)
            {
                pm.active = pm.ball2;
                pm.nonActive = pm.ball1;
            }
            else
            {
                pm.active = pm.ball1;
                pm.nonActive = pm.ball2;
            }
            CheckCollision(pm.active);

            pm.player.transform.SetParent(null);
            pm.gameObject.transform.position = pm.active.transform.position;
            pm.player.transform.SetParent(pm.playerPivot.transform);

            pm.startSpeedRotation *= -1.01f;
            pm.accelerationRotation *= -1.01f;
            pm.speedRotation = pm.startSpeedRotation;
        }
    }
    void CheckCollision(GameObject obj)
    {
        RaycastHit2D hit = Physics2D.Raycast(obj.transform.position, Vector3.forward);
        if (!hit)
        {
            GamePlayController.gameOver = true;

            //Set new HighScore
            int tmpScore = GameController.GetHighScore();
            if (score>tmpScore)
            {
                string tmpName = GameController.instance.GetPlayerName();
                GameController.instance.SetHighScore(score);
                
                GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreBoard>().AddNewHighscore(tmpName,score);
                print("New HighScore");
            }
            print("Score: " + score + "   " + "Highscore: " + tmpScore);
        }
        else
        {
            SpawnPivot.canSpawn = true;
            
            if (Vector3.Distance(pm.nonActive.transform.position, hit.transform.position)<0.2f)//0.2f is a value found playing
            {
                score+=2;

                //Add some animation to make the player understand he got 2 points
            }
            else
            {
                score++;
            }

            scoreText.text = "SCORE: " + score;
        }
    }

    float acceleration = -9.81f;
    public float speed;
    void PlayerFall()
    {
        if (GamePlayController.gameOver)
        {
            Vector3 tmpPos = transform.position;
            speed += acceleration * Time.deltaTime;
            tmpPos.y += speed * Time.deltaTime;
            transform.position = tmpPos;
            if (tmpPos.y < Camera.main.ScreenToWorldPoint(new Vector3(0, -Screen.height, 0)).y * 0.6f)
            {
                GamePlayController.showGameOverPanel = true;
            }
        }
    }
    
}
