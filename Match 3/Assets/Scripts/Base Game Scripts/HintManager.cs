using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 消除提示
/// </summary>
public class HintManager : MonoBehaviour {

    private Board board;

    public float hintDelay;

    private float hintDelaySeconds;

    public GameObject hintParticle;

    public GameObject currentHint;

    void Start() {
        board = FindObjectOfType<Board>();
        hintDelaySeconds = hintDelay;
    }

    void Update() {
        hintDelaySeconds -= Time.deltaTime;
        // 什么时候出提示呢
        if (hintDelaySeconds <= 0 && currentHint == null && board.currentState == GameState.move) {
            MarkHint();
            hintDelaySeconds = hintDelay;
        }
    }

    //首先找到所有可能的匹配
    List<GameObject> findAllMatches() {
        Debug.Log("******************开始一轮匹配查找**********************");
        List<GameObject> possibleMoves = new List<GameObject>();
        for (int i = 0; i < board.width; i++) {
            for (int j = 0; j < board.height; j++) {
                if (board.allDots[i, j] != null) {
                    if (i < board.width - 1) {
                        if (board.SwitchAndCheck(i, j, Vector2.right)) {
                            possibleMoves.Add(board.allDots[i, j]);
                            Debug.Log("[" + i + "," +j+ "]右边合适");
                        }

                    }
                    if (j < board.height - 1) {
                        if (board.SwitchAndCheck(i, j, Vector2.up)) {
                            possibleMoves.Add(board.allDots[i, j]);
                            Debug.Log("[" + i + "," + j + "]上边合适");
                        }
                    }
                }
            }
        }
        return possibleMoves;

    }
    //随机选择一个
    GameObject PickOneRandomly() {
        List<GameObject> possibleMoves = new List<GameObject>();
        possibleMoves = findAllMatches();
        if (possibleMoves.Count > 0) {
            int pieceToUse = Random.Range(0, possibleMoves.Count);
            return possibleMoves[pieceToUse];
        }
        return null;
    }
    //在选中的匹配上创建提示
    private void MarkHint() {
        GameObject move = PickOneRandomly();
        Debug.Log("被选中的" + move.tag);
        if (move != null) {
            currentHint = Instantiate(hintParticle, move.transform.position, Quaternion.identity);
        }
    }
    //销毁提示
    public void DestroyHint() {
        if (currentHint != null) {
            Destroy(currentHint);
            currentHint = null;
            hintDelaySeconds = hintDelay;
        }
    }


}
