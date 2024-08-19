using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Blackboard : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeedCurrent;
    public float moveSpeedTarget;
    public float acceleration;
    public float decceleration;
    public float moveCooldown = 0f;
    public bool canMove = true;

    [Space(5)]
    public Vector2 moveDirectionCurrent;
    public Vector2 moveDirectionInput;
    public Vector2 moveDirectionTarget;

    [Space(5)]
    public List<Vector2> moveDirectionHistory;

    [Space(10), Header("Rigidbody Settings")]
    public float dragDuringMoveCooldown = 0f;
    public float dragWhenOutOfControl = 2.5f;

    [Space(15), Header("Navigation")]
    public NavMeshAgent agent = null;
    public NavMeshPath path = null;
    public bool pathValid = false;

    public Rigidbody2D body = null;
    public Transform trans = null;
    public Collider2D impactCollider = null;
    public Collider2D hitCollider = null;

    private void Awake()
    {
        trans = transform;
        path ??= new NavMeshPath();
        body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        path ??= new NavMeshPath();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        moveCooldown    = Mathf.Max(0f, moveCooldown    - Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Approximately(moveCooldown, 0f))
            moveCooldown = 2.3f;

        body.drag = !Mathf.Approximately(moveCooldown, 0f) ? 2.5f : 0f;
    }
}