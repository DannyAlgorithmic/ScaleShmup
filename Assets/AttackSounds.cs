using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSounds : MonoBehaviour
{
    public AudioSource audioSource = null;

    public void PlayAttackSound() => audioSource.Play();
}