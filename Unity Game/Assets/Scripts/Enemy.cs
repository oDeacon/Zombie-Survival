using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity {

    // different skin used for attack color
    public Material attackSkin;

    public float damage;

    protected string targetName;

    NavMeshAgent pathFinder;
    Transform target;
    LivingEntity targetEntity;
    Material skinMaterial;
    Color originalColor;

    float attackDistanceThreshold = 1.5f;
    float timeBetweenAttacks = 1;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollsiionRadius;

    bool hasTarget;

    // initializing
    protected override void Start() {
        base.Start();
        pathFinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;

        // checks if target exists

        if (GameObject.FindGameObjectWithTag(targetName) != null) {
            hasTarget = true;
            target = GameObject.FindGameObjectWithTag(targetName).transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;
            myCollisionRadius = GetComponent<CapsuleCollider>().radius;

            // sets target radius and path finding instructions for certain target
            // !!!
            // must have target instructions set here before setting target name in inheriting class 
            // !!!
            if (targetName == "Player") {
                targetCollsiionRadius = target.GetComponent<CapsuleCollider>().radius;
                StartCoroutine(UpdatePath());
            }
            else if (targetName == "Core") {
                targetCollsiionRadius = Mathf.Sqrt(Mathf.Pow(target.transform.localScale.x, 2) + Mathf.Pow(target.transform.localScale.z, 2)) / 2;
                Vector3 targetPosition = target.position;
                pathFinder.SetDestination(targetPosition);
            }
        }
    }

    // if target dies
    void OnTargetDeath() {
        hasTarget = false;
    }

    // calculates distance to target and Attack() if time to and close enough
    void Update() {
        if (hasTarget) {
            if (Time.time > nextAttackTime) {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollsiionRadius, 2)) {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }
    }

    // attacks
    IEnumerator Attack() {

        pathFinder.enabled = false;

        Vector3 originalPostition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

        float percent = 0;
        float attackSpeed = 2;

        skinMaterial.color = attackSkin.color;
        bool hasAppliedDamage = false;
        //Debug.Log("skin changed");

        while (percent <= 1) {

            if (percent >= .5f && !hasAppliedDamage) {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPostition, attackPosition, interpolation);

            yield return null;
        }

        skinMaterial.color = originalColor;
        //Debug.Log("skin changed back");
        pathFinder.enabled = true;
    }

    // updates path to take to target
    IEnumerator UpdatePath() {
        float refreshRate = .25f;

        // for chasing player
        while (hasTarget) {
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollsiionRadius + attackDistanceThreshold / 2);
            if (!dead && gameObject != null) {
                if (pathFinder.SetDestination(targetPosition)) {
                    Debug.Log("");
                }
                else {
                    Debug.Log("Object " + gameObject.name + " failed");
                }
            }
        yield return new WaitForSeconds(refreshRate);
        }
    }
}
