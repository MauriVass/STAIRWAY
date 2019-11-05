using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCircle : MonoBehaviour
{
    public bool canCollect;

    float acceleration = -9.81f;
    public float speed;
    public Animator anim;
    void Start()
    {
        canCollect = false;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        CollectCircles();
    }
    void CollectCircles()
    {
        if (canCollect)
        {
            if (!anim.GetBool("explosion"))
                anim.SetBool("explosion", true);
            Vector3 tmpPos = transform.position;
            speed += acceleration * Time.deltaTime;
            tmpPos.y += speed * Time.deltaTime;
            transform.position = tmpPos;
            if (tmpPos.y < Camera.main.ScreenToWorldPoint(new Vector3(0, -Screen.height, 0)).y * 0.6f)
            {
                canCollect = false;
                transform.position = new Vector3(-10, 0, 0);
                if (anim.GetBool("explosion"))
                    anim.SetBool("explosion", false);
                SpawnCircle.pivotQueue.Enqueue(gameObject);
            }
        }
    }
}
