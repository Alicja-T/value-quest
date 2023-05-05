using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Control;
namespace RPG.Combat {
public class WeaponPickup : MonoBehaviour, IRaycastable
{ 

    [SerializeField] Weapon weapon = null;
    [SerializeField] float respawnTime = 5f;
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other){
        
        if (other.gameObject.tag == CoreConstants.PLAYER_TAG) {
        Pickup(other.GetComponent<Fighter>());
      }
    }

    private void Pickup(Fighter fighter) {
      fighter.EquipWeapon(weapon);
      StartCoroutine(HideForSeconds(respawnTime));
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

    public bool handleRaycast(PlayerController caller) {
        if (Input.GetMouseButtonDown(0)) {
            Pickup(caller.GetComponent<Fighter>());
        }
        return true;
    }

    public CursorType GetCursorType() {
      return CursorType.Pickup;
    }
  }
}
