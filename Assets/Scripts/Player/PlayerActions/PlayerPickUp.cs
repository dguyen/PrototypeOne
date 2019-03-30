// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerPickUp : MonoBehaviour, IAction {
//     Inventory inventory;

//     void Start() {
//         inventory = FindObjectOfType<Inventory>();
//     }

//     public bool CanDo() {
//         return true;
//     }

//     public void Act() { }

//     void OnTriggerEnter(Collider other) {
//         IEntity[] entityList = other.GetComponents<IEntity>();
//         foreach (IEntity entity in entityList) {
//             if (entity.HasCapability(Capability.PICKABLE)) {
//                 inventory.AddItem(other.gameObject);
//             }
//         }
//     }
// }
