using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// NPC 控制相关
/// </summary>
public class NPCManager : MonoBehaviour
{

    public GameObject dialogImage;
    public GameObject tipDialogFrame;

    public float showTime = 4;
    private float showTimer;


    // Start is called before the first frame update
    void Start()
    {
        dialogImage.SetActive(false);
        tipDialogFrame.SetActive(false);
        showTimer = -1;
    }

    // Update is called once per frame
    void Update()
    {
        showTimer -= Time.deltaTime;
        if (showTimer < 0)
        {
            tipDialogFrame.SetActive(true);
            dialogImage.SetActive(false);
            
        }
    }

    public void showDialog()
    {
        showTimer = showTime;
        tipDialogFrame.SetActive(false);
        dialogImage.SetActive(true);

    }
}
