using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Blackboard : MonoBehaviour
{
    [Header("Resources")]
    public int health;
    public int energy;

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

    public Rigidbody2D body = null;
    public Transform trans = null;
    public Collider2D impactCollider = null;
    public Collider2D hitCollider = null;

    [Space(10), Header("Animation")]
    public Animator animator = null;

    [HideInInspector] public NavMeshPath path = null;
    [HideInInspector] public bool pathValid = false;
    private void Awake()
    {
        trans   = transform;
        path    = new NavMeshPath();
        body    = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveCooldown    = Mathf.Max(0f, moveCooldown    - Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Approximately(moveCooldown, 0f))
            moveCooldown = 2.3f;

        body.drag = !Mathf.Approximately(moveCooldown, 0f) ? 2.5f : 0f;
    }
}