using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人控制相关
/// </summary>
public class EnemyController : MonoBehaviour
{
    public float speed = 3;//移动速度

    public float changeDirectionTime = 2f;
    private float changeTimeer;

    public bool isVertical;//是否垂直方向移动

    private Vector2 moveDirection;//移动方向


    private Rigidbody2D rbody;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        moveDirection = isVertical ? Vector2.up : Vector2.right;

        changeTimeer = changeDirectionTime;
    }

    // Update is called once per frame
    void Update()
    {
        changeTimeer -= Time.deltaTime;
        if (changeTimeer < 0)
        {
            moveDirection *= -1;
            changeTimeer = changeDirectionTime;
        }
        Vector2 postion = rbody.position;
        postion.x += moveDirection.x * speed * Time.deltaTime;
        postion.y += moveDirection.y * speed * Time.deltaTime;
        rbody.MovePosition(postion);

        anim.SetFloat("MoveX", moveDirection.x);
        anim.SetFloat("MoveY", moveDirection.y);
    }

    /// <summary>
    /// 与玩家碰撞检测
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.ChangeHealth(-1);
        }
    }
}
