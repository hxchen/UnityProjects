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
    /// <summary>
    /// 返回相邻炸弹
    /// </summary>
    /// <returns></returns>
    private List<GameObject> IsAdjacentBomb(Dot dot1, Dot dot2, Dot dot3) {
        List<GameObject> currentDots = new List<GameObject>();

        if (dot1.isAdjacentBomb) {
            currentDots.Union(GetAdjacentPieces(dot1.column, dot1.row));
        }
        if (dot2.isAdjacentBomb) {
            currentDots.Union(GetAdjacentPieces(dot2.column, dot2.row));
        }
        if (dot3.isAdjacentBomb) {
            currentDots.Union(GetAdjacentPieces(dot3.column, dot3.row));
        }

        return currentDots;
    }

    private List<GameObject> IsRowBomb(Dot dot1, Dot dot2, Dot dot3) {
        List<GameObject> currentDots = new List<GameObject>();

        if (dot1.isRowBomb) {
            currentDots.Union(GetRowPieces(dot1.row));
        }
        if (dot2.isRowBomb) {
            currentDots.Union(GetRowPieces(dot2.row));
        }
        if (dot3.isRowBomb) {
            currentDots.Union(GetRowPieces(dot3.row));
        }

        return currentDots;

    }

    private List<GameObject> IsColumnBomb(Dot dot1, Dot dot2, Dot dot3) {
        List<GameObject> currentDots = new List<GameObject>();

        if (dot1.isColumnBomb) {
            currentDots.Union(GetColumnPieces(dot1.column));
        }
        if (dot2.isColumnBomb) {
            currentDots.Union(GetColumnPieces(dot2.column));
        }
        if (dot3.isColumnBomb) {
            currentDots.Union(GetColumnPieces(dot3.column));
        }

        return currentDots;

    }

    private void AddToListAndMatch(GameObject dot) {
        if (!currentMatches.Contains(dot)) {
            currentMatches.Add(dot);
        }
        dot.GetComponent<Dot>().isMatched = true;
    }

    private void GetNearbyPieces(GameObject dot1, GameObject dot2, GameObject dot3) {
        AddToListAndMatch(dot1);
        AddToListAndMatch(dot2);
        AddToListAndMatch(dot3);
    }

    private IEnumerator FindAllMatchesCo() {
        //  yield return new WaitForSeconds(0.2f);
        yield return null;
        for (int i = 0; i < board.width; i++) {
            for (int j = 0; j < board.height; j++) {
                GameObject currentDot = board.allDots[i, j];
                if (currentDot != null) {
                    Dot currentDotDot = currentDot.GetComponent<Dot>();
                    if (i > 0 && i < board.width - 1) {
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];
                        if (leftDot! != null && rightDot != null && leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag) {

                            Dot leftDotDot = leftDot.GetComponent<Dot>();
                            Dot rightDotDot = rightDot.GetComponent<Dot>();

                            currentMatches.Union(IsRowBomb(leftDotDot, currentDotDot, rightDotDot));

                            currentMatches.Union(IsColumnBomb(leftDotDot, currentDotDot, rightDotDot));

                            currentMatches.Union(IsAdjacentBomb(leftDotDot, currentDotDot, rightDotDot));

                            GetNearbyPieces(leftDot, currentDot, rightDot);

                            
                        }
                    }

                    if (j > 0 && j < board.height - 1) {
                        GameObject upDot = board.allDots[i, j + 1];
                        GameObject downDot = board.allDots[i, j - 1];
                        if (upDot! != null && downDot != null && upDot.tag == currentDot.tag && downDot.tag == currentDot.tag) {

                            Dot upDotDot = upDot.GetComponent<Dot>();
                            Dot downDotDot = downDot.GetComponent<Dot>();

                            currentMatches.Union(IsColumnBomb(upDotDot, currentDotDot, downDotDot));

                            currentMatches.Union(IsRowBomb(upDotDot, currentDotDot, downDotDot));

                            currentMatches.Union(IsAdjacentBomb(upDotDot, currentDotDot, downDotDot));

                            GetNearbyPieces(upDot, currentDot, downDot);
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
                
                Dot dot = board.allDots[column, i].GetComponent<Dot>();
                //检查行炸弹
                if (dot.isRowBomb) {
                    dots.Union(GetRowPieces(i)).ToList();
                }

                dots.Add(board.allDots[column, i]);
                dot.isMatched = true;
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
                
                Dot dot = board.allDots[i, row].GetComponent<Dot>();
                //检查列炸弹
                if (dot.isColumnBomb) {
                    dots.Union(GetColumnPieces(i)).ToList();
                }

                dots.Add(board.allDots[i, row]);
                dot.isMatched = true;
            }
        }

        return dots;
    }
    /// <summary>
    /// 邻居炸弹
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    List<GameObject> GetAdjacentPieces(int column, int row) {
        List<GameObject> dots = new List<GameObject>();
        for (int i = column - 1; i <= column + 1; i++) {
            for (int j = row - 1; j <= row + 1; j++) {
                if (i >= 0 && i < board.width && j >= 0 && j < board.height) {
                    if (board.allDots[i, j] != null) {
                        dots.Add(board.allDots[i, j]);
                        board.allDots[i, j].GetComponent<Dot>().isMatched = true;
                    }
                }
            }
        }

        return dots;
    }

    public void CheckBombs(MatchType matchType) {
        // 玩家在移动？
        if (board.currentDot != null) {
            // 移动的点是匹配的？
            if (board.currentDot.isMatched && board.currentDot.tag == matchType.color) {
                // 设置不匹配
                board.currentDot.isMatched = false;
                
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
                if (otherDot.isMatched && otherDot.tag == matchType.color) {
                    otherDot.isMatched = false;
                    
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
