using UnityEngine;
using System.Collections;

public class Core : LivingEntity {

    float roundHealthIncrease = 10;

    protected override void Start() {
        base.Start();
        FindObjectOfType<SpawnManager>().RoundChange += HealthBoost;
    }

    void HealthBoost() {
        if (health + roundHealthIncrease <= startingHealth) {
            health += roundHealthIncrease;
        }
        else {
            health = startingHealth;
        }
        FindObjectOfType<GameUI>().UpdateCoreHealth();
    }

}
