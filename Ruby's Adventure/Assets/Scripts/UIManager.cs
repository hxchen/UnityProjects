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

    public Image healthBar;//角色血条
    public Text bulletCountText;//子弹数量显示



    void Awake()
    {
        instance = this;
    }

    

    /// <summary>
    /// 更新血条
    /// </summary>
    /// <param name="curAmount"></param>
    /// <param name="amout"></param>
    public void UpdatHealthBar(int curAmount, int amout)
    {
        healthBar.fillAmount = (float)curAmount / amout;
    }

    /// <summary>
    /// 更新子弹数量
    /// </summary>
    /// <param name="curAmount"></param>
    /// <param name="maxAmount"></param>
    public void UpdateBulletCount(int curAmount, int maxAmount)
    {
        Debug.Log("更新子弹数量" + curAmount.ToString() + " / " + maxAmount.ToString());
        bulletCountText.text = curAmount.ToString() + " / " + maxAmount.ToString();

    }
}
