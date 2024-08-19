using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Blackboard blackboard = null;
    private void Update()
    {
        float xDir = Input.GetAxisRaw("Horizontal");
        float yDir = Input.GetAxisRaw("Vertical");
        blackboard.moveDirectionTarget = new Vector2(xDir, yDir);
    }
}
