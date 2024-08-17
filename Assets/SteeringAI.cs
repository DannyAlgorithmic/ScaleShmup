using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAI : MonoBehaviour
{
    [Space(10), Header("Obstacle Avoidence")]

    public float avoidenceDistanceStart;
    public float seekingDistanceStart;
    public float avoidenceDistanceLength;
    public float seekingDistanceLength;

    public ContactFilter2D avoidenceContactFilter;
    public ContactFilter2D seekingContactFilter;

    public RaycastHit2D[] avoidenceHits;
    public RaycastHit2D[] seekingHits;

    [Space(5)]
    public Vector2[] avoidanceDirections;
    public float[] unfavorableDirections;
    public float[] favorableDirections;

    public Blackboard blackboard = null;

    private Transform trans = null;

    public void Detection(RaycastHit2D[] _hits, ContactFilter2D _filter, float[] _favorDirections, float _startDist, float _maxDistance)
    {
        int max = avoidanceDirections.Length;
        for (int i = 0; i < max; i++)
        {
            Vector2 direction = avoidanceDirections[i];
            Vector2 startPos = (Vector2)trans.position + (direction * _startDist);
            int hitCount = Physics2D.Raycast(startPos, direction, _filter, _hits, _maxDistance);

            if (hitCount <= 0 | !blackboard.canMove | !Mathf.Approximately(blackboard.moveCooldown, 0f))
                _favorDirections[i] = 0f;
            else
            {
                RaycastHit2D rayHit = _hits[0];
                float currentDistance = Mathf.Abs(rayHit.distance);
                float t = Mathf.Approximately(currentDistance, 0f) ? 1f : currentDistance / _maxDistance;
                _favorDirections[i] = Mathf.Lerp(1f, 0f, t);
            }
        }
    }

    private void FixedUpdate()
    {
        // avoidence
        Detection(avoidenceHits, avoidenceContactFilter, unfavorableDirections, avoidenceDistanceStart, avoidenceDistanceLength);
        // seeking
        Detection(seekingHits, seekingContactFilter, favorableDirections, seekingDistanceStart, seekingDistanceLength);
    }

    private void Awake()
    {
        trans = transform;
        avoidanceDirections = new Vector2[8]
        {
            Vector2.up.normalized,              // North
            Vector2.one.normalized,             // North east
            Vector2.right.normalized,           // East
            new Vector2(1f, -1f).normalized,    // South east
            Vector2.down.normalized,            // South
            new Vector2(-1f, -1f).normalized,   // South west
            Vector2.left.normalized,            // West
            new Vector2(-1f, 1f).normalized     // North west
        };
        favorableDirections = new float[8];
        unfavorableDirections = new float[8];

        avoidenceHits = new RaycastHit2D[1];
        seekingHits = new RaycastHit2D[1];
    }
}