using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPiano : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown( 0 ))
        {
            // 判断：鼠标是否点中当前物体
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            float distance = (mousePos - transform.position).magnitude;
            if(distance < 2 )
            {
                // 取得 AudioSource 组件

                AudioSource audio = GetComponent<AudioSource>();
                audio.Play();

                //if(! audio.isPlaying)
                //{
                //    audio.Play();
                //}

                //if(audio.isPlaying)
                //{
                //    audio.Stop();
                //}
                //else
                //{
                //    audio.Play();
                //}
            }
        }
    }
}
