using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    [Header("Target Detection")]
    public ContactFilter2D targetFilter;
    public Collider2D[] targets = null;

    [Space(15)]
    public Blackboard blackboard = null;

    private void Awake()
    {
        targets = new Collider2D[1];
    }

    private void FixedUpdate()
    {
        Vector2 agentPosition = blackboard.trans.position;
        int targetCount = Physics2D.OverlapCircle(agentPosition, 10f, targetFilter, targets); // Do elsewhere

        blackboard.pathValid = false;
        if (targetCount > 0)
        {
            Vector2 targetPosition = targets[0].transform.position;
            Vector2 nearestPoint = targetPosition + (targetPosition - targets[0].ClosestPoint(agentPosition)).normalized;
            blackboard.pathValid = NavMesh.CalculatePath(agentPosition, targetPosition, NavMesh.AllAreas, blackboard.path); // blackboard.agent.CalculatePath(targets[0].transform.agentPosition, blackboard.path);
        }

        if (blackboard.pathValid)
        {
            blackboard.moveDirectionInput = ((Vector2)blackboard.path.corners[1] - agentPosition).normalized;
        }
        else
        {
            blackboard.moveDirectionInput = Vector2.zero;
            Array.Clear(targets, 0, 1);
        }
    }
}