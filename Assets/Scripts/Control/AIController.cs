using UnityEngine;
using RPG.Core;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control {
  public class AIController : MonoBehaviour {

    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 5f;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointEpsilon = 2.3f;

    Fighter fighter;
    GameObject player;
    Health health;
    Mover mover;
    Vector3 guardLocation;
    int currentPathInd = 0;
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
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        else {
            PatrolBehavior();
        }
      timeSinceLastSawPlayer += Time.deltaTime;
    }

    private void PatrolBehavior() {
      Vector3 nextPosition = guardLocation;
      if (patrolPath != null) {
        if (IsAtWaypoint()) {
            CycleWaypoint();
        }
        nextPosition = GetWaypoint();
      }
      mover.StartMoveAction(nextPosition);
    }

    private bool InAttackRange() {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        return distance < chaseDistance;
    }


    private bool IsAtWaypoint() {
        float distance = Vector3.Distance(transform.position, GetWaypoint());
        return distance < waypointEpsilon;
    }

    private Vector3 GetWaypoint() {
        return patrolPath.GetWaypoint(currentPathInd);
    }

    private void CycleWaypoint() {
        currentPathInd = patrolPath.GetNextWaypoint(currentPathInd);
    }

    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    private void OnDrawGizmosSelected() {
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

}
}//namespace RPG.Control