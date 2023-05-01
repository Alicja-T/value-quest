﻿using System.Collections;
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
    [SerializeField] bool shouldUseModifier = false;
    Experience xp;
    public event Action OnLevelUp;

    private void Awake(){
      xp = GetComponent<Experience>();
    }

    private void OnEnable(){
      if (xp != null) {
        xp.OnExperienceGained += UpdateLevel;
      }
    }

    private void OnDisable() {
      if (xp != null) {
        xp.OnExperienceGained -= UpdateLevel;
      }
    }
    private void Start() {
      currentLevel = CalculateLevel();
    }
  

    public float GetStat(Stat stat)
    {

      return (GetBaseStat(stat) + GetAdditiveModifier(stat)) 
      * (1 + GetPercentageModifier(stat)/100);
    }

    private float GetPercentageModifier(Stat stat)
    {
      if (!shouldUseModifier) return 0f;
      float total = 0f;
      foreach(IModifierProvider provider in GetComponents<IModifierProvider>()) {
        foreach(float item in provider.GetPercentageModifiers(stat)){
            total += item;
        }
      }
      return total;
    }

    private float GetBaseStat(Stat stat)
    {
      return progression.GetStat(stat, characterClass, currentLevel);
    }

    private float GetAdditiveModifier(Stat stat){
      if (!shouldUseModifier) return 0f;
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
