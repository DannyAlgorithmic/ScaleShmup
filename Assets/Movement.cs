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
        Vector2 targetSpeed = blackboard.moveDirectionTarget.normalized * blackboard.moveSpeedTarget;   //Calculate the direction we want to move in and our desired velocity
        Vector2 adjustedSpeed = Vector2.Lerp(body.velocity, targetSpeed, 1f);                           //We can reduce are control using Lerp() this smooths changes to are direction and speed

        float accelRate = Mathf.Abs(targetSpeed.magnitude) > 0.01f ? blackboard.acceleration : blackboard.decceleration;

        //Gets an acceleration value based on if we are accelerating (includes turning)
        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (blackboard.conserveMomentum && Mathf.Abs(body.velocity.magnitude) > Mathf.Abs(adjustedSpeed.magnitude) && Mathf.Sign(body.velocity.magnitude) == Mathf.Sign(adjustedSpeed.magnitude) && Mathf.Abs(adjustedSpeed.magnitude) > 0.01f)
        {
            //Prevent any deceleration from happening, or in other words conserve our current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        #endregion


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