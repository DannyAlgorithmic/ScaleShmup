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



    }
}