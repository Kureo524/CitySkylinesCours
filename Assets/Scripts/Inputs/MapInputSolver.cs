using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs {
    public class MapInputSolver {
        public Action<Vector2Int> OnMapClicked;

        private LayerMask _floorMask;
        
        public MapInputSolver(ref Action clickEvent, LayerMask floorMask) {
            clickEvent += OnClickEvent;
            _floorMask = floorMask;
        }

        private void OnClickEvent() {
            Debug.Log("CLicked");
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(
                    ray,
                    out RaycastHit hit,
                    Mathf.Infinity,
                    _floorMask))
            {
                Vector2Int worldPosition = new Vector2Int((int)hit.point.x, (int)hit.point.z);
                OnMapClicked?.Invoke(worldPosition);
            }
        }
    }
}
