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

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit == null || hit.gameObject.CompareTag("Player") || !hit.transform.root.TryGetComponent(out Blackboard _blackboardHit)) return;
        _blackboardHit.health += 1;
        Destroy(gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Background")
        {
            Destroy(gameObject, 0.025f);
        }
    }
}