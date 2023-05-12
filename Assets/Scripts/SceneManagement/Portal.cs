using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using RPG.Core;
using RPG.Control;
namespace RPG.SceneManagement {
public class Portal : MonoBehaviour {

    enum DestinationIdentifier {
        A, B, C, D, E
    }

    [SerializeField] int sceneIndex = -1;
    [SerializeField] Transform spawnPoint;
    [SerializeField] DestinationIdentifier destination;
    [SerializeField] float fadeOutTime = 0.5f;
    [SerializeField] float fadeInTime = 1f;
    [SerializeField] float waitTime = 0.5f;
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == CoreConstants.PLAYER_TAG) {
            StartCoroutine(Transition());
        }    
    }

    private IEnumerator Transition() {
         DontDestroyOnLoad(gameObject);

        Fader fader = FindObjectOfType<Fader>();
   
        SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
        PlayerController playerController = GameObject
                                            .FindWithTag(CoreConstants.PLAYER_TAG)
                                            .GetComponent<PlayerController>();
        playerController.enabled = false;
        yield return fader.FadeOut(fadeOutTime);
        savingWrapper.Save();
        yield return SceneManager.LoadSceneAsync(sceneIndex);
        
        PlayerController newPlayerController = GameObject
                                            .FindWithTag(CoreConstants.PLAYER_TAG)
                                            .GetComponent<PlayerController>();
        newPlayerController.enabled = false;

        savingWrapper.Load();
        yield return new WaitForEndOfFrame();

        Portal otherPortal = GetOtherPortal();
        UpdatePlayer(otherPortal);
        savingWrapper.Save();

        yield return new WaitForSeconds(waitTime);
        yield return fader.FadeIn(fadeInTime);

        newPlayerController.enabled = true;
        Destroy(gameObject);
    }
    private void UpdatePlayer(Portal otherPortal) {
        GameObject player = GameObject.FindGameObjectWithTag(CoreConstants.PLAYER_TAG);
        player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
        //player.transform.position = otherPortal.spawnPoint.position;
        player.transform.rotation = otherPortal.spawnPoint.rotation;
    }

    private Portal GetOtherPortal() {
        foreach (Portal portal in FindObjectsOfType<Portal>()) {
            if (portal == this) continue;
            if (portal.destination != destination) continue;
            return portal;
        }
        return null;
    }
    
    
    }

    
}//namespace