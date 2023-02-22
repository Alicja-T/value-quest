using UnityEngine;

namespace RPG.Attributes {
public class Experience : MonoBehaviour {

    [SerializeField] float experiencePoints = 0;
    
    public void GainExperience(float newXP) {
        experiencePoints += newXP;
    }
}
}