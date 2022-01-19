using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyJet : MonoBehaviour
{
    public GameObject bulletPrefab;
    private float count = 0;
    // 定时器间隔
    public float interval = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        if (count >= interval)
        {
            count = 0;
            Fire();
        }

        // 键盘事件
        float step = 2.5f * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-step, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(step, 0, 0);
        }
    }

    void Fire()
    {
        // Instantiate 动态创建实例
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = this.transform.position + new Vector3(0, 1f, 0);
    }
}
