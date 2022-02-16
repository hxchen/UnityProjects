using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 子弹补给包
/// </summary>
public class BulletBag : MonoBehaviour
{
    public int bulletCount = 10;//包内含有子弹数量
    public ParticleSystem collectEffect;//拾取特效

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController pc = collision.GetComponent<PlayerController>();
        if (pc != null && pc.MyCurrentBulletCount < pc.MyMaxBulletCount)
        {
            pc.ChangeBulletCount(bulletCount);
            Instantiate(collectEffect, transform.position, Quaternion.identity);

            Destroy(this.gameObject);
        }
    }
}
