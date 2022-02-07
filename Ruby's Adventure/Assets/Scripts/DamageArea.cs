using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///
/// 伤害陷阱相关
/// </summary>
public class DamageArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController pc = collision.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.ChangeHealth(-1);
        }
    }
}
