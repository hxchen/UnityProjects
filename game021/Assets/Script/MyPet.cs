using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPet : MonoBehaviour
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
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            float distance = (mousePos - transform.position).magnitude;
            if (distance < 2)
            {
                // 方法一：调用主控函数
                //GameObject main = GameObject.Find("游戏主控");
                //MyGame myGame = main.GetComponent<MyGame>();
                //myGame.AddScore(1);

                // 方法二：消息调用
                GameObject main = GameObject.Find("游戏主控");
                // 查找目标方法并立即执行
                // 遍历该对象上所有MonoBehaviour组件
                main.SendMessage("AddScore", 2);
            }
        }
    }
}
