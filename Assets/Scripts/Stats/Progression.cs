using System;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Stats {
[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
public class Progression : ScriptableObject
{
    [SerializeField]
    ProgressionCharacterClass[] characterClasses = null;
    Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;
    public float GetStat(Stat stat, CharacterClass checkedClass, int level) {
      BuildLookUp();
      float[] levels = lookupTable[checkedClass][stat];
      if (levels.Length < level) {
        return 0;
      }
      return levels[level - 1];
      
    }

    public float[] GetLevels(Stat stat, CharacterClass checkedClass){
      BuildLookUp();
      return lookupTable[checkedClass][stat];
    }

    private void BuildLookUp()
    {
     if (lookupTable != null) return;
     lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
     foreach (ProgressionCharacterClass charClass in characterClasses){
      Dictionary<Stat, float[]> statsDictionary = new Dictionary<Stat, float[]>();
      foreach (ProgressionStat progressStat in charClass.stats){
        statsDictionary[progressStat.stat] = progressStat.levels;
      }
      lookupTable[charClass.characterClass] = statsDictionary;
     }
    }

    [System.Serializable]
    class ProgressionCharacterClass {
   
        public CharacterClass characterClass;
        public ProgressionStat[] stats;
    }

    [System.Serializable]
    class ProgressionStat {
      public Stat stat;
      public float[] levels;
    }
}
}
