using UnityEngine;
using RPG.Saving;

namespace RPG.Core {
public class Health : MonoBehaviour, ISaveable{
   
    [SerializeField] float health = 100f;
    bool isDead = false;

   
    // Update is called once per frame
    public bool IsDead() {
      return isDead;
   }

    

    public void TakeDamage(float damage) {
       health = Mathf.Max(health-damage,0);
       print("my health is " + health);
       if (health == 0) {
         DeathSequence();
       }
    }


    void DeathSequence() {
      if (!isDead) {
          GetComponent<Animator>().SetTrigger(CoreConstants.DEATH_TRIGGER);
          GetComponent<ActionScheduler>().CancelCurrentAction();
          isDead = true;
      }
    }

    public object CaptureState() {
      return health;
    }

    public void RestoreState(object state) {
      health = (float)state;
      if (health == 0) {
         DeathSequence();
      }
    }

 
}//class
}//namespace RPG.Core