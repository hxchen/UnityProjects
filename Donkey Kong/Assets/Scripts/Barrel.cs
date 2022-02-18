using UnityEngine;

public class Barrel : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    public float speed = 1f;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rigidbody2D.AddForce(collision.transform.right * speed, ForceMode2D.Impulse);
        }

    }
}
