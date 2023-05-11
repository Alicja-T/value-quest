
using UnityEngine;
using UnityEngine.UI;
namespace RPG.Attributes{
public class HealthBarScript : MonoBehaviour
{
    [SerializeField] RectTransform healthBar = null;
    [SerializeField] Health health = null;
    [SerializeField] Canvas canvas = null;
 
    private void Start() {
        canvas.enabled = false;
    }

    public void SetHealthBar() {
        canvas.enabled = true;
        float healthPercent = health.GetFraction();
        if (Mathf.Approximately(healthPercent, 0)) {
            canvas.enabled = false;
        }
        print("setting health bar to " + healthPercent);
        healthBar.localScale = new Vector3(healthPercent, 1,  1);
    }
}
}//namespace
