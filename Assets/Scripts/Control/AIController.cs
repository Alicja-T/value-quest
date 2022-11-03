using UnityEngine;
using RPG.Core;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control {
  public class AIController : MonoBehaviour {

    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 5f;
    Fighter fighter;
    GameObject player;
    Health health;
    Mover mover;
    Vector3 guardLocation;
    float timeSinceLastSawPlayer = Mathf.Infinity;
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
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player.gameObject);
        }
        else if (timeSinceLastSawPlayer < suspicionTime) {
            print("I suspect you!");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        else {
            mover.StartMoveAction(guardLocation);
        }
        timeSinceLastSawPlayer += Time.deltaTime;
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