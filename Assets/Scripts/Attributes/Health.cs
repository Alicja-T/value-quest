using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;


namespace RPG.Attributes {
public class Health : MonoBehaviour, ISaveable{
   
    [SerializeField] float health = 100f;
    bool isDead = false;

   

    void Start() {
      health = GetComponent<BaseStats>().GetStat(Stat.Health);
    }

    // Update is called once per frame
    public bool IsDead() {
      return isDead;
   }

    public void TakeDamage(GameObject instigator, float damage) {
       health = Mathf.Max(health-damage,0);
       if (health == 0) {
         AwardExperience(instigator);
         DeathSequence();
       }
    }

    public float GetPercentage() {
      return 100f * (health / GetComponent<BaseStats>().GetStat(Stat.Health));
    }

    void DeathSequence() {
      if (!isDead) {
          GetComponent<Animator>().SetTrigger(CoreConstants.DEATH_TRIGGER);
          GetComponent<ActionScheduler>().CancelCurrentAction();
          isDead = true;
      }
    }

    private void AwardExperience(GameObject instigator){
      Experience experience = instigator.GetComponent<Experience>();
      if (experience == null) return;
      experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
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