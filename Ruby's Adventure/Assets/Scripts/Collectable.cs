using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 草莓被玩家碰撞时检测的相关类
/// </summary>
public class Collectable : MonoBehaviour
{
    public ParticleSystem collectEffect;
    //拾取音效
    public AudioClip collectClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 碰撞检测相关
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController pc = collision.GetComponent<PlayerController>();
        if (pc != null)
        {
            Debug.Log("玩家碰到了草莓");
            if (pc.health < pc.maxHealth)
            {
                pc.ChangeHealth(1);

                //添加特效
                Instantiate(collectEffect, transform.position, Quaternion.identity);
                //音效
                AudioManager.instance.AudioPlay(collectClip);
                Destroy(this.gameObject);
            }
                
        }
    }
}
