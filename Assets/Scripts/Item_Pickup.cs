using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Pickup : MonoBehaviour
{

    [SerializeField] private Weapon Weapon;
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerWeaponController>()?.PickupWeapon(Weapon);
        
    }

}
