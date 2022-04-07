using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {

    public GameObject pausePanel;
    private Board board;
    public bool paused = false;
    public Image soundButton;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start() {
        audioManager = FindObjectOfType<AudioManager>();
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
        pausePanel.SetActive(false);
        // 用户 Prefs里， Sound的KEY 管理声音
        // sound == 0，静音；sound == 1，开启声音
        if (PlayerPrefs.HasKey("Sound")) {
            if (PlayerPrefs.GetInt("Sound") == 0) {
                soundButton.sprite = musicOffSprite;
            } else {
                soundButton.sprite = musicOnSprite;
            }
        } else {
            soundButton.sprite = musicOnSprite;
        }
    }

    // Update is called once per frame
    void Update() {
        
        if (paused && !pausePanel.activeInHierarchy) {
            pausePanel.SetActive(true);
            board.currentState = GameState.pause;
        }

        if (!paused && pausePanel.activeInHierarchy) {
            pausePanel.SetActive(false);
            board.currentState = GameState.move;
        }
    }
    // 音乐声音控制
    public void SoundButton() {
        if (PlayerPrefs.HasKey("Sound")) {
            if (PlayerPrefs.GetInt("Sound") == 0) {
                soundButton.sprite = musicOnSprite;
                PlayerPrefs.SetInt("Sound", 1);
                audioManager.adjustVolumn();
            } else {
                soundButton.sprite = musicOffSprite;
                PlayerPrefs.SetInt("Sound", 0);
                audioManager.adjustVolumn();
            }
        } else {
            soundButton.sprite = musicOffSprite;
            PlayerPrefs.SetInt("Sound", 1);
            audioManager.adjustVolumn();
        }
    }
    // 暂停
    public void PauseGame() {
        paused = !paused;
    }
    // 退出到菜单
    public void ExitGame() {
        SceneManager.LoadScene("Splash");
    }
}
