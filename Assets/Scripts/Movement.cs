using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Blackboard blackboard = null;

    private void FixedUpdate()
    {
        if(!blackboard.canMove | !Mathf.Approximately(blackboard.moveCooldown, 0f)) return;

        Rigidbody2D body = blackboard.body;
        Vector2 targetSpeed = blackboard.moveDirectionTarget * blackboard.moveSpeedTarget;   //Calculate the direction we want to move in and our desired velocity
        Vector2 adjustedSpeed = Vector2.Lerp(body.velocity, targetSpeed, 1f);                           //We can reduce are control using Lerp() this smooths changes to are direction and speed

        float accelRate = Mathf.Abs(targetSpeed.magnitude) > 0.01f ? blackboard.acceleration : blackboard.decceleration;

        Vector2 speedDif = adjustedSpeed - body.velocity;               //Calculate difference between current velocity and desired velocity
        Vector2 movement = speedDif * accelRate;                        //Calculate force along x-axis to apply to the player
        body.AddForce(movement * body.mass, ForceMode2D.Force);         //Convert this to a vector and apply to rigidbody

        // For those interested here is what AddForce() will do
        /*
            RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime * speedDif * accelRate) / RB.mass, RB.velocity.y);
            Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
       */

        // Platform Origin Effect -- Jumping velocity bugged
        /*
            Collider2D platformCollider = Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer);
		    if (isGrounded && isGrounded.TryGetComponent(out Rigidbody2D platformBody))
			    body.velocity = Vector2.ClampMagnitude(body.velocity + (platformBody.velocity * 0.3f), runMaxSpeed);
        */
    }
}