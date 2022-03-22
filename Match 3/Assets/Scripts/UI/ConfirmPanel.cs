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
    private int starsActive;
    public int level;
    private GameData gamaData;


    void Start() {
        gamaData = FindObjectOfType<GameData>();
        LoadData();
        ActivateStarts();
    }

    void LoadData() {
        if (gamaData != null) {
            starsActive = gamaData.saveData.stars[level - 1];
        }
    }
    /// <summary>
    /// 激活星星展示
    /// </summary>
    void ActivateStarts() {
        for (int i = 0; i < starsActive; i++) {
            stars[i].enabled = true;
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
