using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes {
public class Health : MonoBehaviour, ISaveable{
   
    [SerializeField] TakeDamageEvent takeDamage;
    LazyValue<float> healthPoints;
    bool isDead = false;
    BaseStats stats;
    [System.Serializable]
    public class TakeDamageEvent : UnityEvent<float> {

    } 


    void Awake() {
      stats = GetComponent<BaseStats>();
      healthPoints = new LazyValue<float>(GetInitialHealth);
    }

    private float GetInitialHealth(){
      return GetComponent<BaseStats>().GetStat(Stat.Health);
    }

    private void OnEnable() {
      stats.OnLevelUp += RestoreHealth;
    }

    private void OnDisable() {
      stats.OnLevelUp -= RestoreHealth;
    }
    void Start() {
      healthPoints.ForceInit();
    }

    public bool IsDead() {
      return isDead;
   }

    public void TakeDamage(GameObject instigator, float damage) {
      print(gameObject.name + " took damage " + damage);
      healthPoints.value = Mathf.Max(healthPoints.value-damage,0);
      takeDamage.Invoke(damage);
      if (healthPoints.value == 0) {
        AwardExperience(instigator);
        DeathSequence();
      }
      
    }

    public float GetHealthPoints() {
      return healthPoints.value;
    }

    public float GetMaxHealthPoints() {
      return GetComponent<BaseStats>().GetStat(Stat.Health);
    }

    public float GetFraction() {
      return GetHealthPoints()/GetMaxHealthPoints();
    }

    public float GetPercentage() {
      return 100f * (healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health));
    }

    void DeathSequence() {
      if (!isDead) {
          GetComponent<Animator>().SetTrigger(CoreConstants.DEATH_TRIGGER);
          GetComponent<ActionScheduler>().CancelCurrentAction();
          isDead = true;
      }
    }

    void RestoreHealth(){
      healthPoints.value = GetComponent<BaseStats>().GetStat(Stat.Health);
    }

    private void AwardExperience(GameObject instigator){
      Experience experience = instigator.GetComponent<Experience>();
      if (experience == null) return;
      experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
    }

    public object CaptureState() {
      return healthPoints.value;
    }

    public void RestoreState(object state) {
      healthPoints.value = (float)state;
      if (healthPoints.value == 0) {
         DeathSequence();
      }
    }

 
}//class
}//namespace RPG.Core