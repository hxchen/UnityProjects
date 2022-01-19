using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGirl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("22 出去玩吧, time = " + Time.time);
            // Response();
            //if (!IsInvoking("Response"))
            //{
            //    Invoke("Response", 3f);
                
            //}
            InvokeRepeating("Response", 1f, 2f);
        }
    }

    void Response()
    {
        Debug.Log("好啊，我来了! time = " + Time.time);
    }
}
