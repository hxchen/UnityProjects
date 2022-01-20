using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyGame : MonoBehaviour
{
    public InputField userField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 事件处理函数
    public void Login()
    {
        string username = userField.text;
        Debug.Log("登录" + username);
    }
}
