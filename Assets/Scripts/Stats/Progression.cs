using System;
using UnityEngine;
namespace RPG.Stats {
[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
public class Progression : ScriptableObject
{
    [SerializeField]
    ProgressionCharacterClass[] characterClasses = null;

    public float GetHealth(CharacterClass checkedClass, int startingLevel) {
      foreach (ProgressionCharacterClass charClass in characterClasses){
        if(charClass.characterClass == checkedClass) {
            return charClass.health[startingLevel - 1];
        }
      }
      return 0;
    }

    [System.Serializable]
    class ProgressionCharacterClass {
   
        public CharacterClass characterClass;
        public float[] health;
    }
}
}
