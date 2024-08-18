using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAnim : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 objectPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 direction = mousePosition - objectPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

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
