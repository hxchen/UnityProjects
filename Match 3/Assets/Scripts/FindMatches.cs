using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatches : MonoBehaviour {

    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>(); 

    // Start is called before the first frame update
    void Start() {
        board = FindObjectOfType<Board>();

    }

    public void FindAllMatches() {
        StartCoroutine(FindAllMatchesCo());
    }

    private IEnumerator FindAllMatchesCo() {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < board.width; i++) {
            for (int j = 0; j < board.height; j++) {
                GameObject currentDot = board.allDots[i, j];
                if (currentDot != null) {
                    if (i > 0 && i < board.width - 1) {
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];
                        if (leftDot! != null && rightDot != null && leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag) {

                            // 炸弹检查
                            if (currentDot.GetComponent<Dot>().isRowBomb
                                || leftDot.GetComponent<Dot>().isRowBomb
                                || rightDot.GetComponent<Dot>().isRowBomb) {
                                currentMatches.Union(GetRowPieces(j));
                            }

                            if (currentDot.GetComponent<Dot>().isColumnBomb) {
                                currentMatches.Union(GetColumnPieces(i));
                            }
                            if (leftDot.GetComponent<Dot>().isColumnBomb) {
                                currentMatches.Union(GetColumnPieces(i - 1));
                            }
                            if (rightDot.GetComponent<Dot>().isColumnBomb) {
                                currentMatches.Union(GetColumnPieces(i + 1));
                            }



                            if (!currentMatches.Contains(leftDot)) {
                                currentMatches.Add(leftDot);
                            }
                            if (!currentMatches.Contains(rightDot)) {
                                currentMatches.Add(rightDot);
                            }
                            if (!currentMatches.Contains(currentDot)) {
                                currentMatches.Add(currentDot);
                            }
                              
                            leftDot.GetComponent<Dot>().isMatched = true;
                            rightDot.GetComponent<Dot>().isMatched = true;
                            currentDot.GetComponent<Dot>().isMatched = true;
                        }
                    }

                    if (j > 0 && j < board.height - 1) {
                        GameObject upDot = board.allDots[i, j + 1];
                        GameObject downDot = board.allDots[i, j - 1];
                        if (upDot! != null && downDot != null && upDot.tag == currentDot.tag && downDot.tag == currentDot.tag) {

                            // 炸弹检查
                            if (currentDot.GetComponent<Dot>().isColumnBomb
                                || upDot.GetComponent<Dot>().isColumnBomb
                                || downDot.GetComponent<Dot>().isColumnBomb) {
                                currentMatches.Union(GetColumnPieces(i));

                            }
                            if (currentDot.GetComponent<Dot>().isRowBomb) {
                                currentMatches.Union(GetRowPieces(j));
                            }
                            if (upDot.GetComponent<Dot>().isRowBomb) {
                                currentMatches.Union(GetRowPieces(j + 1));
                            }
                            if (downDot.GetComponent<Dot>().isRowBomb) {
                                currentMatches.Union(GetRowPieces(j - 1));
                            }


                            if (!currentMatches.Contains(upDot)) {
                                currentMatches.Add(upDot);
                            }
                            if (!currentMatches.Contains(downDot)) {
                                currentMatches.Add(downDot);
                            }
                            if (!currentMatches.Contains(currentDot)) {
                                currentMatches.Add(currentDot);
                            }

                            upDot.GetComponent<Dot>().isMatched = true;
                            downDot.GetComponent<Dot>().isMatched = true;
                            currentDot.GetComponent<Dot>().isMatched = true;
                        }
                    }

                }
            }
        }
    }

    /// <summary>
    /// 匹配一个颜色
    /// </summary>
    /// <param name="color">标签名</param>
    public void MatchPiecesOfColor(string color) {
        for (int i = 0; i < board.width; i++) {
            for (int j = 0; j < board.height; j++) {
                if (board.allDots[i, j] != null) {
                    if (board.allDots[i, j].tag == color) {
                        board.allDots[i, j].GetComponent<Dot>().isMatched = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 获取当前列的点
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    List<GameObject> GetColumnPieces(int column) {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.height; i++) {
            if (board.allDots[column, i] != null) {
                dots.Add(board.allDots[column, i]);
                board.allDots[column, i].GetComponent<Dot>().isMatched = true;
            }
        }

        return dots;
    }

    /// <summary>
    /// 获取当前行的点
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    List<GameObject> GetRowPieces(int row) {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.width; i++) {
            if (board.allDots[i, row] != null) {
                dots.Add(board.allDots[i, row]);
                board.allDots[i, row].GetComponent<Dot>().isMatched = true;
            }
        }

        return dots;
    }

    public void CheckBombs() {
        // 玩家在移动？
        if (board.currentDot != null) {
            // 移动的点是匹配的？
            if (board.currentDot.isMatched) {
                // 设置不匹配
                board.currentDot.isMatched = false;
                /**
                int typeOfBomb = Random.Range(0, 100);

                if(typeOfBomb < 50){
                    // 行炸弹
                    board.currentDot.MakeRowBomb();
                } else if (typeOfBomb >= 50) {
                    // 列炸弹
                    board.currentDot.MakeColumnBomb();
                }  
                Debug.Log("把当前点变为炸弹");
                **/
                if ((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45)
                    || (board.currentDot.swipeAngle < -135 || board.currentDot.swipeAngle >= 135)) {
                    // 右滑动或者左滑动
                    board.currentDot.MakeRowBomb();
                } else {
                    board.currentDot.MakeColumnBomb();
                }  
            } else if(board.currentDot.otherDot != null){
                // 另一个点匹配
                Dot otherDot = board.currentDot.otherDot.GetComponent<Dot>();
                if (otherDot.isMatched) {
                    otherDot.isMatched = false;
                    /*
                    int typeOfBomb = Random.Range(0, 100);

                    if (typeOfBomb < 50) {
                        // 行炸弹
                        otherDot.MakeRowBomb();
                    } else if (typeOfBomb >= 50) {
                        // 列炸弹
                        otherDot.MakeColumnBomb();
                    }
                    Debug.Log("把另个点变为炸弹");
                    */
                    if ((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45)
                    || (board.currentDot.swipeAngle < -135 || board.currentDot.swipeAngle >= 135)) {
                        // 右滑动或者左滑动
                        otherDot.MakeRowBomb();
                    } else {
                        otherDot.MakeColumnBomb();
                    }
                }

            }
        }
    }
}
