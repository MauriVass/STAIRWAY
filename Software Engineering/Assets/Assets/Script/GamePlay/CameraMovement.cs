using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public static CameraMovement instance;
    GameObject player;
    public float followingSpeed,gradientSpeed;
    Vector3 offset;

    Vector3 tmpVector3;
    public static bool restart,forward;
    Vector3 startPosition;
    void Awake()
    {
        instance = this;
    }

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = new Vector3(1,0,-10);
        transform.position = player.transform.position + offset;
        startPosition = transform.position;
        forward = true;

        BackGroundEffect(true);
    }

    void Update()
    {
        if (!GamePlayController.gameOver)
        {


            tmpVector3 = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime * followingSpeed);

            gameObject.GetComponent<Transform>().position = tmpVector3;

            followingSpeed += Time.deltaTime * 0.01f;

            if (Vector3.Distance(transform.position, startPosition) > gradientSpeed + 5)
            {
                BackGroundEffect(true);
                forward = !forward;
            }

            BackGroundEffect(false);
        }


    }

    public void RestartCamera()
    {
        transform.position = player.transform.position + offset;
    }

    public Gradient gradient;
    void BackGroundEffect(bool initialize)
    {
        if (initialize)
        {
            //gradient.SetKeys(colorKey, alphaKey);
            initialize = false;

            /*Color color1, color2;
            color1 = gradient.colorKeys[0].color;
            color2 = gradient.colorKeys[1].color;

            gradient.colorKeys[0].color = color2;
            gradient.colorKeys[1].color = color1;*/

            startPosition = transform.position;
        }
        else
        {
            if (forward)
            {
                gameObject.GetComponent<Camera>().backgroundColor = gradient.Evaluate(Vector3.Distance(transform.position,startPosition)/gradientSpeed);
            }
            else
            {
                gameObject.GetComponent<Camera>().backgroundColor = gradient.Evaluate(1-Vector3.Distance(transform.position, startPosition) / gradientSpeed);
            }
        }
    }
}
