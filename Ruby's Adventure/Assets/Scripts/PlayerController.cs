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

    [SerializeField]
    private int maxBulletCount = 99;//最大子弹数量
    private int currentBulletCount;//当前子弹数量

    public int MyCurrentBulletCount { get { return currentBulletCount; } }
    public int MyMaxBulletCount { get { return maxBulletCount; } }

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
        currentBulletCount = 20;
        invincibleTimer = 0;
        rigidBody = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        UIManager.instance.UpdatHealthBar(currentHealth, maxHealth);
        UIManager.instance.UpdateBulletCount(currentBulletCount, maxBulletCount);
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

        ///按下F键 发射
        if (Input.GetKeyDown(KeyCode.F) && currentBulletCount > 0)
        {
            ChangeBulletCount(-1);
            animator.SetTrigger("Launch");
            AudioManager.instance.AudioPlay(launchClip);
            GameObject bullet = Instantiate(bulletPrefeb, rigidBody.position + Vector2.up * 0.5f, Quaternion.identity);
            BulletController bc = bullet.GetComponent<BulletController>();
            if (bc != null)
            {
                bc.Move(lookDirection, 300);
            }
        }
        ///按下D键交互
        if (Input.GetKeyDown(KeyCode.G))
        {
            // 对话射线投射
            RaycastHit2D hit = Physics2D.Raycast(rigidBody.position, lookDirection, 2f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                Debug.Log("射线投射碰到了NPC");

                NPCManager npc = hit.collider.GetComponent<NPCManager>();
                if (npc != null)
                {
                    npc.showDialog();
                }

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
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        UIManager.instance.UpdatHealthBar(currentHealth, maxHealth);
    }
    /// <summary>
    /// 增加子弹数量
    /// </summary>
    /// <param name="amout"></param>
    public void ChangeBulletCount(int amout)
    {
        currentBulletCount = Mathf.Clamp(currentBulletCount + amout, 0, maxBulletCount);
        UIManager.instance.UpdateBulletCount(currentBulletCount, maxBulletCount);

        int result = Mathf.Clamp(2 - 1, 0, 99);
        Debug.Log("result = " + result);
    }

    

}
