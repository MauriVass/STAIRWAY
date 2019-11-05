using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPivot : MonoBehaviour {

    public GameObject pivot;            //This will be the pivots which the player need to touch
    float distance;                     //Player size
    PlayerMovement pm;                  //Get reference of the "PlayerMovement" script in order to use its variables
    public static Queue pivotQueue;            //The pivots are saved in a Queue for improve performance
    public GameObject objWasted,objNow,objAhead;      //Save in memory the last and second last pivot object
    public static bool canSpawn;
    float time, timeMax = 0.1f;
    public static bool restart;
    [SerializeField]
    SpriteRenderer spriteColor;
    Vector3 startingPosition;

    void Start () {
        
        //Get reference to the player
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        distance = Vector3.Distance(pm.nonActive.transform.position, pm.active.transform.position) * 1.15f;//the player width
        pivotQueue = new Queue(GameObject.FindGameObjectsWithTag("Pivot"));
        StartGame();
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
                Spawn();
                objWasted.transform.GetComponent<CollectPivot>().canDelete = true;
                objWasted.transform.GetComponent<CollectPivot>().speed = 0f;
                objWasted.transform.GetComponent<CollectPivot>().anim.SetBool("explosion", true);
                print("spawned");
            }
        }
        else
        {
            if (restart)
            {
                StartGame();
                restart = false;
            }
        }
	}
    void ChangeColor()
    {
        float r = Random.Range(0f, 255f), g = Random.Range(0f, 255f), b = Random.Range(0f, 255f);
        spriteColor.color = new Color(r, g, b);
    }

    ///TEMPONARY VALUE:
    ///valueMax is the max angle at which the pivot can spawn with respect to the player position
    ///valueMin is the max angle at which the pivot can spawn with respect to the player position
    ///angle is used to get a Random number between valueMin and valueMax which corrisponde to an angle
    ///tmpx is the x distant of the new pivot from the player position (distance*cosine(angle))
    ///tmpx is the y distant of the new pivot from the player position (distance*sine(angle))    
    float valueMax,valueMin,angle,tmpx,tmpy;

    GameObject tmpObj;
    void Spawn()
    {
        ///Calculate the new Circle position
        valueMax = Mathf.PI / 4f;
        valueMin = -Mathf.PI / 4f;

        if (pm.speedRotation < 0)
        {
            angle = Random.Range(valueMin, valueMax);
        }
        else
            angle = Random.Range(-valueMax, -valueMin);

        tmpx = distance * Mathf.Cos(angle);
        tmpy = distance * Mathf.Sin(angle);

        ///Set the new Circle position
        objAhead = (GameObject)pivotQueue.Dequeue();
        objAhead.transform.position = pm.active.transform.position + new Vector3(tmpx, tmpy, 0);
        spriteColor = objAhead.GetComponent<SpriteRenderer>();
        ChangeColor();

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
    IEnumerator WaitForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        canSpawn = true;
        Spawn();
    }

    void StartGame()
    {
        objNow = (GameObject)pivotQueue.Dequeue();

        Spawn();

        objNow.transform.position = pm.gameObject.transform.position;
        spriteColor = objNow.GetComponent<SpriteRenderer>();
        ChangeColor();
        canSpawn = false;
        startingPosition = pm.gameObject.transform.position;
    }

}
