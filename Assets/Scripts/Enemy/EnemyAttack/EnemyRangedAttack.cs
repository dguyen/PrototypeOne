using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : EnemyAttack {
    public GameObject Projectible;
    public Transform AttackSpawn;

    public override void Attack() {
        GameObject ClosestPlayer = GetClosestPlayer();
        if (PlayerInRange(ClosestPlayer)) {
            GameObject Attack = Instantiate(Projectible, AttackSpawn.position, AttackSpawn.rotation);
            Projectile EnemyProjectile = Attack.GetComponent<Projectile>();
            EnemyProjectile.Target = ClosestPlayer;
            EnemyProjectile.Fire();
        }
    }
}
