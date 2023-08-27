using UnityEngine;
using GameDevTV.Saving;
using System;

namespace RPG.Stats {
public class Experience : MonoBehaviour, ISaveable {

    [SerializeField] float experiencePoints = 0;
    public event Action OnExperienceGained;
    public object CaptureState()
    {
      return experiencePoints;
    }

    public void GainExperience(float newXP) {
        experiencePoints += newXP;
        OnExperienceGained();
    }

    public void RestoreState(object state)
    {
      experiencePoints = (float) state;
    }

    public float GetExperience(){
        return experiencePoints;
    }
  }
}