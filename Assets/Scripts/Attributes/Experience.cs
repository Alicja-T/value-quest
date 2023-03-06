using UnityEngine;
using RPG.Saving;

namespace RPG.Attributes {
public class Experience : MonoBehaviour, ISaveable {

    [SerializeField] float experiencePoints = 0;

    public object CaptureState()
    {
      return experiencePoints;
    }

    public void GainExperience(float newXP) {
        experiencePoints += newXP;
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