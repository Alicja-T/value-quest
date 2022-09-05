using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using System;

namespace RPG.Control {
  public class PlayerController : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
      InteractWithCombat();
      InteractWithMovement();

    }

    private void InteractWithCombat() {
      RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
      foreach(RaycastHit hit in hits){
        CombatTarget target = hit.transform.gameObject.GetComponent<CombatTarget>();
        if (target!= null) {
          if (Input.GetMouseButtonDown(0)) {
	          GetComponent<Fighter>().Attack(target);
          }
        }
      }
    }

    private void InteractWithMovement() {
      if (Input.GetMouseButton(0)) {
        MoveToCursor();
      }
    }

    private void MoveToCursor() {
      RaycastHit hit;
      bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
      if (hasHit) {
        GetComponent<Mover>().MoveTo(hit.point);
      }
    }

    private static Ray GetMouseRay() {
      return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
  }

} //namespace RPG.Control