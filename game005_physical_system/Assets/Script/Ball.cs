using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("碰撞发生");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("碰撞中");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("碰撞结束");
    }
}
