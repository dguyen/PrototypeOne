using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int perArrowDamage = 10;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
    }

    private void OnCollisionEnter()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamagable[] damagables = other.GetComponents<IDamagable>();
        foreach (IDamagable damagable in damagables)
        {
            damagable.TakeDamage(perArrowDamage);
        }
        Destroy(gameObject);
    }

}


