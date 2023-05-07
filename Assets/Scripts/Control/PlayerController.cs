using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using UnityEngine.EventSystems;
using System;
using UnityEngine.AI;

namespace RPG.Control {
  public class PlayerController : MonoBehaviour {
    // Start is called before the first frame update
    Health health;
    [SerializeField]
    float maxDistance = 0.5f;
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
      if (InteractWithComponent()){
        return;
      }

      if (InteractWithMovement()) {
        return;
      }
      SetCursor(CursorType.None);
    }

    private bool InteractWithComponent() {
      RaycastHit[] hits = GetSortedHits();
      foreach (RaycastHit hit in hits) {
        IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
        foreach (IRaycastable raycastable in raycastables) {
          if (raycastable.handleRaycast(this)) {
            SetCursor(raycastable.GetCursorType());
            return true;
          }
        }
      }
      return false;
    }

    private static RaycastHit[] GetSortedHits() {
      RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
      float[] distances = new float[hits.Length];
      for (int i = 0; i < hits.Length; i++) {
        distances[i] = hits[i].distance;
      }
      Array.Sort(distances, hits);
      return hits;
    }

    private bool InteractWithUI()
    {
      bool debug = EventSystem.current.IsPointerOverGameObject();
      if (debug) {
        SetCursor(CursorType.UI);
      }
      return debug;
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

     
      Vector3 target = new Vector3();
      bool hasHit = RaycastNavMesh(out target);
      if (hasHit) {
        if (Input.GetMouseButton(0)) {
          GetComponent<Mover>().StartMoveAction(target, 1f);
        }
        SetCursor(CursorType.Movement);
        return true;     
      }
      return false;
    }

    private bool RaycastNavMesh(out Vector3 target){ 
      RaycastHit hit;
      target = new Vector3();
      bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
      NavMeshHit navMeshHit;
      if (!hasHit) return false;
      if (NavMesh.SamplePosition(hit.point, out navMeshHit, maxDistance, NavMesh.AllAreas)) {
        target = navMeshHit.position;
        return true;
      }
      return false;
    }
    private static Ray GetMouseRay() {
      return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
  }

} //namespace RPG.Control