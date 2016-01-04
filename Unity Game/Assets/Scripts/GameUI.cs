using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour {

    int round;
    Text[] displayText;

    SpawnManager spawner;
    Player player;
    Core core;

    void Start() {
        spawner = FindObjectOfType<SpawnManager>();
        player = FindObjectOfType<Player>();
        core = FindObjectOfType<Core>();

        // subscriptions
        spawner.RoundChange += UpdateRound;
        player.TookDamage += UpdatePlayerHealth;
        core.TookDamage += UpdateCoreHealth;

        displayText = GetComponentsInChildren<Text>();
    }

    void UpdateRound() {
        round = spawner.GetRoundNum();
        displayText[0].text = "Round: " + round;
    }

    void UpdatePlayerHealth() {
        displayText[1].text = "Health: " + player.GetHealth();
    }

    public void UpdateCoreHealth() {
        displayText[2].text = "Core: " + core.GetHealth();
    }

}
