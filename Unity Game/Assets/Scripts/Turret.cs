using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

    Enemy enemy;
    Transform target;
    Transform protecting;
    // whoa made some changes
    public Transform body;
    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots = 8;
    public float muzzleVelocity = 5;

    float nextShotTime;

    bool hasTarget;

    void Start() {
        protecting = FindObjectOfType<Core>().transform;
        enemy = FindObjectOfType<Walker>();
        GetTarget();
    }

    void Update() {
        Aim();
        if (Input.GetKeyDown("e")) {
            ChangeTarget();
        }
        if (Input.GetKeyDown("space")) {
            Shoot();
        }
    }

    void ChangeTarget() {
        if (protecting.CompareTag("Core")) {
            protecting = FindObjectOfType<Player>().transform;
        }
        else {
            protecting = FindObjectOfType<Core>().transform;
        }
    }

    void GetTarget() {

        //if (Vector3.Distance(transform.position, obj.transform.position) <= Vector3.Distance(transform.position, closestObject.transform.position)) {
          //  closestObject = obj;
        //}

        target = enemy.GetComponentInChildren<Transform>();
    }

    void Aim() {
        body.LookAt(target, Vector3.up);
    }

    public void Shoot() {

        if (Time.time > nextShotTime) {
            nextShotTime = Time.time + msBetweenShots / 1000;
            Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            newProjectile.SetSpeed(muzzleVelocity);
        }
    }
}