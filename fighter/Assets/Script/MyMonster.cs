using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMonster : MonoBehaviour
{
    public float speed = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dy = speed * Time.deltaTime;
        transform.Translate(0, -dy, 0, Space.Self);
        // 屏幕外的怪物销毁
        Vector3 sp = Camera.main.WorldToScreenPoint(transform.position);
        if (sp.y < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
