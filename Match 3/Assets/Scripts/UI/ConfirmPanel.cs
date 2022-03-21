using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 菜单选择关卡后的确认面板
/// </summary>
public class ConfirmPanel : MonoBehaviour {

    public string levelToLoad;
    public Image[] stars;
    public int level;


    void Start() {
        ActivateStarts();
    }

    /// <summary>
    /// 激活星星展示
    /// </summary>
    void ActivateStarts() {
        // TODO 星级展示
        for (int i = 0; i < stars.Length; i++) {
            stars[i].enabled = false;
        }
    }


    public void Cancel() {
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// 开始关卡游戏
    /// </summary>
    public void Play() {
        PlayerPrefs.SetInt("Current Level", level - 1);
        SceneManager.LoadScene(levelToLoad);
    }
}
