using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    wait,
    move
}

public enum TileKind {
    Breakable,
    Blank,
    Normal
}
// 告诉编辑器序列化
[System.Serializable]
public class TileType {
    public int x;
    public int y;
    public TileKind tileKind;
}

public class Board : MonoBehaviour {

    public GameState currentState = GameState.move;
    public int width;
    public int height;
    public int offset;
    public GameObject tilePrefab;
    public GameObject breakableTilePrefab;
    public GameObject destroyEffect;
    // 特殊点
    public TileType[] boardLayout;
    private bool[,] blankSpaces;
    private BackgroundTile[,] breakableTiles;
    // 所有样式
    public GameObject[] dots;

    public GameObject[,] allDots;
    public Dot currentDot;
    private FindMatches findMatches;
    //单点得分
    public int basePieceValue = 20;
    private int streakValue = 1;
    private ScoreManager scoreManager;
    public float refillDelay = 0.5f;
    public int[] scoreGoals; 


    // Start is called before the first frame update
    void Start() {
        scoreManager = FindObjectOfType<ScoreManager>();
        breakableTiles = new BackgroundTile[width, height];
        findMatches = FindObjectOfType<FindMatches>();
        blankSpaces = new bool[width, height];
        allDots = new GameObject[width, height];
        SetUp();
    }
    /// <summary>
    /// 生成空白点
    /// </summary>
    public void GenerateBlankSpaces() {
        for (int i = 0; i < boardLayout.Length; i++) {
            if (boardLayout[i].tileKind == TileKind.Blank) {
                blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
            }
        }
    }
    /// <summary>
    /// 生成易碎瓦片
    /// </summary>
    public void GenerateBreakableTiles() {

        for (int i = 0; i < boardLayout.Length; i++) {
            if (boardLayout[i].tileKind == TileKind.Breakable) {
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(breakableTilePrefab, tempPosition, Quaternion.identity);
                breakableTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }

    private void SetUp() {
        // 生成空白点
        GenerateBlankSpaces();
        // 生成果冻点
        GenerateBreakableTiles();

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (!blankSpaces[i, j]) {
                    Vector2 tempPostion = new Vector2(i, j + offset);
                    Vector2 tilePosition = new Vector2(i, j);
                    GameObject backgroundTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                    backgroundTile.transform.parent = this.transform;
                    backgroundTile.name = "( " + i + ", " + j + " )";

                    int dotToUse = Random.Range(0, dots.Length);
                    // 检查不能有可消除dots
                    int maxIterations = 0;  // 防止样式太少时，无限循环
                    while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100) {
                        dotToUse = Random.Range(0, dots.Length);
                        maxIterations++;
                    }
                    //maxIterations = 0;
                    GameObject dot = Instantiate(dots[dotToUse], tempPostion, Quaternion.identity);
                    dot.GetComponent<Dot>().row = j;
                    dot.GetComponent<Dot>().column = i;

                    dot.transform.parent = this.transform;
                    dot.name = "( " + i + ", " + j + " )";
                    allDots[i, j] = dot;
                }

                
            }
        }
    }
    /// <summary>
    /// 检查是否有三连+
    /// </summary>
    /// <returns></returns>
    private bool MatchesAt(int column, int row, GameObject piece) {
        if (column > 1 && row > 1) {

            if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag) {
                return true;
            }
            if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag) {
                return true;
            }
        } else if (column <= 1 || row <= 1) {
            if (row > 1) {
                if (allDots[column, row - 1] != null && allDots[column, row - 2] != null && allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag) {
                    return true;
                }
            }
            if (column > 1) {
                if (allDots[column - 1, row] != null && allDots[column - 2, row] != null && allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag) {
                    return true;
                }
            }
        }

        return false;
    }
    /// <summary>
    /// 检查是不是一连5个
    /// </summary>
    /// <returns></returns>
    private bool ColumnOrRow() {
        int numberHorizontal = 0;
        int numberVertical = 0;
        Dot firstPiece = findMatches.currentMatches[0].GetComponent<Dot>();
        if (firstPiece != null) {
            foreach (GameObject currentPiece in findMatches.currentMatches) {
                Dot dot = currentPiece.GetComponent<Dot>();
                if (dot.row == firstPiece.row) {
                    numberHorizontal++;
                }
                if (dot.column == firstPiece.column) {
                    numberVertical++;
                }
            }
        }

        return (numberVertical == 5 || numberHorizontal == 5);
    }
    /// <summary>
    /// 生成不同类型炸弹
    /// </summary>
    private void CheckToMakeBombs() {
        if (findMatches.currentMatches.Count == 4 || findMatches.currentMatches.Count == 7) {
            // 行列炸弹
            findMatches.CheckBombs();
        }
        if (findMatches.currentMatches.Count == 5 || findMatches.currentMatches.Count == 8) {
            if (ColumnOrRow()) {
                
                // 一连五个，颜色炸弹
                if (currentDot != null) {
                    if (currentDot.isMatched) {
                        if (!currentDot.isColorBomb) {
                            currentDot.isMatched = false;
                            currentDot.MakeColorBomb();
                        }
                    } else {
                        if (currentDot.otherDot != null) {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if (otherDot.isMatched && !otherDot.isColorBomb) {
                                otherDot.isMatched = false;
                                otherDot.MakeColorBomb();
                            }
                        }
                    }
                    
                }
            } else {
                // 邻近炸弹
                if (currentDot != null) {
                    if (currentDot.isMatched) {
                        if (!currentDot.isAdjacentBomb) {
                            currentDot.isMatched = false;
                            currentDot.MakeAdjacentBomb();
                        }
                    } else {
                        if (currentDot.otherDot != null) {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if (otherDot.isMatched && !otherDot.isAdjacentBomb) {
                                otherDot.isMatched = false;
                                otherDot.MakeAdjacentBomb();
                            }
                        }
                    }

                }
            }
        }
    }
    /// <summary>
    /// 消除
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    private void DestroyMatchesAt(int column, int row) {
        if (allDots[column, row].GetComponent<Dot>().isMatched) {
            //  检查匹配项里面有多少个元素点
            if (findMatches.currentMatches.Count >= 4) {
                CheckToMakeBombs();
            }

            // 检查瓦片是否易碎
            if (breakableTiles[column, row] != null) {
                breakableTiles[column, row].TakeDamage(1);
                if (breakableTiles[column, row].hitPoints <= 0) {
                    breakableTiles[column, row] = null;
                }
            }

            GameObject particle = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            Destroy(particle, 0.5f);
            Destroy(allDots[column, row]);
            scoreManager.IncreaseScore(basePieceValue * streakValue);
            allDots[column, row] = null;
        }
        
    }
    /// <summary>
    /// 消除board上的连点
    /// </summary>
    public void DestroyMatches() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        findMatches.currentMatches.Clear();
        StartCoroutine(DecreaseRowCo2());
    }

    private IEnumerator DecreaseRowCo2() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (!blankSpaces[i, j] && allDots[i, j] == null) {
                    for (int k = j + 1; k < height; k++) {
                        if (allDots[i, k] != null) {
                            allDots[i, k].GetComponent<Dot>().row = j;
                            allDots[i, k] = null;
                            break;
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(refillDelay * 0.5f);
        StartCoroutine(FillBoardCo());
    }

    private IEnumerator DecreaseRowCo() {
        int nullCount = 0;
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] == null)
                {
                    nullCount++;
                } else if (nullCount > 0) {
                    allDots[i, j].GetComponent<Dot>().row -= nullCount;
                    allDots[i, j] = null;
                }
            }
            nullCount = 0;
        } 
        yield return new WaitForSeconds(refillDelay * 0.5f);
        StartCoroutine(FillBoardCo());
    }
    /// <summary>
    /// 填充
    /// </summary>
    private void RefillBoard() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] == null && !blankSpaces[i, j]) {
                    Vector2 tempPosition = new Vector2(i, j + offset);
                    int dotToUse = Random.Range(0, dots.Length);

                    int maxIterations = 0;
                    //Debug.Log("RefillBoard maxIterations = " + maxIterations);
                    while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100) {
                        maxIterations++;
                        dotToUse = Random.Range(0, dots.Length);
                    }
                    maxIterations = 0;

                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;
                } 
            }
        }
    }

    private bool MatchesOnBoard() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    if (allDots[i, j].GetComponent<Dot>().isMatched) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo() {
        RefillBoard();
        yield return new WaitForSeconds(refillDelay);
        while (MatchesOnBoard()) {
            //得分
            streakValue ++;
            DestroyMatches();
            yield return new WaitForSeconds(2* refillDelay);
            
        }
        findMatches.currentMatches.Clear();
        currentDot = null;
        yield return new WaitForSeconds(refillDelay);
        //检查游戏是否能继续
        if (IsDeadlocked()) {
            Debug.Log("Dead Locked!!!");
            StartCoroutine(ShuffleBoard());
        }
        currentState = GameState.move;
        streakValue = 1;
    }
    /// <summary>
    /// 交换元素
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <param name="direction"></param>
    private void SwitchPiece(int column, int row, Vector2 direction) {
        // 保存第二个点
        GameObject holder = allDots[column + (int) direction.x, row + (int) direction.y] as GameObject;
        // 第一个点替换到第二个点
        allDots[column + (int)direction.x, row + (int)direction.y] = allDots[column, row];
        // 第二个点替换第一个点
        allDots[column, row] = holder;
    }
    /// <summary>
    /// 检查是否有可消除元素
    /// </summary>
    /// <returns></returns>
    private bool CheckForMatches() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    if (i < width - 2) {
                        // 检查右边的点
                        if (allDots[i + 1, j] != null && allDots[i + 2, j] != null) {
                            if (allDots[i + 1, j].tag == allDots[i, j].tag && allDots[i + 2, j].tag == allDots[i, j].tag) {
                                return true;
                            }
                        }
                    }
                    if (j < height - 2) {
                        // 检查上面点
                        if (allDots[i, j + 1] != null && allDots[i, j + 2] != null) {
                            if (allDots[i, j + 1].tag == allDots[i, j].tag && allDots[i, j + 2].tag == allDots[i, j].tag) {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 交换并检查是否有可消除元素后还原交换
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public bool SwitchAndCheck(int column, int row, Vector2 direction) {
        SwitchPiece(column, row, direction);
        if (CheckForMatches()) {
            SwitchPiece(column, row, direction);
            return true;
        }
        SwitchPiece(column, row, direction);
        return false;
    }
    /// <summary>
    /// 检查是否死锁
    /// </summary>
    /// <returns></returns>
    private bool IsDeadlocked() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    if (i < width - 1) {
                        if (SwitchAndCheck(i, j, Vector2.right)) {
                            return false;
                        }

                    }
                    if (j < height - 1) {
                        if (SwitchAndCheck(i, j, Vector2.up)) {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    /// <summary>
    /// 重新洗牌
    /// </summary>
    private IEnumerator ShuffleBoard() {

        yield return new WaitForSeconds(0.5f);

        List<GameObject> newBoard = new List<GameObject>();
        // 保存每一个元素点
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    newBoard.Add(allDots[i, j]);
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        // 重新随机设置元素点
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (!blankSpaces[i, j]) {
                    int pieceToUse = Random.Range(0, newBoard.Count);

                    int maxIterations = 0;  // 防止样式太少时，无限循环

                    // 检查不能有可消除dots
                    
                    while (MatchesAt(i, j, newBoard[pieceToUse]) && maxIterations < 100) {
                        pieceToUse = Random.Range(0, newBoard.Count);
                        maxIterations++;
                    }
                    Dot piece = newBoard[pieceToUse].GetComponent<Dot>();
                    //maxIterations = 0;
                    piece.column = i;
                    piece.row = j;
                    allDots[i, j] = newBoard[pieceToUse];
                    newBoard.Remove(newBoard[pieceToUse]);

                }
            }
        }

        // 检查新板是不是死锁
        if (IsDeadlocked()) {
            //递归调用重新洗板
            StartCoroutine(ShuffleBoard());
        }
    }
}
