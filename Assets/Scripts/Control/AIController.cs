using UnityEngine;
using RPG.Core;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using GameDevTV.Utils;

namespace RPG.Control {
  public class AIController : MonoBehaviour {

    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTime = 5f;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointEpsilon = 2.3f;
    [SerializeField] float dwellingTime = 2f;
    [Range(0,1)]
    [SerializeField] float patrolSpeedFraction = 0.2f;

    Fighter fighter;
    GameObject player;
    Health health;
    Mover mover;
    LazyValue<Vector3> guardLocation;
    int currentPathInd = 0;
    float timeSinceLastSawPlayer = Mathf.Infinity;
    float timeAtWaypoint = Mathf.Infinity;

    private void Awake(){
        fighter = GetComponent<Fighter>();
        player = GameObject.FindGameObjectWithTag(CoreConstants.PLAYER_TAG);
        health = GetComponent<Health>();
        mover = GetComponent<Mover>();
        guardLocation = new LazyValue<Vector3>(GetGuardPosition);
    }

    private Vector3 GetGuardPosition(){
      return transform.position;
    }
    private void Start() {
        guardLocation.ForceInit();
    }

    private void Update() {
      if (health.IsDead()) {
        return;
      }
      if (InAttackRange() && fighter.CanAttack(player)) {
        AttackBehavior();
      } else if (timeSinceLastSawPlayer < suspicionTime) {
        GetComponent<ActionScheduler>().CancelCurrentAction();
      } else {
        PatrolBehavior();
      }
      UpdateTimers();
    }

    private void AttackBehavior() {
      timeSinceLastSawPlayer = 0;
      fighter.Attack(player.gameObject);
    }

    private void UpdateTimers() {
      timeSinceLastSawPlayer += Time.deltaTime;
      timeAtWaypoint += Time.deltaTime;
    }

    private void PatrolBehavior() {
      Vector3 nextPosition = guardLocation.value;
      if (patrolPath != null) {
        if (IsAtWaypoint()) {
            CycleWaypoint();
            timeAtWaypoint = 0;
        }
        nextPosition = GetWaypoint();
      }
      if (timeAtWaypoint > dwellingTime) {
        mover.StartMoveAction(nextPosition, patrolSpeedFraction);
      }
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