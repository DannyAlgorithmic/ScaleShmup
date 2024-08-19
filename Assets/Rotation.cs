using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Blackboard blackboard = null;
    private Camera cam = null;

    private void OnEnable()
    {
        cam = Camera.main;    
    }

    private void Update()
    {
        Vector2 origin = transform.position;
        Vector2 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        blackboard.aimDirectionInput = (mouse - origin).normalized;
    }

    private void FixedUpdate()
    {
        if (!blackboard.canRotation | !Mathf.Approximately(blackboard.rotationCooldown, 0f)) return;

    }
}