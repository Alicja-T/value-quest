using UnityEngine;
using RPG.Core;
using RPG.Attributes;
namespace RPG.Combat {
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
public class Weapon : ScriptableObject
{  [SerializeField] AnimatorOverrideController weaponOverride = null;
    [SerializeField] GameObject equippedPrefab = null;
    [SerializeField] float weaponRange = 2f;
    [SerializeField] float weaponDamage = 10f;
    [SerializeField] bool isRightHand = true;
    [SerializeField] Projectile projectile = null;

    public void Spawn(Transform rightHand, Transform leftHand, Animator animator){
        DestroyOldWeapon(rightHand, leftHand);
        if (equippedPrefab != null){
            Transform handTransform = GetHandTransform(rightHand, leftHand);
            GameObject weapon = Instantiate(equippedPrefab, handTransform);
            weapon.name = CoreConstants.WEAPON;
        }
        var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
        if (weaponOverride != null){ 
            animator.runtimeAnimatorController = weaponOverride;
        }
        else if (overrideController != null) {
           animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;

        }
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
                                  GameObject instigator){
        Projectile projectileInstance = Instantiate(
            projectile, 
            GetHandTransform(leftHand, rightHand).position, Quaternion.identity);
        projectileInstance.SetTarget(instigator, target, weaponDamage);
    }

    public float GetRange() {
        return weaponRange;
    }

    public float GetDamage() {
        return weaponDamage;
    }

}  
}//namespace
