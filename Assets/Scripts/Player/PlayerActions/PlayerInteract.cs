﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour, IAction {
    public float interactRadius = 2f;
    public KeyCode interactKey = KeyCode.F;

    private IInteractable tmpInteractable;

    void Update() {
        if (Input.GetKeyUp(interactKey)) {
            Act();
        }
    }

    public bool CanDo() {
        Collider[] colliders = Physics.OverlapSphere (transform.position, interactRadius);
        IInteractable closestInteractable = null;
        float minSqrDistance = Mathf.Infinity;

        for(int i = 0; i < colliders.Length; i++) {
            IInteractable foundInteractable = colliders[i].GetComponent<IInteractable>();
            if (foundInteractable != null) {
                float sqrDistanceToCenter = (transform.position - colliders[i].transform.position).sqrMagnitude;
                if (sqrDistanceToCenter < minSqrDistance)
                {
                    minSqrDistance = sqrDistanceToCenter;
                    closestInteractable = foundInteractable;
                }
            }
        }
        tmpInteractable = closestInteractable;
        return tmpInteractable != null;
    }

    public void Act() {
        if (CanDo() && tmpInteractable != null) {
            tmpInteractable.Interact(gameObject);
            tmpInteractable = null;
        }
    }
}