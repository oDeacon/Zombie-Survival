using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerController))]
[RequireComponent (typeof(GunController))]
public class Player : LivingEntity {
    
    public float moveSpeed = 5f;
    public bool alive = true;
    public bool menu = false;

    PlayerController controller;
    Camera viewCamera;
    GunController gunController;

    Core core;
    
	protected override void Start () {
        
        base.Start();
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        core = FindObjectOfType<Core>();
        viewCamera = Camera.main;
        OnDeath += SetDead;
        //subscribe to core death so player dies if core dies
        core.OnDeath += Die;
        
    }

	protected void Update () {

        if (!menu) {
            // Movement input
            Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            Vector3 moveVeloctiy = moveInput.normalized * moveSpeed;
            controller.Move(moveVeloctiy);

            // Weapon input
            if (Input.GetMouseButton(0)) {
                gunController.Shoot();
            }
        } 
        
        // Look input
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance)) {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point, Color.red);
            controller.LookAt(point);
        }

        
    }
    
    void SetDead() {
        alive = false;  
    }
    
}
