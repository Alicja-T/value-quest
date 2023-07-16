using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.UI {

public class ShowPanel : MonoBehaviour{

[SerializeField] KeyCode toggleKey = KeyCode.Escape;
[SerializeField] GameObject uiPanel = null;

void Start() {
    uiPanel.SetActive(false);
}

private void Update() {
    if (Input.GetKeyDown(toggleKey)){
        uiPanel.SetActive(!uiPanel.activeSelf);
    }    
}


} //class


}//namespace 