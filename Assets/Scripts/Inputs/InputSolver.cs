using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs {
    public class InputSolver : MonoBehaviour {
        private InputActionMappings _mappings;
        
        private void OnEnable() {
            _mappings ??= new InputActionMappings();

            BindClickActions(_mappings.Player.Click);
            BindMovementActions(_mappings.Player.Move);
            
            _mappings?.Enable();
        }

        private void OnDisable() {
            _mappings?.Disable();
        }

        public Action OnClickStarted;
        public Action OnClickCanceled;

        public event Action<Vector2> OnMovePerformed;

        private void BindClickActions(InputAction playerClick) {
            Debug.Log("Bind click actions");
            playerClick.started += ctx => OnClickStarted?.Invoke();
            playerClick.canceled += ctx => OnClickCanceled?.Invoke();
        }

        private void BindMovementActions(InputAction playerMovement) {
            playerMovement.performed += ctx => OnMovePerformed?.Invoke(ctx.ReadValue<Vector2>());
        }
    }
}
