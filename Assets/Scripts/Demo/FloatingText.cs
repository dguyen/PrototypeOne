using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float DestroyTime = 2f;
    public Vector3 Offset = new Vector3(0, 2f, 0);
    public Vector3 RngIntensity = new Vector3(0.25f, 0, 0);

    void Start() {
        Destroy(gameObject, DestroyTime);
        transform.localPosition += Offset;
        transform.localPosition += new Vector3(
            Random.Range(-RngIntensity.x, +RngIntensity.x),
            Random.Range(-RngIntensity.y, +RngIntensity.y),
            Random.Range(-RngIntensity.z, +RngIntensity.z)  
        );
    }
}
