using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;

    Vector3 offset;
    bool targetSet;

    public void SetupCamera() {
        if (target == null) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        offset = transform.position - target.position;
        targetSet = true;
    }

    void FixedUpdate() {
        if (targetSet) {
            Vector3 targetCamPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}
