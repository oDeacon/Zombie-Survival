using UnityEngine;
using System.Collections;

public class Walker : Enemy {

	protected override void Start() {
        targetName = "Player";
        base.Start();
    }
}
