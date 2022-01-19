using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCtrl : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Sprite[] images;

    // Start is called before the first frame update
    void Start()
    {
        // 定时器
        InvokeRepeating("CreateMonster", 0.1f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateMonster()
    {
        float x = Random.Range(-2, 2);
        float y = 5;
        GameObject monster = Instantiate(monsterPrefab);
        monster.transform.position = new Vector3(x, y, 0);
        // 随机怪物图像
        int index = Random.Range(0, images.Length);
        SpriteRenderer renderer = monster.GetComponent<SpriteRenderer>();
        renderer.sprite = this.images[index];
        // 头像的大小设置为100px宽度
        Sprite sprite = this.images[index];
        float imgWidth = sprite.rect.width;
        float scale = 100 / imgWidth;
        monster.transform.localScale = new Vector3(scale, scale, scale);

    }
}
