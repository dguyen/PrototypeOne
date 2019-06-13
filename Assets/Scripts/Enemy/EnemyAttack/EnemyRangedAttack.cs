using UnityEngine;

public class EnemyRangedAttack : EnemyAttack {
    public GameObject Projectible;
    public bool AttackFollowTarget = true;
    public Transform AttackSpawn;
    public float AttackSpeed = 5;

    public override void Attack() {
        GameObject ClosestPlayer = GetClosestPlayer();
        if (PlayerInRange(ClosestPlayer)) {
            GameObject Attack = Instantiate(Projectible, AttackSpawn.position, AttackSpawn.rotation);
            Projectile EnemyProjectile = Attack.GetComponent<Projectile>();
            if (AttackFollowTarget) {
                EnemyProjectile.Fire(AttackSpeed, ClosestPlayer);
            } else {
                EnemyProjectile.Fire(AttackSpeed * 50);
            }
        }
    }
}
