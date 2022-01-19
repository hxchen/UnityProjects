using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGame : MonoBehaviour
{
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int value)
    {
        this.score += value;
        Debug.Log("得分:" + value + ", 总分:" + this.score);
    }
}
