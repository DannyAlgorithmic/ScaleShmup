using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeedCurrent;
    public float moveSpeedTarget;
    public float acceleration;
    public float decceleration;
    public float moveCooldown = 0f;
    public bool canMove = true, conserveMomentum;

    [Space(5)]
    public Vector2 moveDirectionCurrent;
    public Vector2 moveDirectionInput;
    public Vector2 moveDirectionTarget;

    [Space(5)]
    public List<Vector2> moveDirectionHistory;

    [Space(10), Header("Rotation")]
    public float rotationSpeedCurrent;
    public float rotationSpeedTarget;
    public float rotationCooldown = 0f;
    public bool canRotation = true;

    [Space(5)]
    public Vector2 aimDirectionCurrent;
    public Vector2 aimDirectionInput;
    public Vector2 aimDirectionTarget;

    [Space(5)]
    public List<Vector2> aimDirectionHistory;

    [Space(10), Header("Rigidbody Settings")]
    public float dragDuringMoveCooldown = 0f;
    public float dragWhenOutOfControl = 2.5f;

    public Rigidbody2D body = null;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveCooldown    = Mathf.Max(0f, moveCooldown    - Time.deltaTime);
        rotationCooldown     = Mathf.Max(0f, rotationCooldown     - Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Approximately(moveCooldown, 0f))
            moveCooldown = 2.3f;

        body.drag = !Mathf.Approximately(moveCooldown, 0f) ? 2.5f : 0f;
    }
}