using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnrage : MonoBehaviour {
    public int EnrageAtHealth = 50;
    public float AtkMultiplier = 1f;
    public float AtkSpeedMultiplier = 1f;
    public float MovSpeedMultiplier = 1f;

    public ParticleSystem[] EnrageParticleSystem;

    private int OldDamage;
    private float OldAtkSpeed;
    private float OldMoveSpeed;

    private EnemyHealth EnemyHealth;
    private EnemyAttack EnemyAttack;
    private EnemyMovement EnemyMovement;
    private Animator EnemyAnimator;
    private bool Enraged = false;

    void Start() {
        EnemyAttack = GetComponent<EnemyAttack>();
        EnemyHealth = GetComponent<EnemyHealth>();
        EnemyMovement = GetComponent<EnemyMovement>();
        EnemyAnimator = GetComponent<Animator>();
    }

    void Update() {
        if (EnemyHealth.currentHealth <= EnrageAtHealth && !Enraged) {
            Enrage();
        }
    }

    /**
     * Enrage the enemy
     */
    public void Enrage() {
        if (Enraged) {
            return;
        } else if (EnemyAnimator != null) {
            EnemyAnimator.SetBool("Enraged", true);
        }
        UpdateParticles(true);
        Enraged = true;
        OldDamage = EnemyAttack.AttackDamage;
        OldAtkSpeed = EnemyAttack.AttackDelay;
        OldMoveSpeed = EnemyMovement.GetNav().speed;

        UpdateEnemyAtk(Mathf.RoundToInt(OldDamage*AtkMultiplier));
        UpdateEnemyAtkSpeed(OldAtkSpeed/AtkSpeedMultiplier);
        UpdateEnemyMovSpeed(OldMoveSpeed * MovSpeedMultiplier);
    }

    /**
     * Remove the rage buffs from enemy
     */
    public void Unrage() {
        if (Enraged) {
            UpdateEnemyAtk(OldDamage);
            UpdateEnemyAtkSpeed(OldAtkSpeed);
            UpdateEnemyMovSpeed(OldMoveSpeed);            
            UpdateParticles(false);
            Enraged = false;
        }
    }

    void UpdateParticles(bool enabled) {
        foreach (ParticleSystem pSystem in EnrageParticleSystem) {
            if (enabled) {
                pSystem.Play();
            } else {
                pSystem.Stop();
            }
        }
    }

    /**
     * Returns true if enemy is Enraged
     */
    public bool IsEnraged() {
        return Enraged; 
    }

    void UpdateEnemyAtk(int newDamage) {
        if (AtkMultiplier != 1f) {
            EnemyAttack.AttackDamage = newDamage;
        }
    }

    void UpdateEnemyAtkSpeed(float newAtkSpeed) {
        if (AtkMultiplier != 1f) {
            EnemyAttack.AttackDelay = newAtkSpeed;
        }
    }

    void UpdateEnemyMovSpeed(float newMovSpeed) {
        if (AtkMultiplier != 1f) {
            EnemyMovement.UpdateSpeed(newMovSpeed);            
        }
    }
}
