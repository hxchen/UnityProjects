using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    wait,
    move,
    win,
    lose,
    pause
}

public enum TileKind {
    Breakable,
    Blank,
    Normal
}

[System.Serializable]
public class MatchType {
    public int type;
    public string color;
}

// 告诉编辑器序列化
[System.Serializable]
public class TileType {
    public int x;
    public int y;
    public TileKind tileKind;
}

public class Board : MonoBehaviour {

    [Header("Scriptable Object Stuff")]
    public World world;
    public int level;

    public GameState currentState = GameState.move;

    [Header("Board Dimensions")]
    public int width;
    public int height;
    public int offset;

    [Header("Prefabs")]
    public GameObject tilePrefab;
    public GameObject breakableTilePrefab;
    // 所有样式
    public GameObject[] dots;
    public GameObject destroyEffect;

    [Header("Layout")]
    // 特殊点
    public TileType[] boardLayout;
    private bool[,] blankSpaces;
    private BackgroundTile[,] breakableTiles;
    public GameObject[,] allDots;

    [Header("Match Stuff")]
    public MatchType matchType;
    public Dot currentDot;
    private FindMatches findMatches;
    //单点得分
    public int basePieceValue = 20;
    private int streakValue = 1;
    private ScoreManager scoreManager;
    private AudioManager audioManager;
    private GoalManager goalManager;
    public float refillDelay = 0.5f;
    public int[] scoreGoals;


    void Awake() {
        if (PlayerPrefs.HasKey("Current Level")) {
            level = PlayerPrefs.GetInt("Current Level");
        }
        if (world != null) {
            if (level < world.levels.Length) {
                if (world.levels[level] != null) {
                    width = world.levels[level].width;
                    height = world.levels[level].height;
                    dots = world.levels[level].dots;
                    scoreGoals = world.levels[level].scoreGoals;
                    boardLayout = world.levels[level].boardLayout;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
        goalManager = FindObjectOfType<GoalManager>();
        audioManager = FindObjectOfType<AudioManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        breakableTiles = new BackgroundTile[width, height];
        findMatches = FindObjectOfType<FindMatches>();
        blankSpaces = new bool[width, height];
        allDots = new GameObject[width, height];
        SetUp();
        currentState = GameState.pause;
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
   /// 生成炸弹类型
   /// </summary>
   /// <returns></returns>
    private MatchType ColumnOrRow() {
        // 复制当前匹配的元素
        List<GameObject> matchCopy = findMatches.currentMatches as List<GameObject>;

        matchType.type = 0;
        matchType.color = "";

        for (int i = 0; i < matchCopy.Count; i++) {
            Dot thisDot = matchCopy[i].GetComponent<Dot>();
            string color = matchCopy[i].tag;

            int column = thisDot.column;
            int row = thisDot.row;
            int columnMatch = 0;
            int rowMatch = 0;
            for (int j = 0; j < matchCopy.Count; j++) {
                Dot nextDot = matchCopy[j].GetComponent<Dot>();
                if (nextDot == thisDot) {
                    continue;
                }
                if (nextDot.column == thisDot.column && nextDot.tag == color) {
                    columnMatch++;
                }
                if (nextDot.row == thisDot.row && nextDot.tag == color) {
                    rowMatch++;
                }
            }
            // 行列炸弹返回 3
            // 相邻炸弹返回 2
            // 颜色炸弹返回 1
            if (columnMatch == 4 || rowMatch == 4) {
                matchType.type = 1;
                matchType.color = color;
                return matchType;
            }
            else if (columnMatch == 2 || rowMatch == 2) {
                matchType.type = 2;
                matchType.color = color;
                return matchType;
            }
            else if (columnMatch == 3 || rowMatch == 3) {
                matchType.type = 3;
                matchType.color = color;
                return matchType;
            }

        }
        matchType.type = 0;
        matchType.color = "";
        return matchType;

    }

    /// <summary>
    /// 生成不同类型炸弹
    /// </summary>
    private void CheckToMakeBombs() {
        // How many objects are in findMatches currentMatches
        if (findMatches.currentMatches.Count > 3) {
            // 炸弹类型
            MatchType typeOfMatch = ColumnOrRow();
            if (typeOfMatch.type == 1) {
                // 颜色炸弹
                if (currentDot != null && currentDot.isMatched && currentDot.tag == typeOfMatch.color) {
                    currentDot.isMatched = false;
                    currentDot.MakeColorBomb();
                } else if (currentDot.otherDot != null) {
                    Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                    if (otherDot.isMatched && otherDot.tag == typeOfMatch.color) {
                        otherDot.isMatched = false;
                        otherDot.MakeColorBomb();
                    }
                }
            } else if (typeOfMatch.type == 2) {
                // 邻近炸弹
                if (currentDot != null && currentDot.isMatched && currentDot.tag == typeOfMatch.color) {
                    currentDot.isMatched = false;
                    currentDot.MakeAdjacentBomb();
                } else if (currentDot.otherDot != null) {
                    Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                    if (otherDot.isMatched && otherDot.tag == typeOfMatch.color) {
                        otherDot.isMatched = false;
                        otherDot.MakeAdjacentBomb();
                    }
                }
            } else if (typeOfMatch.type == 3) {
                // 行列炸弹
                findMatches.CheckBombs(typeOfMatch);
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
            //if (findMatches.currentMatches.Count >= 4) {
            //    CheckToMakeBombs();
            //}

            // 检查瓦片是否易碎
            if (breakableTiles[column, row] != null) {
                breakableTiles[column, row].TakeDamage(1);
                if (breakableTiles[column, row].hitPoints <= 0) {
                    breakableTiles[column, row] = null;
                }
            }
            // 目标
            if (goalManager != null) {
                goalManager.CompareGoal(allDots[column, row].tag.ToString());
                goalManager.UpdateGoals();
            }
            //音效
            if (audioManager != null) {
                audioManager.PlayRandomDestroyNoise();
            }
            //特效
            GameObject particle = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            Destroy(particle, 0.5f);
            // 动画播放
            allDots[column, row].GetComponent<Dot>().PopAnimation();
            //销毁
            Destroy(allDots[column, row], 0.5f);
            scoreManager.IncreaseScore(basePieceValue * streakValue);
            allDots[column, row] = null;
        }
        
    }
    /// <summary>
    /// 消除board上的连点
    /// </summary>
    public void DestroyMatches() {
        //  检查匹配项里面有多少个元素点
        if (findMatches.currentMatches.Count >= 4) {
            CheckToMakeBombs();
        }
        findMatches.currentMatches.Clear();
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        
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
        findMatches.FindAllMatches();
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
        
        yield return new WaitForSeconds(refillDelay);
        RefillBoard();
        yield return new WaitForSeconds(refillDelay);
        while (MatchesOnBoard()) {
            //得分
            streakValue ++;
            DestroyMatches();
            yield break;
        }
        currentDot = null;
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
