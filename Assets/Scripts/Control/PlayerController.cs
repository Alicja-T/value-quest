﻿using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using UnityEngine.EventSystems;

namespace RPG.Control {
  public class PlayerController : MonoBehaviour {
    // Start is called before the first frame update
    Health health;

    enum CursorType {
      None, 
      Movement, 
      Combat, 
      UI
    }
    [System.Serializable]
    struct CursorMapping {
      public CursorType type;
      public Vector2 hotspot;
      public Texture2D texture;
    }

    [SerializeField] CursorMapping[] cursorMappings = null;
    private void Awake() {
      health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update() {
      if (InteractWithUI()) {
        return;
      }
      if (health.IsDead()) {
        SetCursor(CursorType.None);
        return;
      }
      if (InteractWithCombat()) {
        return;
      };
      if (InteractWithMovement()) {
        return;
      }
      SetCursor(CursorType.None);
    }

    private bool InteractWithUI()
    {
      bool debug = EventSystem.current.IsPointerOverGameObject();
      if (debug) {
        SetCursor(CursorType.UI);
      }
      return debug;
    }

    private bool InteractWithCombat() {
      RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
      foreach (RaycastHit hit in hits) {
        CombatTarget target = hit.transform.gameObject.GetComponent<CombatTarget>();
        if (target == null) continue;
        bool canAttack = GetComponent<Fighter>().CanAttack(target.gameObject);
        if (!canAttack) {
          continue;
        }
        if (Input.GetMouseButton(0)) {
          GetComponent<Fighter>().Attack(target.gameObject);
        }
        SetCursor(CursorType.Combat);
        return true;
      }
      return false;
    }

    private void SetCursor(CursorType type)
    {
      CursorMapping mapping = GetCursorMapping(type);
      Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
    }

    private CursorMapping GetCursorMapping(CursorType type) {
      foreach (CursorMapping mapping in cursorMappings) {
        if (mapping.type == type) {
          return mapping;
        }
      }
      return cursorMappings[0];
    }
    private bool InteractWithMovement() {

      RaycastHit hit;
      bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
      if (hasHit) {
        if (Input.GetMouseButton(0)) {
          GetComponent<Mover>().StartMoveAction(hit.point, 1f);
        }
        SetCursor(CursorType.Movement);
        return true;     
      }
      return false;
    }

    private static Ray GetMouseRay() {
      return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
  }

} //namespace RPG.Control