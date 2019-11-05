using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCircle : MonoBehaviour {

    public static SpawnCircle instance;

    public GameObject pivot;            //This will be the pivots which the player need to touch
    float distance;                     //Player size
    ChangePivot player;                  //Get reference of the "PlayerMovement" script in order to use its variables
    public static Queue pivotQueue;            //The pivots are saved in a Queue for improve performance
    public GameObject objWasted,objNow,objAhead;      //Save in memory the last and second last pivot object
    public static bool canSpawn;
    float time, timeMax = 0.1f;
    //[SerializeField]
    //SpriteRenderer spriteColor;
    public Vector3 startingPosition;
    void Awake()
    {
        instance = this;
    }
    void Start () {

        //Get reference to the player
        canSpawn = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ChangePivot>();
        distance = Vector3.Distance(player.nonActive.transform.position, player.active.transform.position) * 1.15f;//the player width
        pivotQueue = new Queue(GameObject.FindGameObjectsWithTag("Pivot"));
        StartGame();
        startingPosition = player.gameObject.transform.position;
    }


    void Update () {
        if (!GamePlayController.gameOver)
        {
            //It will wait for 0.1 seconds and the call "Spawn" function
            /*if (!canSpawn)
            {
                time += Time.deltaTime;
                if (time>timeMax)
                {
                    canSpawn = true;
                }
            }*/

            if (canSpawn)
            {
                objWasted = objNow;
                objNow = objAhead;
                SpawnCircles();
                CollectCircle cp = objWasted.GetComponent<CollectCircle>();
                cp.canCollect = true;
                cp.speed = 0f;
            }
        }
	}
    //void ChangeColor()
    //{
    //    spriteColor = objAhead.GetComponent<SpriteRenderer>();
    //    float r = Random.Range(0f, 255f), g = Random.Range(0f, 255f), b = Random.Range(0f, 255f);
    //    spriteColor.color = new Color(r, g, b);
    //}

    ///TEMPONARY VALUE:
    ///valueMax is the max angle at which the pivot can spawn with respect to the player position
    ///valueMin is the max angle at which the pivot can spawn with respect to the player position
    ///angle is used to get a Random number between valueMin and valueMax which corrisponde to an angle
    ///tmpx is the x distant of the new pivot from the player position (distance*cosine(angle))
    ///tmpx is the y distant of the new pivot from the player position (distance*sine(angle))    
    float valueMax,valueMin,angle,tmpx,tmpy;

    GameObject tmpObj;
    void SpawnCircles()
    {
        ///Calculate the new Circle position
        valueMax = Mathf.PI / 4f;
        valueMin = -Mathf.PI / 4f;

        if (player.speedRotation < 0)
        {
            angle = Random.Range(valueMin, valueMax);
        }
        else
            angle = Random.Range(-valueMax, -valueMin);

        tmpx = distance * Mathf.Cos(angle);
        tmpy = distance * Mathf.Sin(angle);

        ///Set the new Circle position
        objAhead = (GameObject)pivotQueue.Dequeue();
        objAhead.transform.position = player.active.transform.position + new Vector3(tmpx, tmpy, 0);
        //ChangeColor();

        /*
        ///Make last Circle(lastObj) disappear
        if (lastObj != null)
            lastObj.GetComponent<Rigidbody2D>().isKinematic = false;

        lastObj = preLastObj;
        preLastObj = tmpObj;
        */
        canSpawn = false;
    }

    //It needs to wait for some second to update the player position and spawn a new pivot
    //IEnumerator WaitForSeconds(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    canSpawn = true;
    //    SpawnCircles();
    //}

    void StartGame()
    {
        objNow = (GameObject)pivotQueue.Dequeue();
        objNow.transform.position = player.gameObject.transform.position;
        SpawnCircles();

        //spriteColor = objNow.GetComponent<SpriteRenderer>();
        //ChangeColor();
        canSpawn = false;
        
    }
    //The difference between Start and Restart is that we need use the startPosition
    public void RestartSpawn()
    {
        objWasted= objAhead= null;
        objNow = (GameObject)pivotQueue.Dequeue();
        objNow.transform.position = startingPosition;
        SpawnCircles();
    }
}
