using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhasing : MonoBehaviour {
    [Tooltip("How long the enemy will be phased out for")]
    public float PhaseOutDuration = 2f;

    [Tooltip("How long the enemy will be phased in for")]
    public float PhaseInDuration = 1f;

    [Tooltip("How fast the enemy phases out")]
    public float PhaseOutSpeed = 0.01f;

    [Tooltip("How fast the enemy phases in")]
    public float PhaseInSpeed = 0.025f;

    [Tooltip("Phase in and out automatically")]
    public bool AutoPhase = true;

    [Tooltip("Allow attacking during phase out")]
    public bool AllowPhaseAttacks = false;

    [Tooltip("Allow damage to be taken during phase out")]
    public bool AllowPhaseImmunity = true;

    private Renderer EnemyRenderer;
    private Color EnemyColor;
    private bool PhasingIn = true;
    private float Timer = 0f;
    private EnemyHealth EHealth;
    private EnemyAttack EAttack;

    void Start() {
        EnemyRenderer = GetComponentInChildren<Renderer>();
        EnemyColor = EnemyRenderer.material.color;
        EHealth = GetComponent<EnemyHealth>();
        EAttack = GetComponent<EnemyAttack>();
    }

    void Update() {
        if (PhasingIn) {
            Phase(PhasingIn, PhaseInSpeed, PhaseInDuration);
        } else {
            Phase(PhasingIn, -PhaseOutSpeed, PhaseOutDuration);
        }
    }

    /**
     * Phase the enemy in and out
     * Param:
     *   bool ToPhaseIn - True to phase Enemy In, false to phase Enemy out
     *   float PhaseSpeed - How fast to phase the enemy in and out
     *   float PhaseDuration - How long to phase leave the enemy phased in or out
     */
    private void Phase(bool ToPhaseIn, float PhaseSpeed, float PhaseDuration) {
        if (UpdateAlpha(PhaseSpeed) && AutoPhase) {
            if (Timer == 0f) {
                Phased(ToPhaseIn);
            }
            // Wait until PhaseDuration expires
            Timer += Time.deltaTime;
            if (Timer >= PhaseDuration) {
                PhasingIn = !PhasingIn;
                Timer = 0f;
            }
        }
    }

    /**
     * Is called once when Enemy has completed phasing in or phasing out
     */
    private void Phased(bool PhasedIn) {
        if (AllowPhaseImmunity) {
            EHealth.CanTakeDamage(PhasedIn);
        }
        if (!AllowPhaseAttacks) {
            EAttack.enabled = PhasedIn;
        }
    }

    /**
     * Updates the Alpha of Enemy, returns true if Alpha has hit 0 or 1
     */
    private bool UpdateAlpha(float AlphaOffset) {
        bool ReturnValue = true;

        if (EnemyColor.a + AlphaOffset < 0) {
            EnemyColor.a = 0f;
        } else if (EnemyColor.a + AlphaOffset > 1) {
            EnemyColor.a = 1f;
        } else {
            EnemyColor.a += AlphaOffset;
            ReturnValue = false;
        }
        EnemyRenderer.material.color = EnemyColor;
        return ReturnValue;
    }

    /**
     * Phase out enemy
     */
    public void PhaseOut() {
        PhasingIn = false;
    }

    /**
     * Phase in enemy
     */
    public void PhaseIn() {
        PhasingIn = true;
    }
}
