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

    public Image[] stars;
    public Text levelText;
    public int level;
    public GameObject confirmPanel;


    void Start() {
        buttonImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        ActivateStarts();
        ShowLevel();
        DecideSprite();
    }

    void ActivateStarts() {
        // TODO 星级展示
        for (int i = 0; i < stars.Length; i++) {
            stars[i].enabled = false;
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
    


