using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    private Vector2 goal;

    [SerializeField]
    private Sprite normalSprite, hitSprite;

    private SpriteRenderer spriteRenderer;

    private float moveSpeed;

    [SerializeField]
    private float speed, speedWhenHit;


    [SerializeField]
    private float moveRange;


    void Awake() {
        moveSpeed = speed;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, goal, moveSpeed * Time.deltaTime);
        if ((Vector2)transform.position == goal) {
            goal = ChooseNewGoal();
        }
        
    }

    private Vector2 ChooseNewGoal() {
        Vector2 newGoal = Random.insideUnitCircle * moveRange;
        return newGoal;
    }

    public void StartLightingShock() {
        spriteRenderer.sprite = hitSprite;
        moveSpeed = speedWhenHit;
    }

    public void StopLightingShock() {
        spriteRenderer.sprite = normalSprite;
        moveSpeed = speed;
    }
}
