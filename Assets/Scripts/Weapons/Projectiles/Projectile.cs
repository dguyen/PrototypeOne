using UnityEngine;

public class Projectile : MonoBehaviour {
    public ParticleSystem HitParticleSystem;
    public int Damage = 30;

    [HideInInspector]
    public int MoneyPerHit = 5;

    [HideInInspector]
    public PlayerMoney PlayerMoney;

    private float Speed;
    private GameObject Target;
    private bool Fired = false;
    private bool FollowTarget = false;
    private Rigidbody Rb;

    void Awake() {
        Rb = gameObject.GetComponent<Rigidbody>();
        Destroy(gameObject, 120);
    }

    void FixedUpdate() {
        if (Fired && FollowTarget) {
            ChaseTarget();
        }
    }

    /**
     * Move projectile per frame
     */
    public virtual void ChaseTarget() {
        Vector3 NewPos = Vector3.MoveTowards(gameObject.transform.position, Target.transform.position, Speed * Time.deltaTime);
        NewPos.y = transform.position.y;
        gameObject.transform.position = NewPos;
    }

    void OnCollisionEnter(Collision other) {
        if (PlayerMoney != null && other.gameObject.CompareTag("Player")) {
            return;
        }
        Hit(other.gameObject);
    }

    /**
     * Is called when projectile hits another collider
     */
    public virtual void Hit(GameObject other) {
        IDamagable Damagable = other.gameObject.GetComponent<IDamagable>();
        if (Damagable != null) {
            if (Damagable is EnemyHealth && PlayerMoney != null) {
                PlayerMoney.IncreaseMoney(MoneyPerHit);
            }
            Damagable.TakeDamage(Damage);
        }
        ReattachParticle(other);
        DestroyProjectile();
    }

    /**
     * Removes destroy particle system from gameobject and reattaches to another gameobject
     */
    public void ReattachParticle(GameObject other) {
        if (!HitParticleSystem) {
            return;
        } else if (other.gameObject.isStatic || other.gameObject.activeSelf) {
            HitParticleSystem.gameObject.transform.parent = null;
        } else {
            var emptyObject = new GameObject();
            emptyObject.transform.parent = other.transform;
            HitParticleSystem.gameObject.transform.parent = emptyObject.transform;
        }
    }

    /**
     * Destroy the projectile
     */
    public void DestroyProjectile() {
        if (HitParticleSystem != null) {
            HitParticleSystem.Play();
            Destroy(HitParticleSystem, 2);
        }
        gameObject.SetActive(false);
        Destroy(gameObject, 3);
    }

    /**
     * Fire the projectile at a target
     */
    public virtual void Fire(float speed, GameObject target) {
        if (Fired) {
            return;
        }
        Speed = speed;
        Target = target;
        FollowTarget = true;
        Fired = true;
    }

    /**
     * Fire the projectile in a straight line
     */
    public virtual void Fire(float speed) {
        if (Fired) {
            return;
        }
        Fired = true;
        Rb.AddForce(transform.forward * speed);
    }
}
