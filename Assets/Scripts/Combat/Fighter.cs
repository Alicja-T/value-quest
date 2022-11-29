using UnityEngine;
using RPG.Core;
using RPG.Movement;

namespace RPG.Combat{
public class Fighter : MonoBehaviour, IAction {

[SerializeField] float weaponRange = 2f;
[SerializeField] float timeBetweenAttacks = 1f;
[SerializeField] float weaponDamage = 10f;

[SerializeField] Transform rightHandTransform;

[SerializeField] Weapon weapon;

Health target;
float timeSinceLastAttack = Mathf.Infinity;

private void Start() {
    SpawnWeapon();
} 


private void Update() {
    timeSinceLastAttack += Time.deltaTime;
    if (target == null) return;
    if (target.IsDead()) return;

    if (!GetIsInRange()) {
        GetComponent<Mover>().MoveTo(target.transform.position, 1f);
    }
    else {
        GetComponent<Mover>().Cancel();
        AttackBehaviour();
    }
}

private void SpawnWeapon() {
    if (weapon == null) return;
    Animator animator = GetComponent<Animator>();
    if (animator != null) {
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;
        weapon.Spawn(rightHandTransform, animator);
    }
}

//animation event
void Hit() {
    if (target == null) {return;}
    target.TakeDamage(weaponDamage);
}

void AttackBehaviour() {
    transform.LookAt(target.transform.position);
    if (timeSinceLastAttack > timeBetweenAttacks) {
        // triggers Hit() event
        TriggerAttack();
        timeSinceLastAttack = 0;
    }
}


private bool GetIsInRange() {
    return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
}

private void TriggerAttack() {
    GetComponent<Animator>().ResetTrigger(CombatConstants.STOP_ATTACK);
    GetComponent<Animator>().SetTrigger(CombatConstants.ATTACK);
}

private void StopAttack() {
    GetComponent<Animator>().ResetTrigger(CombatConstants.ATTACK);
    GetComponent<Animator>().SetTrigger(CombatConstants.STOP_ATTACK);
}


public bool CanAttack(GameObject combatTarget) {
    if (combatTarget == null) return false;
    Health targetToTest = combatTarget.GetComponent<Health>();
    return targetToTest != null && !targetToTest.IsDead();
}

public void Attack(GameObject combatTarget){
    GetComponent<ActionScheduler>().StartAction(this);
    target = combatTarget.GetComponent<Health>();
}

public void Cancel() {
    StopAttack();
    target = null;
    GetComponent<Mover>().Cancel();
}



}//class Fighter
}//namespace RPG.Combat