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

    public GameObject bulletPrefeb;//子弹
    //=====玩家的音效
    public AudioClip hitClip;
    public AudioClip launchClip;


    // 玩家朝向
    private Vector2 lookDirection = new Vector2(1, 0);// 默认朝向右方
    Animator animator;


    Rigidbody2D rigidBody;// 刚体组件

    float horizontal;
    float vertical;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = 5;
        invincibleTimer = 0;
        rigidBody = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        Debug.Log("UIManager instace = " + UIManager.instance);
        UIManager.instance.UpdatHealthBar(currentHealth, maxHealth);
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
        Vector2 moveVector = new Vector2(horizontal, vertical);
        if (!Mathf.Approximately(moveVector.x, 0.0f) || !Mathf.Approximately(moveVector.y, 0.0f)) {
            lookDirection = moveVector;
            lookDirection.Normalize();
        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", moveVector.magnitude);

        Vector2 position = rigidBody.position;
        position.x += horizontal * speed * Time.deltaTime;
        position.y += vertical * speed * Time.deltaTime;
        rigidBody.MovePosition(position);

        ///按下J键 发射
        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger("Launch");
            AudioManager.instance.AudioPlay(launchClip);
            GameObject bullet = Instantiate(bulletPrefeb, rigidBody.position + Vector2.up * 0.5f, Quaternion.identity);
            BulletController bc = bullet.GetComponent<BulletController>();
            if (bc != null)
            {
                bc.Move(lookDirection, 300);
            }
        }

    }
    /// <summary>
    /// 改变生命值
    /// </summary>
    /// <param name="value"></param>
    public void ChangeHealth(int value)
    {
        if (value < 0)
        {
            animator.SetTrigger("Hit");
            AudioManager.instance.AudioPlay(hitClip);
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            invincibleTimer = invincibleTime;
        }
        Debug.Log(currentHealth + "/" + maxHealth);
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        UIManager.instance.UpdatHealthBar(currentHealth, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }

    

}
