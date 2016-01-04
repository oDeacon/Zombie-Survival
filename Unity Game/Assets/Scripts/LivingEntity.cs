using UnityEngine;
using System.Collections;

public class LivingEntity : MonoBehaviour, IDamagable {

    float timeBetween = 1;
    float updateTime;

    public float startingHealth;

    protected float health;
    protected bool dead;

    public event System.Action OnDeath;
    public event System.Action TookDamage;

    protected virtual void Start() {
        health = startingHealth;
    }

    void Update() {
        if (Time.time > updateTime) {
            updateTime = Time.time + timeBetween;
        }
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (gameObject.name == "Player") {
            if (!GetComponent<Player>().menu) {
                TookDamage();
            } 
        }
        else if(gameObject.name == "Core") {
            TookDamage();
        }
        if (health <= 0 && !dead) {
            Die();
        }
    }

    public void TakeHit(float damage, RaycastHit hit) {
        // maybe add use for RayCast hit ??? headshots, animations, etc
        TakeDamage(damage);
    }

    public float GetHealth() {
        return health;
    }

    [ContextMenu("Self Destuct")]
    protected void Die() {
        dead = true;
        if(OnDeath != null) {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
