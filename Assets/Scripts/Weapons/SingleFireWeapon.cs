using UnityEngine;

public class SingleFireWeapon : RangedWeapon {
    public Transform gunPoint;
    public ParticleSystem gunParticles;
    public Projectile projectile;
    public float projectileSpeed = 1000;

    private float timer;

    public override void Update() {
        base.Update();
        timer += Time.deltaTime;
    }

    public override void WeaponActive() {
        if (Input.GetButtonDown("Fire1_P" + playerControlScheme) && CanFire()) {
            Fire();
        }
    }

    public bool CanFire() {
        return timer >= attackDelay;
    }

    public void Fire() {
        timer = 0f;

        gunParticles.Stop();
        gunParticles.Play();

        Projectile newProjectile = Instantiate(projectile, gunPoint.transform.position, gunPoint.rotation) as Projectile;
        newProjectile.PlayerMoney = playerMoney;
        newProjectile.MoneyPerHit = pointsPerHit;
        newProjectile.Fire(projectileSpeed);
        DecreaseAmmo(1);
    }
}
