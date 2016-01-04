using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    Player target;
    Vector3 targetPosition, adjust;
    public float smoothing;

    void Start() {
        target = FindObjectOfType<Player>();
        adjust.Set(0, 14, -12);
    }

    void FixedUpdate() {
        if (target.alive) {
            targetPosition = target.transform.position + adjust;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
        }
    }
}
