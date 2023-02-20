using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
namespace RPG.Combat {
public class WeaponPickup : MonoBehaviour
{ 

    [SerializeField] Weapon weapon = null;
    [SerializeField] float respawnTime = 5f;
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other){
        
        if (other.gameObject.tag == CoreConstants.PLAYER_TAG) {
            other.GetComponent<Fighter>().EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }
    }

    IEnumerator HideForSeconds(float seconds) {
        ShowPickup(false);
        yield return new WaitForSeconds(seconds);
        ShowPickup(true);
    } 
    
    void ShowPickup(bool shouldShow) {
        GetComponent<Collider>().enabled = shouldShow;
        foreach( Transform child in transform ){
            child.gameObject.SetActive(shouldShow);
        }

    }

    void ShowPickup() {

    }
}
}
