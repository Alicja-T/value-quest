using UnityEngine;
using RPG.Core;
using RPG.Attributes;
using GameDevTV.Inventories;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Combat {
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
public class WeaponConfig : EquipableItem, IModifierProvider
{  [SerializeField] AnimatorOverrideController weaponOverride = null;
    [SerializeField] Weapon equippedPrefab = null;
    [SerializeField] float weaponRange = 2f;
    [SerializeField] float weaponDamage = 10f;
    [SerializeField] float percentageBonus = 0f;
    [SerializeField] bool isRightHand = true;
    [SerializeField] Projectile projectile = null;

    public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator){
        DestroyOldWeapon(rightHand, leftHand);
        Weapon weapon = null;
        if (equippedPrefab != null){
            Transform handTransform = GetHandTransform(rightHand, leftHand);
            weapon = Instantiate(equippedPrefab, handTransform);
            weapon.name = CoreConstants.WEAPON;
        }
        var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
        if (weaponOverride != null){ 
            animator.runtimeAnimatorController = weaponOverride;
        }
        else if (overrideController != null) {
           animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;

        }
        return weapon;
    }


    private void DestroyOldWeapon(Transform rightHand, Transform leftHand) {
        Transform oldWeapon = rightHand.Find(CoreConstants.WEAPON);
        if (oldWeapon == null) {
            oldWeapon = leftHand.Find(CoreConstants.WEAPON);
        }
        if (oldWeapon == null) { return;}
        oldWeapon.name = CoreConstants.DESTROYING_STATE;
        Destroy(oldWeapon.gameObject);
    }

    private Transform GetHandTransform(Transform rightHand, Transform leftHand)
    {
      return isRightHand ? rightHand : leftHand;
    }

    public bool HasProjectile() {
        return projectile != null;
    }

    public void LaunchProjectile (Transform leftHand, 
                                  Transform rightHand, 
                                  Health target,
                                  GameObject instigator,
                                  float calculatedDamage){
        Projectile projectileInstance = Instantiate(
            projectile, 
            GetHandTransform(leftHand, rightHand).position, Quaternion.identity);
        projectileInstance.SetTarget(instigator, target, calculatedDamage);
    }

    public float GetRange() {
        return weaponRange;
    }

    public float GetDamage() {
        return weaponDamage;
    }

    public float GetPercentageBonus(){
        return percentageBonus;
    }

    public IEnumerable<float> GetAdditiveModifiers(Stat stat) {
        //TODO change to StatEquipableItem to implement more stats
        if (stat == Stat.Damage) {
            yield return weaponDamage;
        }
    }

    public IEnumerable<float> GetPercentageModifiers(Stat stat) {
        //TODO change to StatEquipableItem to implement more stats
        if (stat == Stat.Damage) {
            yield return percentageBonus;
        }
    }
    }  
}//namespace
