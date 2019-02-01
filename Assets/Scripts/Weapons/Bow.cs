using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

    public Transform ArrowSpawn;
    public Rigidbody Arrow;
    public float ArrowSpeed;
    private PlayerShowWeapon item;
    // Start is called before the first frame update
    void Start()
    {

        item = gameObject.GetComponent<PlayerShowWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        //check if item1 (the bow) is active otherwise it wont shoot.
        if(Input.GetButtonDown("Fire1") && item.item1.activeSelf == true)
        {
            Rigidbody arrowRigidbody;
            arrowRigidbody = Instantiate(Arrow, ArrowSpawn.position, ArrowSpawn.rotation) as Rigidbody;
            arrowRigidbody.AddForce(ArrowSpawn.forward * ArrowSpeed);
        }
    }
}
