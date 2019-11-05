using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizeMenu : MonoBehaviour {
    
    /// <summary>
    /// /Transformations' Order:
    /// 1. Translate to origin (0,0)
    /// 2. Scale
    /// 3. Rotate
    /// 4. Translate to the desired position
    /// </summary>
    
    public Vector3 rotation;

    RectTransform rectTransform;
    
    float w, h,angle;
    Vector2 referenceResolution= new Vector2(720,1280);
    void Awake()
    {
        w = Screen.width;
        h = Screen.height*3/2f;
        angle = rotation.z-90;

        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    void Start () {
            Transforms();

       
    }

    //All the transforms
    void Transforms()
    {
        rectTransform.localPosition = new Vector2(0, 0);
        //rectTransform.sizeDelta = new Vector2(w, h);
        rectTransform.sizeDelta = referenceResolution;

        rectTransform.Rotate(0, 0, angle);

        rectTransform.localPosition += h * new Vector3(-Mathf.Sin(angle * Mathf.PI / 180f), Mathf.Cos(angle * Mathf.PI / 180f), 0);// * new Vector3(Mathf.Sin(angle / 180f * Mathf.PI), Mathf.Cos(angle / 180f * Mathf.PI), 0);
    }
    
}
