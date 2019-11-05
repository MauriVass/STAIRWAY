using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePivot : MonoBehaviour
{
    public static ChangePivot instance;
    [SerializeField]
    SpawnCircle spawner;
    //public Animator anim;
    [SerializeField]
    public GameObject ball1, ball2, active, nonActive, player, playerPivot;
    //Balls are the black dots on the player's sprite
    //active is the pivot around which the player can rotate(it could be one of the 2 balls gameobject)
    //nonActive is the opposite of active(it could be ball1 or ball2). It is used for spawning the pivots in the right place
    //player is the father gameobject of ball1-2,non-active
    //playerPivot is the great father gameobject(all the others(ball1-2,non-active,player) are all children)
    public float initialSpeedRotation,updatedSpeedRotation, speedRotation, accelerationRotation,initialAccelerationRotation;      
    //startSpeedRotation is the speed at which the player start to rotate
    //speedRotation is the instantaneous speed(it changes every frame)
    //accelerationRotation is the rate at which speedRotation increase sR = sR + aR*time
    float maxSpeedRotation; //it is the max speed the player can rotate
    bool isFalling;
    float acceleration = -9.81f;
    public float fallingSpeed;
    public static bool isMoving;
    float averageFrameRate = 45;
    int numTouch;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //spawner = GameObject.Find("CirclesHolder").GetComponent<SpawnPivot>();
        initialSpeedRotation = 3.2f*averageFrameRate;
        speedRotation = updatedSpeedRotation =initialSpeedRotation;
        accelerationRotation =initialAccelerationRotation= 1.2f * averageFrameRate ;
        maxSpeedRotation = initialSpeedRotation * 2f;

        active = ball1;
        nonActive = ball2;
        isMoving = true;
        canChangePivot = true;
        numTouch = 0;
        //anim = GameObject.Find("PlayerSprite").GetComponent<Animator>();
    }
    void Update()
    {
        PlayerMovement();
        if (GamePlayController.gameOver)
            PlayerFall();

        if (Input.GetKeyDown(KeyCode.T))
        {
            text_2.transform.position = new Vector3(10,0,0);
            text_2.GetComponent<Animator>().Play("PointAnim");
            //text_2.transform.GetChild(0).GetComponent<Animator>().SetBool("Set",true);
            //text_2.SetActive(true);
        }

        if (canChangePivot==false)
        {
            StartCoroutine(Wait(0.05f));
        }

        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)&&!GamePlayController.pause)
        {
            if (Input.GetTouch(0).position.y>Camera.main.pixelHeight/8||Input.GetTouch(0).position.x>Camera.main.pixelWidth/4)
            {
                ChangePivotFunction();
            }
        }

        if ((Input.GetMouseButtonDown(0))&& !GamePlayController.pause)
        {
            if (Input.mousePosition.y > Camera.main.pixelHeight / 8 || Input.mousePosition.x > Camera.main.pixelWidth / 4)
            {
                ChangePivotFunction();
            }
        }
    }

    //To fix the problem of double tap
    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        canChangePivot = true;
    }
    public bool canChangePivot;
    void PlayerMovement()
    {
        if (!GamePlayController.gameOver&&isMoving)
        {
            if (Mathf.Abs(speedRotation) < maxSpeedRotation) //Delete Abs for EasterEgg
                speedRotation += accelerationRotation * Time.deltaTime;

            transform.Rotate(0, 0, speedRotation*Time.deltaTime);
        }
    }
    public void ChangePivotFunction()
    {
        if (!GamePlayController.gameOver&&canChangePivot)
        {
            canChangePivot = false;

            CheckCollision(nonActive);
            
            if (active == ball1)
            {
                active = ball2;
                nonActive = ball1;
            }
            else
            {
                active = ball1;
                nonActive = ball2;
            }
            
            player.transform.SetParent(null);
            gameObject.transform.position = active.transform.position;
            player.transform.SetParent(playerPivot.transform);

            float tmp = 1.01f;
            updatedSpeedRotation *= -tmp;
            accelerationRotation *= -tmp;
            speedRotation = updatedSpeedRotation;

            numTouch++;
        }
    }
    public GameObject text_2;
    void CheckCollision(GameObject obj)
    {
        RaycastHit2D hit = Physics2D.Raycast(obj.transform.position, Vector3.forward);
        if (!hit)
        {
            GamePlayController.gameOver = true;
            GamePlayController.instance.UpdateHighScore();

            if (GameController.instance.GetAudio() == 1 && GameController.instance.currentAudioClip != GameController.instance.audioClip[0])
            {
                GameController.instance.ChangeAudio(GameController.instance.audioClip[0]);
            }

            ball1.GetComponent<TrailRenderer>().enabled = false;
            ball2.GetComponent<TrailRenderer>().enabled = false;
        }
        else
        {
            SpawnCircle.canSpawn = true;
            UpdateScore(hit.transform.position);
            GamePlayController.instance.ResetTimer();

            if (numTouch < 1 && GameController.instance.GetAudio() == 1)
            {
                GameController.instance.ChangeAudio(GameController.instance.audioClip[1]);
                GameController.instance.audioSource.volume = 0;
                GameController.instance.ChangeAudio(false);
                //print("Change Audio");
            }

            //print("Distance: "+Vector3.Distance(hit.transform.position,obj.transform.position));
        }
    }
    public void UpdateScore(Vector3 a)
    {
        if (Vector3.Distance(nonActive.transform.position, a) < 0.15f)//0.2f is a value found playing
        {
            GamePlayController.score += 2;
            

            text_2.transform.position = a + new Vector3(.4f, .2f, 0);

            text_2.transform.GetChild(0).GetComponent<Animator>().Play("PointAnim");


        }
        else
        {
            GamePlayController.score++;
        }
        GamePlayController.instance.scoreText.text = GamePlayController.score.ToString();

        //print(Vector3.Distance(nonActive.transform.position, a));
    }
   
    void PlayerFall()
    {
        //if (!SpawnCircle.instance.objNow.GetComponent<CollectCircle>().anim.GetBool("explosion"))
            //SpawnCircle.instance.objNow.GetComponent<CollectCircle>().anim.SetBool("explosion", true);

        Vector3 tmpPos = transform.position;

        if (tmpPos.y < Camera.main.ScreenToWorldPoint(new Vector3(0, -Screen.height, 0)).y * 0.6f)
        {
            if (!GamePlayController.instance.gameOverPanel.activeInHierarchy)
                GamePlayController.instance.ShowGameOverPanel();
            if (SpawnCircle.instance.objNow.GetComponent<CollectCircle>().anim.GetBool("explosion"))
                SpawnCircle.instance.objNow.GetComponent<CollectCircle>().anim.SetBool("explosion", false);
        }
        else
        {
            fallingSpeed += acceleration * Time.deltaTime;
            tmpPos.y += fallingSpeed * Time.deltaTime;
            transform.position = tmpPos; 
        }
    }
    public void RestartPlayer()
    {
        transform.position = SpawnCircle.instance.startingPosition;
        fallingSpeed = 0f;
        speedRotation =updatedSpeedRotation= initialSpeedRotation;
        accelerationRotation = initialAccelerationRotation;
        numTouch = 0;
        canChangePivot = true;

        GameController.instance.ChangeAudio(true);
    }
    
}
