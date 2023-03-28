using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Stats {
public class BaseStats : MonoBehaviour
{
    [Range(1,99)]
    [SerializeField] int currentLevel = 1;
    [SerializeField] CharacterClass characterClass;
    [SerializeField] Progression progression = null;
    
private void Start() {
    currentLevel = CalculateLevel();
    Experience xp = GetComponent<Experience>();
    if (xp != null){
          xp.OnExperienceGained += UpdateLevel;
    }
}


    private void Update(){
        if (gameObject.tag == CoreConstants.PLAYER_TAG){
            print("My level: " + GetLevel());
        }
    }

    public float GetStat(Stat stat) {

        return progression.GetStat(stat, characterClass, currentLevel);
    }

    public int GetLevel (){
        if (currentLevel < 1) {
            currentLevel = CalculateLevel();
        }
        return currentLevel;
    }

    public void UpdateLevel(){
      int newLevel = CalculateLevel();
      if (newLevel > currentLevel){
        currentLevel = newLevel;
        print("Levelled Up!");
      }
    }

    public int CalculateLevel(){
        Experience xp = GetComponent<Experience>();
        if (xp == null) return 1;
        float currentExperience = xp.GetExperience();
        float[] levels = progression.GetLevels(Stat.ExperienceToLevel, characterClass);
        for (int index = currentLevel - 1; index < levels.Length; index++){
            if (currentExperience <= levels[index]) {
                currentLevel = index + 1;
                break;
            }
        }
        return currentLevel;
    }

}

}
