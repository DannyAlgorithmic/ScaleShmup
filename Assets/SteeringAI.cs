using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAI : MonoBehaviour
{
    [Space(10), Header("Obstacle Avoidence")]
    public AnimationCurve avoidProximityMultiplier;
    public AnimationCurve seekProximityMultiplier;

    public float avoidenceDistanceStart;
    public float seekingDistanceStart;
    public float avoidenceDistanceLength;
    public float seekingDistanceLength;

    public ContactFilter2D avoidenceContactFilter;
    public ContactFilter2D seekingContactFilter;

    public RaycastHit2D[] avoidenceHits;
    public RaycastHit2D[] seekingHits;

    [Space(5)]
    public int rayCount = 24;
    public Vector2[] raycastDirections;
    public float[] unfavorableDirections;
    public float[] favorableDirections;

    public Blackboard blackboard = null;

    private Transform trans = null;

    public void Detection(AnimationCurve _distanceCurve, RaycastHit2D[] _hits, ContactFilter2D _filter, float[] _favorDirections, float _startDist, float _maxDistance)
    {
        for (int i = 0; i < rayCount; i++)
        {
            Vector2 direction   = raycastDirections[i];
            Vector2 startPos    = (Vector2)trans.position + (direction * _startDist);
            int hitCount        = Physics2D.Raycast(startPos, direction, _filter, _hits, _maxDistance);

            if (hitCount <= 0 | !blackboard.canMove | !Mathf.Approximately(blackboard.moveCooldown, 0f))
                _favorDirections[i]     = 0f;
            else
            {
                RaycastHit2D rayHit     = _hits[0];
                float currentDistance   = Mathf.Abs(rayHit.distance);
                float t                 = Mathf.Approximately(currentDistance, 0f) ? 1f : currentDistance / _maxDistance;
                _favorDirections[i]     = _distanceCurve.Evaluate(Mathf.Clamp01(t));
            }
        }
    }

    public Vector2 Computation()
    {
        Vector2 sumedDirections = Vector2.zero;
        for (int i = 0; i < rayCount; i++)
        {
            Vector2 direction           = raycastDirections[i];

            float directionProximity    = Vector2.Dot(blackboard.moveDirectionInput, direction);

            // float avoidMultiplier       = avoidProximityMultiplier.Evaluate(directionProximity);
            // float seekMultiplier        = seekProximityMultiplier.Evaluate(directionProximity);

            float avoidanceValue        = unfavorableDirections[i]  * ((directionProximity + 1f) * 0.5f);
            float seekValue             = favorableDirections[i]    * ((directionProximity + 1f) * 0.5f);
            sumedDirections             += (raycastDirections[i]    * (seekValue - avoidanceValue));
        }
        return sumedDirections.normalized / rayCount;
    }

    private void FixedUpdate()
    {
        // Avoidence & Seeking
        Detection(avoidProximityMultiplier, avoidenceHits, avoidenceContactFilter, unfavorableDirections, avoidenceDistanceStart, avoidenceDistanceLength);
        Detection(seekProximityMultiplier, seekingHits, seekingContactFilter, favorableDirections, seekingDistanceStart, seekingDistanceLength);
        Vector2 computedDirection = Computation();
        blackboard.moveDirectionTarget = computedDirection;
        if(computedDirection.magnitude > 0f) Debug.Log(computedDirection);
    }

    private void Awake()
    {
        trans                   = transform;

        raycastDirections     = new Vector2[rayCount];
        favorableDirections     = new float[rayCount];
        unfavorableDirections   = new float[rayCount];

        float rotationDegree    = 360f / (float)rayCount;
        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = (i * -rotationDegree) + 90f;

            float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad);
            float y = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
            raycastDirections[i] = new Vector2(x, y).normalized;
        }
        avoidenceHits   = new RaycastHit2D[1];
        seekingHits     = new RaycastHit2D[1];
    }


    private void OnDrawGizmos()
    {
        int rayCount = raycastDirections.Length;
        float rotationDegree = 360f / (float)rayCount;
        for (int i = 0; i < rayCount; i++)
        {
            Vector2 startPos = (Vector2)trans.position + (raycastDirections[i] * seekingDistanceStart);

            Gizmos.color = Color.green;
            Gizmos.DrawRay(startPos, raycastDirections[i] * (seekingDistanceLength * (1f - favorableDirections[i])));

            startPos = (Vector2)trans.position + (raycastDirections[i] * avoidenceDistanceStart);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(startPos, raycastDirections[i] * (avoidenceDistanceLength * (1f - unfavorableDirections[i])));
        }

    }
}