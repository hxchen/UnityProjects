using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGun : MonoBehaviour
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
                AudioSource audio = GetComponent<AudioSource>();
                audio.PlayOneShot(audio.clip);
                // audio.Play();
            }
        }
    }
}
