using UnityEngine;
using System.Collections;

public class Sprinter : Enemy {

    protected override void Start() {
        targetName = "Core";
        base.Start();
    }
}
