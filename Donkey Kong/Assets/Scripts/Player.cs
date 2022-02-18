using UnityEngine;

public class Player : MonoBehaviour
{
    // 动画相关
    private SpriteRenderer spriteRenderer;

    public Sprite[] runSprites;
     
    public Sprite climbSprite;

    private int spriteIndex;

    //碰撞相关
    private new Rigidbody2D rigidbody;

    private new Collider2D collider;

    private Collider2D[] results;

    private bool grounded;

    private bool climing;

    private Vector2 direction;

    public float moveSpeed = 1f;

    public float jumpStrength = 1f;
     
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4]; 

    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f / 12f, 1f / 12f); 
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(AnimateSprite));
    }
    /// <summary>
    /// 碰撞检查
    /// </summary>
    private void CheckCollision()
    {
        grounded = false;
        climing = false;
        Vector2 size = collider.bounds.size;
        size.y += 0.1f;
        size.x /= 2f;

        // 获取位于指定盒形区域内的所有碰撞体的列表。
        int amout = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, results);
        for (int i = 0; i < amout; i++)
        {
            GameObject hit = results[i].gameObject;
            if (hit.layer == LayerMask.NameToLayer("Ground"))
            {
                grounded = hit.transform.position.y < (transform.position.y - 0.5f);
                // 不在地面上时忽略碰撞
                Physics2D.IgnoreCollision(collider, results[i], !grounded);
            } else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                climing = true;
            }
        }
    }

    private void Update()
    {
        CheckCollision();

        if (climing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed;

        } else if (Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpStrength;
        } else
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }

        /// Edit -> Project Settings -> Input Manager
        direction.x = Input.GetAxis("Horizontal") * moveSpeed;
        if (grounded)
        {
            direction.y = Mathf.Max(direction.y, -1f);
        }
        

        if (direction.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (direction.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180f, 0f);
        }


    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime);
    }
    /// <summary>
    /// 动画设置
    /// </summary>
    private void AnimateSprite()
    {
        if (climing)
        {
            spriteRenderer.sprite = climbSprite;
        }
        else if(direction.x != 0f)
        {
            spriteIndex++;
            if (spriteIndex >= runSprites.Length)
            {
                spriteIndex = 0;
            }
            spriteRenderer.sprite = runSprites[spriteIndex];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Objective"))
        {
            enabled = false;
            FindObjectOfType<GameManager>().LevelComplete();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            enabled = false;
            FindObjectOfType<GameManager>().LevelFailed();
        }
    }

}
