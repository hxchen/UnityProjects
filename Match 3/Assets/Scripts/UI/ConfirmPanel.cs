using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 菜单选择关卡后的确认面板
/// </summary>
public class ConfirmPanel : MonoBehaviour {

    [Header("Level Information")]
    public string levelToLoad;
    public int level;
    private GameData gamaData;
    private int starsActive;
    private int highScore;

    [Header("UI stuff")]
    public Image[] stars;
    public Text highScoreText;
    public Text starText;
    


    void OnEnable() {
        gamaData = FindObjectOfType<GameData>();
        LoadData();
        ActivateStarts();
        SetText();
    }

    void LoadData() {
        if (gamaData != null) {
            starsActive = gamaData.saveData.stars[level - 1];
            highScore = gamaData.saveData.highScores[level - 1];
        }
    }
    /// <summary>
    /// 填充UI数据
    /// </summary>
    void SetText() {
        highScoreText.text = "" + highScore;
        starText.text = "" + starsActive + "/3";

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
        Debug.Log("ConfirmPanel:Play = " + levelToLoad);
        SceneManager.LoadScene(levelToLoad);
    }
}
