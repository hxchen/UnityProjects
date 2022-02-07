using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 控制角色移动、生命、动画等 
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;// 移动速度

    public int maxHealth = 5;//最大生命
    private int currentHealth;//当前生命值
    public int health { get { return currentHealth; } }


    private float invincibleTime = 2f;//无敌时间2秒
    private float invincibleTimer;//无敌计时器
    private bool isInvincible;//是否处于无敌状态

    Rigidbody2D rigidBody;// 刚体组件

    float horizontal;
    float vertical;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = 2;
        invincibleTimer = 0;

        rigidBody = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 position = rigidBody.position;
        position.x += horizontal * speed * Time.deltaTime;
        position.y += vertical * speed * Time.deltaTime;
        rigidBody.MovePosition(position);

    }
    /// <summary>
    /// 改变生命值
    /// </summary>
    /// <param name="value"></param>
    public void ChangeHealth(int value)
    {
        if (value < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            invincibleTimer = invincibleTime;
        }
        Debug.Log(currentHealth + "/" + maxHealth);
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }
}
