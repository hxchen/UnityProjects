using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyJet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = 1.8f * Time.deltaTime;
        transform.Translate(0, step, 0);
    }

    // 碰撞事件回调
    // 碰撞发生 collision表示对方碰撞体组件
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("飞机：OnTriggerEnter2D");
        Debug.Log("飞机撞到了：" + collision.name);
        Destroy(collision.gameObject);
    }
    // 碰撞事件回调
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("飞机：OnTriggerStay2D");
    }
    // 碰撞事件回调
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("飞机：OnTriggerExit2D");
    }
}
