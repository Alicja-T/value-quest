using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.SceneManagement{
public class Fader : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Coroutine currentlyActive;
    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeOutImmediate() {
        canvasGroup.alpha = 1f;
    }
    public IEnumerator FadeOut(float time) {
        return Fade(1f, time);
    }

    public IEnumerator FadeIn(float time) {
        return Fade(0, time);
    }
    private IEnumerator Fade(float target, float time) {
        if (currentlyActive != null) {
            StopCoroutine(currentlyActive);
        }
        currentlyActive = StartCoroutine(FadeCoroutine(target, time));
        yield return currentlyActive;
   }

    private IEnumerator FadeCoroutine(float target, float time) {
        while (!Mathf.Approximately(target, canvasGroup.alpha)) {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 
                                                target, 
                                                Time.deltaTime/time);
            yield return null;
        }
    }
    
}
}//namespace