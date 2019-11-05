using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenùAnimation : MonoBehaviour {
    public bool right,left;//they are used to check if the menu must rotate to left or right

    //Some variable used to control the rotation of the "pivots"
    float angle = 120f, speed=0f,acceleration=2.2f,initialSpeed;
    public float missing, totalAngle;

    //The pivotPanel is the gameobject used to make the buttons rotate around it //buttonVisible is used to start the pivotPanel's animation
    public GameObject pivotPanel,buttonVisible;
    public GameObject[] pivotButtonChilden;
    
    public GameObject highScore;

    //It is used to solve the problem that the player can change the menu when the pivotPanel is at the right-bottom of the screen.
    public  bool canRotate, canChangeScene;

	// Use this for initialization
	void Start () {
        initialSpeed = speed;
        canRotate = true;
        canChangeScene = true;
        not_panelAnimClosing = true;
        activeMenu = 1;
        Vector3 tmpPos = new Vector3(0,-Screen.height*3/2f,0);
        //transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        transform.GetComponent<RectTransform>().anchoredPosition = tmpPos;

        realButton.SetActive(false);
    }

    //Vector3 posEndAnimButton = new Vector3(194,-477,0);
    Vector3 posStartAnimButton = new Vector3(0,0,0);
    void Update()
    {
        Rotate();
        
        if (Vector3.Distance( pivotPanel.transform.parent.transform.position, Camera.main.ScreenToWorldPoint(posStartAnimButton)) <0.1f)//World from screen position //Bottom right position
        {
            buttonVisible.SetActive(true);

            canRotate = false;
        }
    }

    float deltaAngle = 5f;
    void Rotate()
    {
        if (right)
        {
            speed += Time.deltaTime * acceleration;
            missing += speed;
            transform.Rotate(0, 0, speed);

            pivotPanel.transform.Rotate(0, 0, speed);
            for (int i = 0; i < pivotButtonChilden.Length; i++)
            {
                pivotButtonChilden[i].transform.Rotate(0, 0, -speed);
            }


            if (missing > angle )
            {
                right = false;
                totalAngle += angle;
                //This gameobject
                transform.rotation = Quaternion.Euler(0, 0, totalAngle);

                missing = 0;
                speed = initialSpeed;
                
                pivotPanel.transform.rotation = Quaternion.Euler(0, 0, totalAngle);

                if ( (pivotButtonChilden[0].transform.rotation.z - 120f) < deltaAngle)
                {
                    for (int i = 0; i < pivotButtonChilden.Length; i++)
                    {
                        pivotButtonChilden[i].transform.rotation = Quaternion.Euler(0, 0, -120);
                    }
                }
                if ((pivotButtonChilden[0].transform.rotation.z + 120f) < deltaAngle)
                {
                    for (int i = 0; i < pivotButtonChilden.Length; i++)
                    {
                        pivotButtonChilden[i].transform.rotation = Quaternion.Euler(0, 0, +120);
                    }
                }
                if ((pivotButtonChilden[0].transform.rotation.z) < deltaAngle)
                {
                    for (int i = 0; i < pivotButtonChilden.Length; i++)
                    {
                        pivotButtonChilden[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                }

                //pivotPanel.transform.parent.GetComponent<Animator>().SetTrigger("Close");
               pivotPanel.transform.parent.GetComponent<Animator>().SetBool("Close1",true);
                pivotPanel.transform.parent.GetComponent<Animator>().SetBool("Open1", false);
                buttonVisible.SetActive(true);
                
                if (activeMenu==3)
                {
                    realButton.SetActive(true);
                }

                canChangeScene = true;
                not_panelAnimClosing = true;

            }
        }
        if (left)
        {
            speed -= Time.deltaTime * acceleration;
            missing += -speed;
            transform.Rotate(0, 0, speed);
            pivotPanel.transform.Rotate(0, 0, speed);
            for (int i = 0; i < pivotButtonChilden.Length; i++)
            {
                pivotButtonChilden[i].transform.Rotate(0, 0, -speed);
            }

            if (missing > angle)
            {
                left = false;
                totalAngle -= angle;
                transform.rotation = Quaternion.Euler(0, 0, totalAngle);

                pivotPanel.transform.rotation = Quaternion.Euler(0, 0, totalAngle);

                missing = 0;
                speed = initialSpeed;

                if ((pivotButtonChilden[0].transform.rotation.z - 120f) < deltaAngle)
                {
                    for (int i = 0; i < pivotButtonChilden.Length; i++)
                    {
                        pivotButtonChilden[i].transform.rotation = Quaternion.Euler(0, 0, -120);
                    }
                }
                if ((pivotButtonChilden[0].transform.rotation.z + 120f) < deltaAngle)
                {
                    for (int i = 0; i < pivotButtonChilden.Length; i++)
                    {
                        pivotButtonChilden[i].transform.rotation = Quaternion.Euler(0, 0, +120);
                    }
                }
                if ((pivotButtonChilden[0].transform.rotation.z) < deltaAngle)
                {
                    for (int i = 0; i < pivotButtonChilden.Length; i++)
                    {
                        pivotButtonChilden[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                }

                //pivotPanel.transform.parent.GetComponent<Animator>().SetTrigger("Close");
                pivotPanel.transform.parent.GetComponent<Animator>().SetBool("Close1", true);
                pivotPanel.transform.parent.GetComponent<Animator>().SetBool("Open1", false);
                buttonVisible.SetActive(true);
                
                if (activeMenu == 3)
                {
                    realButton.SetActive(true);
                }

                canChangeScene = true;
                not_panelAnimClosing = true;
            }
        }
    }
    public GameObject realButton;
    //3 menus: Main-->1, HighScore-->3, Options-->2
    int menus = 3;
    public static int activeMenu;
    public void ChangeScene(string isRight)
    {
        if (canRotate&&canChangeScene)
        {
            canChangeScene = false;

            if (isRight == "Right")
            {
                right = true;

                activeMenu++;
            }
            else
            {
                left = true;

                activeMenu--;
                
            }

            if (activeMenu > menus)
            {
                activeMenu = 1;
            }
            else if (activeMenu < 1)
            {
                activeMenu = menus;
            }
            
            switch (activeMenu)
            {
                case 1:
                    //print("Active Menu: Main");
                    realButton.SetActive(false);
                    break;
                case 2:
                    //print("Active Menu: Options");
                    realButton.SetActive(false);
                    break;
                case 3:
                    //print("Active Menu: HighScore");
                    //realButton.SetActive(true);
                    break;
            }

        }
    }

    bool not_panelAnimClosing;
    public void SetButtonPivotPanel()
    {
        //if (not_panelAnimClosing)
        {
            not_panelAnimClosing = false;
            buttonVisible.SetActive(!buttonVisible.activeInHierarchy);
            //pivotPanel.transform.parent.GetComponent<Animator>().SetTrigger("Open");
            pivotPanel.transform.parent.GetComponent<Animator>().SetBool("Open1", true);
            pivotPanel.transform.parent.GetComponent<Animator>().SetBool("Close1", false);
        }
        
    }

    public void EnableButtonPannel()
    {
        buttonVisible.SetActive(true);
        pivotPanel.transform.parent.GetComponent<Animator>().SetBool("Open1", false);
        pivotPanel.transform.parent.GetComponent<Animator>().SetBool("Close1", true);
    }
    
}
