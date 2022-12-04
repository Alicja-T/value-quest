using UnityEngine;
using RPG.Core;
using RPG.Movement;

namespace RPG.Combat{
public class Fighter : MonoBehaviour, IAction {


[SerializeField] float timeBetweenAttacks = 1f;
[SerializeField] Transform rightHandTransform;
[SerializeField] Weapon defaultWeapon = null;

Health target;
float timeSinceLastAttack = Mathf.Infinity;
Weapon currentWeapon = null;

private void Start() {
    EquipWeapon(defaultWeapon);
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

public void EquipWeapon(Weapon weapon) {
    if (weapon == null) return;
    currentWeapon = weapon;
    Animator animator = GetComponent<Animator>();
    if (animator != null) {
        //AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        //animator.runtimeAnimatorController = animatorOverrideController;
        currentWeapon.Spawn(rightHandTransform, animator);
    }
}

//animation event
void Hit() {
    if (target == null) {return;}
    target.TakeDamage(currentWeapon.GetDamage());
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
    return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
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