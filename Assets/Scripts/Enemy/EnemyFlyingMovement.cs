using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingMovement : EnemyMovement {
    public float StoppingDistance = 10f;

    public override void Move() {
        GameObject ClosestPlayer = GetClosestPlayer();
        if (ClosestPlayer != null) {
            if (InRange(ClosestPlayer)) {
                transform.LookAt(ClosestPlayer.transform);
                GetNav().ResetPath();
            } else {
                GetNav().SetDestination (ClosestPlayer.transform.position);
            }
        } else {
            GetNav().enabled = false;
        }
    }

    private bool InRange(GameObject other) {
        Vector3 DirectionToTarget = other.transform.position - transform.position;
        return DirectionToTarget.sqrMagnitude <= (StoppingDistance * StoppingDistance);
    }
}
