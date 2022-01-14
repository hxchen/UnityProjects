using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 父节点子节点
        //foreach (Transform transform in transform)
        //{
        //    Debug.Log(transform.name);
        //}

        signedAngleOfVector();

    }

    // Update is called once per frame
    void Update()
    {
        // lengthOfObjects();
    }

    void lengthOfObjects()
    {
        // 向量的减法求距离
        GameObject hongyanlong = GameObject.Find("红岩龙");
        GameObject jukouniaolong = GameObject.Find("巨口鸟龙");
        Vector3 direction = hongyanlong.transform.position - jukouniaolong.transform.position;
        Debug.Log("红岩龙和巨口鸟龙的距离是:" + direction.magnitude);

    }

    // 向量夹角
    void signedAngleOfVector()
    {
        Vector3 a = new Vector3(2, 2, 0);
        Vector3 b = new Vector3(-1, 3, 0);

        // 带符号的角度SignedAngle
        float angle = Vector3.SignedAngle(a, b, Vector3.forward);
        Debug.Log("向量的夹角是:" + angle);

    }

}
