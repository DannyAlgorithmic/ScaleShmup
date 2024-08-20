using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberBehaviour : MonoBehaviour
{
    public float attackRadius = 1f;
    public string attackTagFilter = string.Empty;
    public ContactFilter2D attackFilter;
    public Transform attackOrigin = null;
    public Blackboard blackboard = null;

    int contactCount;
    List<Collider2D> contacts = new List<Collider2D>();

    private void Awake()
    {
        contacts = new List<Collider2D>();
    }

    private void FixedUpdate()
    {
        if(blackboard.health <= 0) return;
        contactCount = Physics2D.OverlapCircle(attackOrigin.position, attackRadius, attackFilter, contacts);
        for (int i = 0; i < contactCount; i++)
        {
            Collider2D hit = contacts[i];
            if (hit == null || !hit.gameObject.CompareTag(attackTagFilter)) continue;
            blackboard.animator.SetTrigger("Attack");
            blackboard.moveCooldown = float.PositiveInfinity;
            break;
        }
    }

    public void Attack()
    {
        contactCount = Physics2D.OverlapCircle(attackOrigin.position, attackRadius, attackFilter, contacts);

        for (int i = 0; i < contactCount; i++)
        {
            Collider2D hit = contacts[i];
            if(hit == null || !hit.gameObject.CompareTag(attackTagFilter)) continue;
            if(!hit.transform.root.TryGetComponent(out Blackboard _blackboardHit)) continue;
            _blackboardHit.moveCooldown = Mathf.Clamp(blackboard.moveCooldown + 0.3f, 0, 2f);
            _blackboardHit.moveCooldown = Mathf.Clamp(blackboard.moveCooldown + 0.3f, 0, 2f);
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject, 0.1f);
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