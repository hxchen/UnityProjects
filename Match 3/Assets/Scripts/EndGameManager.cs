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
    public Text counter;
    public int currentCounterValue;
    private float timerSeconds;


    // Start is called before the first frame update
    void Start() {
        Setup();
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

    public void DecreaseCounterValue() {
        currentCounterValue--;
        counter.text = "" + currentCounterValue;

        if (currentCounterValue <= 0) {
            Debug.Log("You Lose!");
            currentCounterValue = 0;
            counter.text = "" + currentCounterValue;
        }
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
