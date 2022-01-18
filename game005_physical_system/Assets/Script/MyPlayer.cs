using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("检测到了碰撞，对方是:" + collision.tag);
        if (collision.tag.Equals("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.tag.Equals("Player"))
        {
            Debug.Log("朋友你好");
        }
    }
}
