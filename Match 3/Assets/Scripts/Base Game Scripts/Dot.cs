using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour {
    [Header("Board Variables")]
    // 当前位置
    public int column;
    public int row;
    // 上一次位置
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;
    public GameObject otherDot;

    /// <summary>
    /// 私有变量
    /// </summary>
    ///
    // 动画
    private Animator animator;
    private float shineDelay;
    private float shineDelaySeconds;

    private EndGameManager endGameManager;
    private HintManager hintManager;
    private FindMatches findMatches;
    private Board board;
    private Vector2 firstTouchPosition = Vector2.zero;
    private Vector2 finalTouchPosition = Vector2.zero;
    private Vector2 tempPosition;

    [Header("Swipe Stuff")]
    public float swipeAngle = 0;
    public float swipResist = 0.1f;


    [Header("Powerup Stuff")]
    public bool isColorBomb; 
    public bool isColumnBomb;
    public bool isRowBomb;
    public bool isAdjacentBomb;

    public GameObject rowArrow;
    public GameObject columnArrow;
    public GameObject colorBomb;
    public GameObject adjacentMarker;

    private void Start() {


        isColumnBomb = false;
        isRowBomb = false;
        isColorBomb = false;
        isAdjacentBomb = false;

        // 动画
        shineDelay = Random.Range(3f, 6f);
        shineDelaySeconds = shineDelay;
        animator = GetComponent<Animator>();
        // 更快的方式
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
        // board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        hintManager = FindObjectOfType<HintManager>();
        endGameManager = FindObjectOfType<EndGameManager>();
        //targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
        //row = targetY;
        //column = targetX;
        //previousRow = row;
        //previousColumn = column;
    }


    /// <summary>
    /// 调试专用函数
    /// </summary>
    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(1)) {
            //isColumnBomb = true;
            //GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
            //arrow.transform.parent = this.transform;

            //isRowBomb = true;
            //GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
            //arrow.transform.parent = this.transform;

            isColorBomb = true;
            GameObject color = Instantiate(colorBomb, transform.position, Quaternion.identity);
            color.transform.parent = this.transform;

            //isAdjacentBomb = true;
            //GameObject bomb = Instantiate(adjacentMarker, transform.position, Quaternion.identity);
            //bomb.transform.parent = this.transform;
        }
    }


    private void Update() {

        shineDelaySeconds -= Time.deltaTime;
        if (shineDelaySeconds <= 0) {
            shineDelaySeconds = shineDelay;
            StartCoroutine(StartShineCo());
        }
        //FindMatches();
        //if (isMatched) {
        //    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        //    spriteRenderer.color = new Color(1, 1, 1, .2f);
        //}
        targetX = column;
        targetY = row;

        if (Mathf.Abs(targetX - transform.position.x) > .1) {
            // 朝目标移动
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject) {
                board.allDots[column, row] = this.gameObject;
                findMatches.FindAllMatches();
            }
        } else {
            // 直接设置
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1) {
            // 朝目标移动
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject) {
                board.allDots[column, row] = this.gameObject;
                findMatches.FindAllMatches();
            }
            
        } else {
            // 直接设置
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }

    }
    


    private void OnMouseDown() {
        // 动画
        //if (animator != null) {
        //    animator.SetBool("Touched", true);
        //}
        // 销毁提示
        if (hintManager != null) {
            hintManager.DestroyHint();
        }
        
        if (board.currentState == GameState.move) {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        
    }

    private void OnMouseUp() {

        //animator.SetBool("Touched", false);

        if(board.currentState == GameState.move) {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }
    /// <summary>
    /// 计算滑动夹角
    /// </summary>
    private void CalculateAngle() {

        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipResist) {
            board.currentState = GameState.wait;
            
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;

            MovePieces();
            board.currentDot = this;

        } else {
            board.currentState = GameState.move;
            
        }
         
    }
    /// <summary>
    /// 交换移动
    /// </summary>
    private void MovePieces() {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1) {
            MovePiecesActual(Vector2.right);
        } else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1) {
            // 上交换
            MovePiecesActual(Vector2.up);
        } else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0) {
            // 左交换
            MovePiecesActual(Vector2.left);
        } else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0) {
            // 下交换
            MovePiecesActual(Vector2.down);
        } else {
            // 划出边界重设状态
            board.currentState = GameState.move;
        }

    }
    private void MovePiecesActual(Vector2 direction) {
        otherDot = board.allDots[column + (int)direction.x, row + (int)direction.y];
        previousRow = row;
        previousColumn = column;
        if (board.lockTiles[column, row] == null && board.lockTiles[column + (int)direction.x, row + (int)direction.y] == null) {
            if (otherDot != null) {
                otherDot.GetComponent<Dot>().column -= 1 * (int)direction.x;
                otherDot.GetComponent<Dot>().row -= 1 * (int)direction.y;
                column += (int)direction.x;
                row += (int)direction.y;
                //协同程序
                StartCoroutine(CheckMoveCo());
            } else {
                board.currentState = GameState.move;
            }
        } else {
            board.currentState = GameState.move;
        }
    }
    /// <summary>
    /// Shine 动画
    /// </summary>
    IEnumerator StartShineCo() {
        animator.SetBool("Shine", true);
        yield return null;
        animator.SetBool("Shine", false);
    }
    /// <summary>
    /// Pop动画
    /// </summary>
    public void PopAnimation() {
        animator.SetBool("Popped", true);
    }

    /// <summary>
    /// 交换后不匹配，等待0.5秒后还原
    /// </summary>
    /// <returns></returns>
    public IEnumerator CheckMoveCo() {
        if (isColorBomb) {
            //  该元素是个颜色炸弹，另一块是待消除元素
            findMatches.MatchPiecesOfColor(otherDot.tag);
            isMatched = true;
        } else if (otherDot != null && otherDot.GetComponent<Dot>().isColorBomb) {
            // 另一块是颜色炸弹，这个是待消除元素
            findMatches.MatchPiecesOfColor(this.gameObject.tag);
            otherDot.GetComponent<Dot>().isMatched = true;
        }

        yield return new WaitForSeconds(0.5f);

        if (otherDot != null) {
            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched) {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;

                row = previousRow;
                column = previousColumn;

                yield return new WaitForSeconds(0.5f);
                board.currentDot = null;
                board.currentState = GameState.move;

            } else {
                if (endGameManager != null) {
                    if (endGameManager.requiremants.gameType == GameType.Moves) {
                        endGameManager.DecreaseCounterValue();
                    }
                }
                board.DestroyMatches();

            }
            //otherDot = null;
        }
    }
    /// <summary>
    /// 查找匹配并设置
    /// </summary>
    private void FindMatches() {
        if (column > 0 && column < board.width - 1) {
            GameObject leftDot1 = board.allDots[column - 1, row];
            GameObject rightDot1 = board.allDots[column + 1, row];
            if (leftDot1 != null && rightDot1 != null) {
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag) {
                    leftDot1.GetComponent<Dot>().isMatched = true;
                    rightDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
            
        }

        if (row > 0 && row < board.height - 1) {
            GameObject upDot1 = board.allDots[column, row + 1];
            GameObject downDot1 = board.allDots[column, row - 1];
            if (upDot1 != null && downDot1 != null) {
                if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag) {
                    upDot1.GetComponent<Dot>().isMatched = true;
                    downDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
            
        }
    }
    /// <summary>
    /// 制作行炸弹点
    /// </summary>
    public void MakeRowBomb() {
        if (!isColumnBomb && !isColorBomb && isAdjacentBomb) {
            isRowBomb = true;
            GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
            arrow.transform.parent = this.transform;
        }
        
    }
    /// <summary>
    /// 制作列炸弹点
    /// </summary>
    public void MakeColumnBomb() {
        if (!isRowBomb && !isColorBomb && !isAdjacentBomb) {
            isColumnBomb = true;
            GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
            arrow.transform.parent = this.transform;
        }
        
    }
    /// <summary>
    /// 同颜色炸弹
    /// </summary>
    public void MakeColorBomb() {
        if (!isRowBomb && !isColumnBomb && !isAdjacentBomb) {
            isColorBomb = true;
            GameObject color = Instantiate(colorBomb, transform.position, Quaternion.identity);
            color.transform.parent = this.transform;
            this.gameObject.tag = "Color";
        }
        
    }
    /// <summary>
    /// 相邻炸弹
    /// </summary>
    public void MakeAdjacentBomb() {
        if (!isRowBomb && !isColumnBomb && !isColorBomb) {
            isAdjacentBomb = true;
            GameObject bomb = Instantiate(adjacentMarker, transform.position, Quaternion.identity);
            Debug.Log("Adjacent Scale is :" + bomb.transform.localScale);
            bomb.transform.parent = this.transform;
        }
        
    }

}
