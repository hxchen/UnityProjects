using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkins : MonoBehaviour
{
    public Sprite[] sprites;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start...");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DoChange();
        }
    }

    void DoChange()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = sprites[index++];
        if (index > sprites.Length - 1)
        {
            index = 0;
        }
    }
}
