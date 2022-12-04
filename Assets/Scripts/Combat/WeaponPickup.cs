using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
namespace RPG.Combat {
public class WeaponPickup : MonoBehaviour
{ 

    [SerializeField] Weapon weapon = null;
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other){
        
        if (other.gameObject.tag == CoreConstants.PLAYER_TAG) {
            other.GetComponent<Fighter>().EquipWeapon(weapon);
            Destroy(gameObject);
        }
    }
  
}
}
