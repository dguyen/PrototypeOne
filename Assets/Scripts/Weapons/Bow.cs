using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Entity
{
    public Transform ArrowSpawn;
    public Rigidbody Arrow;
    public float ArrowSpeed;

    private Inventory inventory;

    void Start()
    {
        base.AddCapability(Capability.PICKABLE);
        base.AddCapability(Capability.DROPABLE);
        base.AddCapability(Capability.WEAPON);
        inventory = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        // Check if bow is being held otherwise it wont shoot.
        if(inventory.GetSelectedItem() == gameObject && Input.GetButtonDown("Fire1"))
        {
            Rigidbody arrowRigidbody;
            arrowRigidbody = Instantiate(Arrow, ArrowSpawn.position, ArrowSpawn.rotation) as Rigidbody;
            arrowRigidbody.AddForce(ArrowSpawn.forward * ArrowSpeed);
        }
    }
}
