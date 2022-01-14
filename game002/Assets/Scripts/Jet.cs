using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jet : MonoBehaviour
{
    public float speed = 0.05f;
    GameObject ball = null;

    // Start is called before the first frame update
    void Start()
    {
        changeDirection();
    }

    // Update is called once per frame
    void Update()
    {
        flyToTarget();
    }

    // 调转机头
    void changeDirection()
    {
        ball = GameObject.Find("小球");
        Vector3 head = this.transform.up;

        Vector3 direction = ball.transform.position - this.transform.position;


        float angle = Vector3.SignedAngle(head, direction, Vector3.forward);
        Debug.Log("夹角:" + angle);

        this.transform.Rotate(0, 0, angle);
    }

    // 飞向目标
    void flyToTarget()
    {
        
        float step = speed * Time.deltaTime;
        Vector3 direction = this.transform.position - ball.transform.position;

        if (direction.magnitude > 0.5)
        {
            // 沿着自己Y轴方向运动
            this.transform.Translate(0, step, 0, Space.Self);
        }
        
    }   
}
