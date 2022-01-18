using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = 1.2f * Time.deltaTime;
        this.transform.Translate(0, step, 0, Space.Self);
        // 实例的销毁
        Vector3 sp = Camera.main.WorldToScreenPoint(transform.position);
        if (sp.y > Screen.height)
        {
            Destroy(this.gameObject);
        }
    }
}
