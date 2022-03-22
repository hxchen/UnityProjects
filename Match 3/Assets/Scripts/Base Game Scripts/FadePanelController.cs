using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanelController : MonoBehaviour
{

    public Animator panelAnim;
    public Animator gameInfoAnim;


    /// <summary>
    /// 开始按钮
    /// </summary>
    public void OK() {
        if (panelAnim != null && gameInfoAnim != null) {
            panelAnim.SetBool("Out", true);
            gameInfoAnim.SetBool("Out", true);
            StartCoroutine(GameStartCo());
        }
    }
    /// <summary>
    /// 游戏结束
    /// </summary>
    public void GameOver() {
        panelAnim.SetBool("Out", false);
        panelAnim.SetBool("Game Over", true);
    }


    /// <summary>
    /// 延迟1秒设置移动状态
    /// </summary>
    /// <returns></returns>
    IEnumerator GameStartCo() {
        yield return new WaitForSeconds(1f);
        Board board = FindObjectOfType<Board>();
        board.currentState = GameState.move;
    }
}
