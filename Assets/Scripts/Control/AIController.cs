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

    private void Start() {
        fighter = GetComponent<Fighter>();
        player = GameObject.FindGameObjectWithTag(CoreConstants.PLAYER_TAG);
        health = GetComponent<Health>();
    }

    private void Update() {
        if (health.IsDead()) {
            return;
        }
        if (InAttackRange() && fighter.CanAttack(player)) {
            fighter.Attack(player.gameObject);
        }
        else {
            fighter.Cancel();
        }
    }

    private bool InAttackRange() {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        return distance < chaseDistance;
    }


}
}//namespace RPG.Control