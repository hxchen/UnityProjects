using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 父节点子节点
        foreach (Transform transform in transform)
        {
            Debug.Log(transform.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
