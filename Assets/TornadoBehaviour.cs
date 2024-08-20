using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoBehaviour : MonoBehaviour
{
    public float attackRadius = 1f;
    public string attackTagFilter = string.Empty;
    public ContactFilter2D attackFilter;
    public Transform attackOrigin = null;
    public Blackboard blackboard = null;

    float walkingSpeed = 0f;
    bool isAttacking = false;
    int contactCount;
    List<Collider2D> contacts = new List<Collider2D>();

    private void Awake()
    {
        walkingSpeed = blackboard.moveSpeedTarget;
        contacts = new List<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (isAttacking || blackboard.health <= 0) return;
        contactCount = Physics2D.OverlapCircle(attackOrigin.position, attackRadius, attackFilter, contacts);
        for (int i = 0; i < contactCount; i++)
        {
            Collider2D hit = contacts[i];
            if (hit == null || !hit.gameObject.CompareTag(attackTagFilter)) continue;
            isAttacking = true;
            blackboard.animator.SetTrigger("Attack");
            break;
        }
    }

    public void Attack()
    {
        contactCount = Physics2D.OverlapCircle(attackOrigin.position, attackRadius, attackFilter, contacts);

        for (int i = 0; i < contactCount; i++)
        {
            Collider2D hit = contacts[i];
            if (hit == null || !hit.gameObject.CompareTag(attackTagFilter)) continue;
            if (!hit.transform.root.TryGetComponent(out Blackboard _blackboardHit)) continue;
            _blackboardHit.health -= 1;
            _blackboardHit.body.velocity *= 0.75f;
            _blackboardHit.body.velocity += 4 * (Vector2)(blackboard.trans.position - _blackboardHit.trans.position).normalized;
        }
    }

    public void CanMove()
    {
        blackboard.canMove = true;
    }
    public void CannotMove()
    {
        blackboard.canMove = false;
    }
    public void IsNotAttacking()
    {
        isAttacking = false;
    }
    public void IsAttacking()
    {
        isAttacking = true;
    }

    public void SetAttackSpeed()
    {
        blackboard.moveSpeedTarget = walkingSpeed * 1.75f;
    }

    public void SetWalkingSpeed()
    {
        blackboard.moveSpeedTarget = walkingSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < contactCount; i++)
        {
            Collider2D hit = contacts[i];
            if (hit == null || !hit.gameObject.CompareTag(attackTagFilter)) continue;
            Gizmos.color = Color.green;
            break;
        }

        Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
    }
}