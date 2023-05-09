using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.UI {
public class DamageTextSpanwer : MonoBehaviour
{
    [SerializeField] DamageText damageTextPrefab = null;
    public void Spawn(float damageAmount) {
        print("Damage spawned " + damageAmount);
        DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform);
        instance.SetValue(damageAmount);
    }

}
}//namespace