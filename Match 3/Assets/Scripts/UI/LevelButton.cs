using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 菜单关卡选择
/// </summary>
public class LevelButton : MonoBehaviour {
    [Header("Active Stuff")]
    public bool isActive;
    public Sprite activeSprite;
    public Sprite lockedSprite;
    private Image buttonImage;
    private Button myButton;
    private int starsActive;

    [Header("Level UI")]
    public Image[] stars;
    public Text levelText;
    public int level;
    public GameObject confirmPanel;
    


    private GameData gameData;

    void Start() {
        gameData = FindObjectOfType<GameData>();
        buttonImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        LoadData();
        ActivateStarts();
        ShowLevel();
        DecideSprite();
    }
    /// <summary>
    /// 加载数据
    /// </summary>
    void LoadData() {
        if (gameData != null) {
            // 激活
            if (gameData.saveData.isActive[level - 1]) {
                isActive = true;
            } else {
                isActive = false;
            }
            //星级展示
            starsActive = gameData.saveData.stars[level - 1];
            //Debug.Log("level = " + level + ", stars = " + starsActive);

        }
    }


    void ActivateStarts() {
        for (int i = 0; i < starsActive; i++) {
            stars[i].enabled = true;
        }
    }

    /// <summary>
    /// 显示激活还是锁定图片
    /// </summary>
    void DecideSprite() {

        if (isActive) {
            buttonImage.sprite = activeSprite;
            myButton.enabled = true;
            levelText.enabled = true; 
        } else {
            buttonImage.sprite = lockedSprite;
            myButton.enabled = false;
            levelText.enabled = false;

        }
    }

    void ShowLevel() {
        levelText.text = "" + level;
    }

    public void ConfirmPanel(int level) {
        confirmPanel.GetComponent<ConfirmPanel>().level = level;
        confirmPanel.SetActive(true);
    }

}
    


