using UnityEngine;
using RPG.Core;
using RPG.Movement;

namespace RPG.Combat{
public class Fighter : MonoBehaviour {

[SerializeField] float weaponRange = 2f;

Transform target;
private void Update() {
    if (target != null) {
        float dist = Vector3.Distance(target.position, transform.position);
        if (dist > weaponRange) {
            GetComponent<Mover>().MoveTo(target.position);
        }
        else {
            GetComponent<Mover>().Stop();
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

}//class Fighter
}//namespace RPG.Combat