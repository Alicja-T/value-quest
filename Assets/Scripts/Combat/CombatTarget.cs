using UnityEngine;
using RPG.Attributes;
using RPG.Control;
namespace RPG.Combat{
  [RequireComponent(typeof(Health))]
  public class CombatTarget : MonoBehaviour, IRaycastable
  {
    public bool handleRaycast(PlayerController caller) {
      Fighter fighter = caller.GetComponent<Fighter>();
      if (!fighter.CanAttack(gameObject)) {
        return false;
      }
      if (Input.GetMouseButton(0)) {
        fighter.Attack(gameObject);
      }
      return true;
    }
  }

}//namespace RPG.Combat