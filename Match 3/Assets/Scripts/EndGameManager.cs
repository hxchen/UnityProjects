using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameType {
    Moves,
    Time
}
[System.Serializable]
public class EndGameRequiremants {
    public GameType gameType;
    public int counterValue;

}

public class EndGameManager : MonoBehaviour {



    public EndGameRequiremants requiremants;

    public GameObject movesLabel;
    public GameObject timeLabel;
    public GameObject youWinPanel;
    public GameObject tryAgainPanel;

    public Text counter;
    public int currentCounterValue;

    private Board board;
    private float timerSeconds;


    // Start is called before the first frame update
    void Start() {
        board = FindObjectOfType<Board>();
        SetGameType();
        Setup();
    }

    void SetGameType() {
        if (board.world != null) {
            if (board.level < board.world.levels.Length) {
                if (board.world.levels[board.level] != null) {
                    requiremants = board.world.levels[board.level].endGameRequiremants;
                }
            }
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Setup() {
        currentCounterValue = requiremants.counterValue;
        if (requiremants.gameType == GameType.Moves) {
            movesLabel.SetActive(true);
            timeLabel.SetActive(false);

        } else {
            timerSeconds = 1;
            movesLabel.SetActive(false);
            timeLabel.SetActive(true);
        }
        counter.text = "" + currentCounterValue;
    }

    /// <summary>
    /// 减少倒数或者倒计时
    /// </summary>
    public void DecreaseCounterValue() {
        if (board.currentState != GameState.pause) {
            currentCounterValue--;
            counter.text = "" + currentCounterValue;

            if (currentCounterValue <= 0) {
                LoseGame();
            }
        }
        
    }
    /// <summary>
    /// 获胜
    /// </summary>
    public void WinGame() {

        youWinPanel.SetActive(true);
        board.currentState = GameState.win;
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;

        FadePanelController fade = FindObjectOfType<FadePanelController>();
        fade.GameOver();

    }
    /// <summary>
    /// 失败
    /// </summary>
    public void LoseGame() {
        tryAgainPanel.SetActive(true);
        board.currentState = GameState.lose;
        Debug.Log("You Lose!");
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;

        FadePanelController fade = FindObjectOfType<FadePanelController>();
        fade.GameOver();
    }

    void Update() {
        if (requiremants.gameType == GameType.Time && currentCounterValue > 0) {
            timerSeconds -= Time.deltaTime;
            if (timerSeconds <= 0) {
                DecreaseCounterValue();
                timerSeconds = 1;
            }
        }
    }

}
