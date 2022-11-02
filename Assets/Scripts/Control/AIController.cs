using UnityEngine;
using RPG.Core;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control {
  public class AIController : MonoBehaviour {

    [SerializeField] float chaseDistance = 5f;
    Fighter fighter;
    GameObject player;
    Health health;
    Mover mover;
    Vector3 guardLocation;

    private void Start() {
        fighter = GetComponent<Fighter>();
        player = GameObject.FindGameObjectWithTag(CoreConstants.PLAYER_TAG);
        health = GetComponent<Health>();
        mover = GetComponent<Mover>();
        guardLocation = transform.position;
    }

    private void Update() {
        if (health.IsDead()) {
            return;
        }
        if (InAttackRange() && fighter.CanAttack(player)) {
            fighter.Attack(player.gameObject);
        }
        else {
            mover.StartMoveAction(guardLocation);
        }
    }

    private bool InAttackRange() {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        return distance < chaseDistance;
    }

    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    private void OnDrawGizmosSelected() {
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

}
}//namespace RPG.Control