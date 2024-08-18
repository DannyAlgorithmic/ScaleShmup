using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDAnimations : MonoBehaviour
{
    private Vector2 movement;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Leer la entrada del jugador
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalizar el vector de movimiento para evitar que el movimiento diagonal sea más rápido
        movement.Normalize();

        // Actualizar la animación según la dirección del movimiento
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (movement != Vector2.zero)
        {
            if (movement.x > 0)
            {
                if (movement.y > 0)
                    animator.Play("MoveUpRight");
                else if (movement.y < 0)
                    animator.Play("MoveDownRight");
                else
                    animator.Play("MoveRight");
            }
            else if (movement.x < 0)
            {
                if (movement.y > 0)
                    animator.Play("MoveUpLeft");
                else if (movement.y < 0)
                    animator.Play("MoveDownLeft");
                else
                    animator.Play("MoveLeft");
            }
            else
            {
                if (movement.y > 0)
                    animator.Play("MoveUp");
                else if (movement.y < 0)
                    animator.Play("MoveDown");
            }
        }
        else
        {
            // Aquí puedes agregar una animación de "Idle" si el jugador no se está moviendo
            animator.Play("Idle");
        }
    }
}
