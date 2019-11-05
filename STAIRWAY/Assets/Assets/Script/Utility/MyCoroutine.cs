using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCoroutine : MonoBehaviour {
    public static IEnumerator MyWaitForSeconds(float time)
    {
        float tmp = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup<(time+tmp))
        {
            yield return null; 
        }
    }
}
