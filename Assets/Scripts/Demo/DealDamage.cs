using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [Tooltip("How much damage to inflict")]
    public int damageAmount = 20;

    [Tooltip("The interval between damages")]
    public float damageInterval = 1;

    private Dictionary<Collider, IDamagable> activeEntities = new Dictionary<Collider, IDamagable>();

    void OnTriggerEnter(Collider other) {
        IDamagable[] damagableList = other.GetComponents<IDamagable>();
        foreach (IDamagable damagable in damagableList) {
            activeEntities.Add(other, damagable);
            StartCoroutine(DamageLoop(other));
        }
    }
    
    void OnTriggerExit(Collider other) {
        activeEntities.Remove(other);
    }

    IEnumerator DamageLoop(Collider other) {
        IDamagable toDamage;
        activeEntities.TryGetValue(other, out toDamage);
        if (toDamage != null) {
            yield return StartCoroutine(DoDamage(toDamage));
            yield return StartCoroutine(DamageLoop(other));
        }
    }

    IEnumerator DoDamage(IDamagable toDamage) {
        toDamage.TakeDamage(damageAmount);
        yield return new WaitForSeconds(damageInterval);
    }
}
