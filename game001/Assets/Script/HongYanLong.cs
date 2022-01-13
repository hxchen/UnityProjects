using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 红岩龙 
 */
public class HongYanLong : MonoBehaviour
{
    // 组件的属性
    public int number = 5;
    public string teamName = "自定义编队";
    public float speed = 0.05f;
    // 飞行方向向上
    private bool upWard = true;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Hello, world!");

        // 获取当前节点
        //SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        //renderer.flipX = true;

        // 获取其他节点
        //GameObject gameObject = GameObject.Find("巨口鸟龙");
        //SpriteRenderer renderer2 = gameObject.GetComponent<SpriteRenderer>();
        //renderer2.flipY = true;

        // 获取父节点
        //GameObject gameObject = this.transform.parent.gameObject;
        //Debug.Log(gameObject.name);

        // 坐标
        // this.transform.position = new Vector3(1, 1.0f, 0);
        // 转转
        // transform.eulerAngles = new Vector3(0, 0, 45f);
        // 本地坐标
        // this.transform.localPosition = new Vector3(0, 1.0f, 0);

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update" + Time.deltaTime);

        // this.transform.Translate(-0.0005f, 0, 0);

        // 掉头运动检测
        if (upWard && transform.position.y > 5)
        {
            upWard = false;
            this.transform.localEulerAngles = new Vector3(0, 0, 180);
        }

        if (!upWard && transform.position.y < -5)
        {
            upWard = true;
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        // 物体的运动(Translate)
        float step = speed * Time.deltaTime;
        // 世界坐标系
        // this.transform.Translate(step, 0, 0, Space.World);
        // 自身坐标系
        this.transform.Translate(0, step, 0, Space.Self);

        

    }
}
