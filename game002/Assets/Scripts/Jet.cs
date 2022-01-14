using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jet : MonoBehaviour
{
    public float speed = 2f;
    private bool right = true;
    private GameObject ball = null;

    // Start is called before the first frame update
    void Start()
    {
        // ChangeDirection();
        // HeadUp();

        // ResetPostion();
    }

    // Update is called once per frame
    void Update()
    {
        // FlyToTarget();
        // LeftRihgtFly();
        FlyToMousePos();
    }

    // 调转机头
    void ChangeDirection()
    {
        ball = GameObject.Find("小球");
        Vector3 head = this.transform.up;

        Vector3 direction = ball.transform.position - this.transform.position;


        float angle = Vector3.SignedAngle(head, direction, Vector3.forward);
        Debug.Log("夹角:" + angle);

        this.transform.Rotate(0, 0, angle);
    }

    // 飞向目标
    void FlyToTarget()
    {
        
        float step = speed * Time.deltaTime;
        Vector3 direction = this.transform.position - ball.transform.position;

        if (direction.magnitude > 0.5)
        {
            // 沿着自己Y轴方向运动
            this.transform.Translate(0, step, 0, Space.Self);
        }

        


    }
    // 屏幕坐标
    void ScreenCoordinate()
    {
        //int screenW = Screen.width;
        //int screenH = Screen.height;
        //Debug.Log("屏幕的宽高, w = " + screenW + ", h = " + screenH);

        Vector3 worldPos = this.transform.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Debug.Log("屏幕坐标," + screenPos);        
    }

    // 机头摆正Up
    void HeadUp()
    {
        transform.eulerAngles = Vector3.up;

    }

    // 归位
    void ResetPostion()
    {
        transform.position = Vector3.zero;
    }

    // 小飞机左右屏幕内飞行
    void LeftRihgtFly()
    {
        Vector3 worldPos = this.transform.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        int screenW = Screen.width;
        Debug.Log("当前屏幕x坐标:" + screenPos[0] + ", 屏幕宽度" + screenW);
        if (right && screenPos[0] < screenW)
        {
            // 向右飞行
            right = true;
            float step = speed * Time.deltaTime;
            this.transform.Translate(step, 0, 0, Space.Self);
        }
        else
        {
            right = false;
            if (screenPos[0] <= 0)
            {
                right = true;
            }
            if (!right)
            {
                float step = speed * Time.deltaTime;
                this.transform.Translate(-step, 0, 0, Space.Self);
            }
            
        }
    }

    // 飞向鼠标所在位置
    void FlyToMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
        Debug.Log("鼠标位置" + mousePos);
        Vector3 mouseWdPos = Camera.main.ScreenToWorldPoint(mousePos);

        // 转向
        Vector3 head = this.transform.up;
        Debug.Log("头朝向" + head);
        Vector3 direction = mouseWdPos - this.transform.position;
        float angle = Vector3.SignedAngle(head, direction, Vector3.forward);
        if ( angle != 0)
        {
            this.transform.Rotate(0, 0, angle);
        }

        // 移动
        float step = speed * Time.deltaTime;
        if (direction.magnitude > 0.5)
        {
            // 沿着自己Y轴方向运动
            this.transform.Translate(0, step, 0, Space.Self);

        }


    }
    
}
