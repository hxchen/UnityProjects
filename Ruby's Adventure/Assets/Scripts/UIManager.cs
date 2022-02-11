using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI管理相关
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    void Awake()
    {
        instance = this;
    }

    public Image healthBar;//角色血条

    public void UpdatHealthBar(int curAmount, int amout)
    {
        healthBar.fillAmount = (float)curAmount / amout;
    }

}
