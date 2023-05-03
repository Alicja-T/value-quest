using UnityEngine;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat{
public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider {

[SerializeField] float timeBetweenAttacks = 1f;
[SerializeField] Transform rightHandTransform;
[SerializeField] Transform leftHandTransform;
[SerializeField] Weapon defaultWeapon = null;
[SerializeField] string defaultWeaponName = CoreConstants.DEFAULT_WEAPON_NAME;
Health target;
float timeSinceLastAttack = Mathf.Infinity;
LazyValue<Weapon> currentWeapon;

private void Awake() {
   currentWeapon = new LazyValue<Weapon>(SetDefaultWeapon);
} 

private void Start(){
    currentWeapon.ForceInit();
}

private Weapon SetDefaultWeapon() {
    AttachWeapon(defaultWeapon);
    return defaultWeapon;
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

public void EquipWeapon(Weapon weapon)
    {
      if (weapon == null) return;
      currentWeapon.value = weapon;
      AttachWeapon(weapon);
    }

    private void AttachWeapon(Weapon weapon)
    {
      Animator animator = GetComponent<Animator>();
      if (animator != null)
      {
        //AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        //animator.runtimeAnimatorController = animatorOverrideController;
        weapon.Spawn(rightHandTransform, leftHandTransform, animator);
      }
    }

    public Health GetTarget() {
    return target;
}

//animation event
void Hit() {
    if (target == null) {return;}
    float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
    if (currentWeapon.value.HasProjectile()) {
        currentWeapon.value.LaunchProjectile(leftHandTransform, rightHandTransform, target, gameObject, damage);
    }
    else {
        target.TakeDamage(gameObject, damage);
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
    return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.value.GetRange();
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
      return currentWeapon.value.name;
    }

    public void RestoreState(object state) {
      string weaponName = (string)state;
      Weapon weapon = Resources.Load<Weapon>(weaponName);
      EquipWeapon(weapon);
    }

    public IEnumerable<float> GetAdditiveModifiers(Stat stat) {
      if (stat == Stat.Damage) {
        yield return currentWeapon.value.GetDamage();
      }
    }

    IEnumerable<float> IModifierProvider.GetPercentageModifiers(Stat stat) {
      if (stat == Stat.Damage) {
        yield return currentWeapon.value.GetPercentageBonus();
      }
    }
  }//class Fighter
}//namespace RPG.Combat