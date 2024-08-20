using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public BulletType bulletType;               // Tipo de bala que este pickup proporcionar�
    public int ammoAmount = 10;                 // Cantidad de munici�n que a�adir� este pickup
    public GameObject selfGameObject = null;
    public Transform selfTransform = null;
    public Rigidbody2D selfRigidbody = null;
    public Collider2D selfCollider = null;


    private void Awake()
    {
        selfGameObject = gameObject;
        selfTransform = transform;
        selfRigidbody = GetComponent<Rigidbody2D>();
        selfCollider = GetComponent<Collider2D>();
    }
}
