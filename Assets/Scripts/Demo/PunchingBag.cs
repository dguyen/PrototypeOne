using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PunchingBag : MonoBehaviour, IDamagable
{
    public GameObject FloatingTextPrefab;

    public void TakeDamage(int damage) {
        var tmp = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        tmp.GetComponent<TextMesh>().text = damage.ToString();
    }
}
