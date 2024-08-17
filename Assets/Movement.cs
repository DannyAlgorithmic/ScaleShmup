using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float targetSpeed = 5f;
    Rigidbody2D body = null;
    Vector2 direction = Vector2.zero;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float xDir = Input.GetAxisRaw("Horizontal");
        float yDir = Input.GetAxisRaw("Vertical");
        direction = new Vector2(xDir, yDir);
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = direction.normalized * targetSpeed;

        float speedDiff = Mathf.Abs(targetVelocity.magnitude) - targetSpeed / (targetSpeed - 0f);
        float actualSpeed = speedDiff * targetSpeed;
        Debug.Log(speedDiff);
        body.AddForce(targetVelocity);
        if(Mathf.Abs(body.velocity.magnitude) >= targetSpeed)
            body.velocity = targetVelocity;
    }
}