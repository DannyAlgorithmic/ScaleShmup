using UnityEngine;

public class HandBulletAnim : MonoBehaviour
{
    private Animator animator;
    private ShootSystem shootSystem;

    private string[] animationTriggers = { "Anim1", "Anim2", "Anim3", "Anim4" };
    private string resetTrigger = "Nada";

    void Start()
    {
        animator = GetComponent<Animator>();
        shootSystem = FindObjectOfType<ShootSystem>(); // Encuentra el ShootSystem en la escena
    }

    void Update()
    {
        // Detecta si se presiona una tecla numérica del 1 al 4
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayAnimation(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayAnimation(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayAnimation(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayAnimation(4);
        }

        // Detecta si se presiona la tecla 'R'
        if (Input.GetKeyDown(KeyCode.R))
        {
            ActivateResetTrigger();
        }
    }

    void PlayAnimation(int animNumber)
    {
        // Resetea cualquier animación que pudiera estar en ejecución
        ResetAllAnimationTriggers();

        // Activa el trigger de la animación correspondiente si hay balas disponibles
        if (animNumber >= 1 && animNumber <= 4 && shootSystem.HasAmmoForType(animNumber))
        {
            animator.SetTrigger(animationTriggers[animNumber - 1]);
        }
    }

    void ResetAllAnimationTriggers()
    {
        // Resetea todos los triggers para detener cualquier animación
        foreach (string trigger in animationTriggers)
        {
            animator.ResetTrigger(trigger);
        }
    }

    void ActivateResetTrigger()
    {
        // Resetea todos los triggers de animación
        ResetAllAnimationTriggers();

        // Activa el trigger "Nada" para cambiar a ese estado
        animator.SetTrigger(resetTrigger);
    }
}
