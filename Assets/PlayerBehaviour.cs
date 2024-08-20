using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{

    public Blackboard blackboard = null;
    private void Update()
    {
        float xDir = Input.GetAxisRaw("Horizontal");
        float yDir = Input.GetAxisRaw("Vertical");
        blackboard.moveDirectionTarget = new Vector2(xDir, yDir);
    }

    private void LateUpdate()
    {
        if(blackboard.health <= 0)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}
