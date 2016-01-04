using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))]
public class PlayerMenuMovement : MonoBehaviour {

    int speed = 35;
    int speedReset;

    // time in seconds
    float timeBetweenUpdates = 1;
    float timeToUpdate = 0;

    Core target;
    Player player;

    // Use this for initialization
    void Start () {
        speedReset = speed;
        target = FindObjectOfType<Core>();
        player = FindObjectOfType<Player>();
        player.menu = true;
    }

    void FixedUpdate () {
        // Rotates player around target (Core)
        if (Time.time > timeToUpdate) {
            timeToUpdate = Time.time + timeBetweenUpdates;
            NewSpeed();
            Debug.Log("Speed at: " + speed); 
        }
        transform.RotateAround(target.transform.position, Vector3.up, speed * Time.deltaTime);
    }

    void NewSpeed() {
        speed += 2;
        if (speed >= 75) speed = speedReset;
    }
}
