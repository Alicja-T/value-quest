using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;


namespace RPG.Attributes {
public class Health : MonoBehaviour, ISaveable{
   
    float health = -1f;
    bool isDead = false;
    BaseStats stats;
   
    void Awake() {
      stats = GetComponent<BaseStats>();
    }

    private void OnEnable() {
      stats.OnLevelUp += RestoreHealth;
    }

    private void OnDisable() {
      stats.OnLevelUp -= RestoreHealth;
    }
    void Start() {
    if (health < 0) {
        health = stats.GetStat(Stat.Health);
      }
    }

    public bool IsDead() {
      return isDead;
   }

    public void TakeDamage(GameObject instigator, float damage) {
      print(gameObject.name + " took damage " + damage);
      health = Mathf.Max(health-damage,0);
      if (health == 0) {
        AwardExperience(instigator);
        DeathSequence();
      }
    }

    public float GetHealthPoints() {
      return health;
    }

    public float GetMaxHealthPoints() {
      return GetComponent<BaseStats>().GetStat(Stat.Health);
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

    void RestoreHealth(){
      health = GetComponent<BaseStats>().GetStat(Stat.Health);
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