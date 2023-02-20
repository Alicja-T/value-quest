using UnityEngine;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Combat{
public class Fighter : MonoBehaviour, IAction, ISaveable {

[SerializeField] float timeBetweenAttacks = 1f;
[SerializeField] Transform rightHandTransform;
[SerializeField] Transform leftHandTransform;
[SerializeField] Weapon defaultWeapon = null;
[SerializeField] string defaultWeaponName = CoreConstants.DEFAULT_WEAPON_NAME;
Health target;
float timeSinceLastAttack = Mathf.Infinity;
Weapon currentWeapon = null;

private void Awake() {
   if (currentWeapon == null) {
    EquipWeapon(defaultWeapon);
   }
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
    print("equipped weapon: " + currentWeapon.name);
    Animator animator = GetComponent<Animator>();
    if (animator != null) {
        //AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        //animator.runtimeAnimatorController = animatorOverrideController;
        currentWeapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }
}

//animation event
void Hit() {
    if (target == null) {return;}
    if (currentWeapon.HasProjectile()) {
        currentWeapon.LaunchProjectile(leftHandTransform, rightHandTransform, target);
    }
    else {
        target.TakeDamage(currentWeapon.GetDamage());
    }
}

void Shoot() {
    Hit();
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

    public object CaptureState() {
      return currentWeapon.name;
    }

    public void RestoreState(object state) {
      string weaponName = (string)state;
      Weapon weapon = Resources.Load<Weapon>(weaponName);
      EquipWeapon(weapon);
    }
  }//class Fighter
}//namespace RPG.Combat