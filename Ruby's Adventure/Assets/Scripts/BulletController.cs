using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 控制子弹的移动、碰撞
/// </summary>
public class BulletController : MonoBehaviour
{
    Rigidbody2D rbody;

    public AudioClip hitClip;

    // Start is called before the first frame update
    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 1f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector2 moveDirection, float moveForce)
    {
        rbody.AddForce(moveDirection * moveForce);
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        EnemyController ec = other.gameObject.GetComponent<EnemyController>();
        if (ec != null)
        {
            Debug.Log("碰到了敌人");
            ec.Fixed();
        }
        AudioManager.instance.AudioPlay(hitClip);
        //我们还增加了调试日志来了解飞弹触碰到的对象
        Debug.Log("Projectile Collision with " + other.gameObject);
        Destroy(gameObject);
    }

}
