using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Stats {
public class BaseStats : MonoBehaviour
{
    [Range(1,99)]
    [SerializeField] int currentLevel = 1;
    [SerializeField] CharacterClass characterClass;
    [SerializeField] Progression progression = null;
    [SerializeField] GameObject levelUpEffect = null;
    
    public event Action OnLevelUp;
private void Start() {
    currentLevel = CalculateLevel();
    Experience xp = GetComponent<Experience>();
    if (xp != null){
          xp.OnExperienceGained += UpdateLevel;
    }
}
  

    public float GetStat(Stat stat) {

        return progression.GetStat(stat, characterClass, currentLevel) + GetAdditiveModifier(stat);
    }

    private float GetAdditiveModifier(Stat stat){
      float total = 0f;
      foreach(IModifierProvider provider in GetComponents<IModifierProvider>()) {
        foreach(float item in provider.GetAdditiveModifiers(stat)){
            total += item;
        }
      }
      return total;
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
        LevelUpEffect();
        OnLevelUp();
      }
    }

    private void LevelUpEffect()
    {
      Instantiate(levelUpEffect, transform);
    }

    public int CalculateLevel(){
        Experience xp = GetComponent<Experience>();
        if (xp == null) return 1;
        float currentExperience = xp.GetExperience();
        int level = 0;
        float[] levels = progression.GetLevels(Stat.ExperienceToLevel, characterClass);
        for (int index = 0; index < levels.Length; index++){
            if (currentExperience <= levels[index]) {
                level = index + 1;
                break;
            }
        }
        return level;
    }

}

}
