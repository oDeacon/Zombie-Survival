using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

    public Transform weaponHold;
    public Gun startingGun;
    Gun equippedGun;

    // equips startingGun
    void Start() {
        if (startingGun != null) {
            EquipGun(startingGun);
        }
    }

    // used to equip a gun
    public void EquipGun(Gun gunToEquip){
        // destorys instance of currently equipped gun
        if (equippedGun != null) {
            Destroy(equippedGun.gameObject);
        }
        // rotates arm and then equips
        weaponHold.Rotate(-90,0,0);
        equippedGun = (Gun)Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation);
        equippedGun.transform.parent = weaponHold;
    }

    public void Shoot() {
        if (equippedGun != null) {
            equippedGun.Shoot();
        }
    }
}
