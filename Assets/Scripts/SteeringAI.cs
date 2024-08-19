using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SteeringAI : MonoBehaviour
{
    [Header("Curves")]
    public AnimationCurve avoidDistanceProximity;

    [Space(5)]
    public AnimationCurve avoidAngleProximity;

    [Space(15), Header("Size")]
    public float avoidenceDistanceStart;
    public float avoidenceDistanceLength;

    [Space(15), Header("Filters")]
    public ContactFilter2D avoidenceContactFilter;

    [Space(15), Header("Weights")]
    public int rayCount = 50;
    public Vector2[] raycastDirections;
    public float[] avoidedDirections;

    public Blackboard blackboard = null;

    private RaycastHit2D[] avoidenceHits;


    public void Detection(AnimationCurve _distanceCurve, RaycastHit2D[] _hits, ContactFilter2D _filter, float[] _favorDirections, float _startDist, float _maxDistance)
    {
        for (int i = 0; i < rayCount; i++)
        {
            Vector2 direction   = raycastDirections[i];
            Vector2 startPos    = (Vector2)blackboard.trans.position + (direction * _startDist);
            int hitCount        = Physics2D.Raycast(startPos, direction, _filter, _hits, _maxDistance);

            if (hitCount <= 0 | !blackboard.canMove | !Mathf.Approximately(blackboard.moveCooldown, 0f))
                _favorDirections[i]     = 0f;
            else
            {
                RaycastHit2D rayHit     = _hits[0];
                bool isSelf             = rayHit.collider == blackboard.impactCollider | rayHit.collider == blackboard.hitCollider;
                float currentDistance   = Mathf.Abs(rayHit.distance);
                float t                 = Mathf.Approximately(currentDistance, 0f) ? 1f : currentDistance / _maxDistance;
                _favorDirections[i]     = isSelf ? 0f : _distanceCurve.Evaluate(Mathf.Clamp01(t));
            }
        }
    }

    public Vector2 Computation()
    {
        Vector2 sumedDirections = Vector2.zero;
        Vector2 targetDirection = blackboard.moveDirectionInput;
        for (int i = 0; i < rayCount; i++)
        {
            Vector2 direction = raycastDirections[i];

            float directionProximity = Vector2.Dot(targetDirection, direction);

            float avoidMultiplier   = avoidAngleProximity.Evaluate(directionProximity);
            float avoidanceValue    = avoidedDirections[i] * ((avoidMultiplier + 1f) * 0.5f);
            sumedDirections         += raycastDirections[i] * avoidanceValue;
        }
        return (sumedDirections.normalized / rayCount).normalized;
    }


    private void FixedUpdate()
    {   
        // Avoidence & Seeking
        Detection(avoidDistanceProximity, avoidenceHits, avoidenceContactFilter, avoidedDirections, avoidenceDistanceStart, avoidenceDistanceLength);

        Vector2 originalDirection       = blackboard.moveDirectionInput;
        Vector2 computedDirection       = -Computation();
        Vector2 obstructedDirection     = (originalDirection + computedDirection.normalized) / 2f;
        float angleBetween              = Vector2.Angle(originalDirection, computedDirection);
        Vector2 targetDirection         = Mathf.Abs(angleBetween) <= 90f ? originalDirection : obstructedDirection;


        if (originalDirection.magnitude > 0.5f)
            Aim(Vector2.Lerp(blackboard.moveDirectionTarget, targetDirection, 1f * 10f * Time.fixedDeltaTime));
        else
            Aim(blackboard.moveDirectionTarget);

        blackboard.moveDirectionTarget = targetDirection;
    }

    private void Awake()
    {
        raycastDirections       = new Vector2[rayCount];
        avoidedDirections       = new float[rayCount];
        avoidenceHits           = new RaycastHit2D[1];

        float rotationDegree    = 360f / (float)rayCount;
        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = (i * -rotationDegree) + 90f;

            float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad);
            float y = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
            raycastDirections[i] = new Vector2(x, y).normalized;
        }
    }


    private void OnDrawGizmos()
    {
        int rayCount = raycastDirections.Length;
        float rotationDegree = 360f / (float)rayCount;

        Gizmos.color = Color.red;
        for (int i = 0; i < rayCount; i++)
        {
            Vector2 startPos = (Vector2)blackboard.trans.position + (raycastDirections[i] * avoidenceDistanceStart);
            Gizmos.DrawRay(startPos, raycastDirections[i] * (avoidenceDistanceLength * (1f - avoidedDirections[i])));
        }

    }

    public Animator animator;

    void Aim(Vector2 _direction)
    {
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        string animationName;

        if (angle > -22.5f && angle <= 22.5f) // East
        {
            animationName = "East";
        }
        else if (angle > 22.5f && angle <= 67.5f) // Northeast
        {
            animationName = "Northeast";
        }
        else if (angle > 67.5f && angle <= 112.5f) // North
        {
            animationName = "North";
        }
        else if (angle > 112.5f && angle <= 157.5f) // Northwest
        {
            animationName = "Northwest";
        }
        else if (angle > 157.5f || angle <= -157.5f) // West
        {
            animationName = "West";
        }
        else if (angle > -157.5f && angle <= -112.5f) // Southwest
        {
            animationName = "Southwest";
        }
        else if (angle > -112.5f && angle <= -67.5f) // South
        {
            animationName = "South";
        }
        else // Southeast
        {
            animationName = "Southeast";
        }

        animator.SetTrigger(animationName);
    }
}