using UnityEngine;
using RPG.Core;
using RPG.Movement;

namespace RPG.Combat{
public class Fighter : MonoBehaviour, IAction {

[SerializeField] float weaponRange = 2f;
[SerializeField] float timeBetweenAttacks = 1f;
Transform target;
float timeSinceLastAttack = 0;
private void Update() {
    timeSinceLastAttack += Time.deltaTime;
    if (target != null) {
        float dist = Vector3.Distance(target.position, transform.position);
        if (dist > weaponRange) {
            GetComponent<Mover>().MoveTo(target.position);
        }
        else {
            GetComponent<Mover>().Cancel();
            AttackBehaviour();
        }
    }
}

public void Cancel() {
    target = null;
}


public void Attack(CombatTarget combatTarget){
    GetComponent<ActionScheduler>().StartAction(this);
    target = combatTarget.transform;
    print("Die, you bad baddie!");
}

void AttackBehaviour() {
    if (timeSinceLastAttack >= timeBetweenAttacks) {
        GetComponent<Animator>().SetTrigger("Attack");
        timeSinceLastAttack = 0;
    }
}

void Hit() {}

}//class Fighter
}//namespace RPG.Combat