using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleFalls : MonoBehaviour
{
    public Animator anim;
    float acceleration = -5f;
    public float speed;
    float randomX, randomY,maxY,explodeY;
    bool isCollected = false;
    void Start()
    {
        maxY = 27f;
        anim = GetComponent<Animator>();
        randomX = Random.Range(-2.6f, 2.6f);
        randomY = Random.Range(6.47f, maxY);
        explodeY = Random.Range(1f, 5f);
        transform.position = new Vector3(randomX, randomY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (MenùAnimation.activeMenu == 1)
        {
            if (isCollected == true)
                isCollected = false;
            CircleFalling();
        }
        else
        {if (isCollected == false)
                CircleCollecting();
        }
    }
    void CircleFalling()
    {
        if (transform.position.y < explodeY && !anim.GetBool("explosion"))
            anim.SetBool("explosion", true);    
        Vector3 tmpPos = transform.position;
        speed += acceleration * Time.deltaTime;
        tmpPos.y += speed * Time.deltaTime;
        transform.position = tmpPos;
        if (tmpPos.y < -5f)
        {
            speed = 0;
            //float randomX = Random.Range(Camera.main.ScreenToWorldPoint(new Vector3(0, -0.5f * Screen.height, 0)).x, Camera.main.ScreenToWorldPoint(new Vector3(0, 0.5f * Screen.height, 0)).x);
            randomX = Random.Range(-2.6f, 2.6f);
            randomY = Random.Range(6.47f, maxY);
            explodeY = Random.Range(1f, 5f);
            transform.position = new Vector3(randomX, randomY, 0);
            if (anim.GetBool("explosion"))
                anim.SetBool("explosion", false);

        }
    }
    void CircleCollecting()
    {
        randomX = Random.Range(-2.6f, 2.6f);
        randomY = Random.Range(6.47f, maxY);
        transform.position = new Vector3(randomX, randomY, 0);
        isCollected = true;
    }
}