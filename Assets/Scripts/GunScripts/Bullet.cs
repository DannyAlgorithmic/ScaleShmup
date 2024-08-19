using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    float speed = 15f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void shoot(Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Background")
        {
            Destroy(gameObject);
        }
    }
}