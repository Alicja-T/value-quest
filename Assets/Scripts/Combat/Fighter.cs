using UnityEngine;
using RPG.Core;
using RPG.Movement;
using GameDevTV.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;
using GameDevTV.Inventories;

namespace RPG.Combat{
public class Fighter : MonoBehaviour, IAction, ISaveable {

[SerializeField] float timeBetweenAttacks = 1f;
[SerializeField] Transform rightHandTransform;
[SerializeField] Transform leftHandTransform;
[SerializeField] WeaponConfig defaultWeapon = null;
[SerializeField] string defaultWeaponName = CoreConstants.DEFAULT_WEAPON_NAME;
Health target;
float timeSinceLastAttack = Mathf.Infinity;
WeaponConfig currentWeaponConfig;
LazyValue<Weapon> currentWeapon;
Equipment equipment;
private void Awake() {
   currentWeaponConfig = defaultWeapon;
   currentWeapon = new LazyValue<Weapon>(SetDefaultWeapon);
   equipment = GetComponent<Equipment>();
   if (equipment) {
    equipment.equipmentUpdated += UpdateWeapon;
   }
} 

private void Start(){
     currentWeapon.ForceInit();
}

private Weapon SetDefaultWeapon() {
   return AttachWeapon(defaultWeapon);
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

private void UpdateWeapon() {
  var weapon = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
  if (weapon != null) {
    EquipWeapon(weapon);
  }
  else {
    EquipWeapon(defaultWeapon);
  }
}
public void EquipWeapon(WeaponConfig weapon)
    {
      if (weapon == null) return;
      currentWeaponConfig = weapon;
      currentWeapon.value = AttachWeapon(weapon);
    }

    private Weapon AttachWeapon(WeaponConfig weapon)
    {
      Animator animator = GetComponent<Animator>();
      return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }

    public Health GetTarget() {
    return target;
}

//animation event
void Hit() {
    if (target == null) {return;}
    float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
    if (currentWeapon.value) {
      currentWeapon.value.OnHit();
    }
    if (currentWeaponConfig.HasProjectile()) {
        currentWeaponConfig.LaunchProjectile(leftHandTransform, rightHandTransform, target, gameObject, damage);
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
    return Vector3.Distance(transform.position, target.transform.position) < currentWeaponConfig.GetRange();
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
      return currentWeaponConfig.name;
    }

    public void RestoreState(object state) {
      string weaponName = (string)state;
      WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
      EquipWeapon(weapon);
    }
    
  }//class Fighter
}//namespace RPG.Combat