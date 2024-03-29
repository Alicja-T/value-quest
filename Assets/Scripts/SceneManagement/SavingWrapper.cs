﻿using System.Collections;
using UnityEngine;
using GameDevTV.Saving;
namespace RPG.SceneManagement
{
  public class SavingWrapper : MonoBehaviour
{
    [SerializeField] float fadeInTime = 0.2f;
    const string defaultSaveFile = "save";

    private void Awake() {
        StartCoroutine(LoadLastScene());
    }
    private IEnumerator LoadLastScene() {

       yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
       Fader fader = FindObjectOfType<Fader>();
       fader.FadeOutImmediate();
       yield return fader.FadeIn(fadeInTime);
    }
   
    void Update(){
        if (Input.GetKeyDown(KeyCode.L)) {
            Load();
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.Delete)) {
            Delete();
        }
    }

    public void Load() {
        StartCoroutine(GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile));
    }

    public void Save() {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }

    public void Delete() {
        GetComponent<SavingSystem>().Delete(defaultSaveFile);
    }

}
}//namespace
