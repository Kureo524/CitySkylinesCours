using System;
using System.Collections.Generic;
using BuildingSystem;
using Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Management {
    [RequireComponent(typeof(InputSolver))]
    public class SortaManager : MonoBehaviour {
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private List<Vector2Int> gridSizeChecks;

        [SerializeField] private LayerMask floorMask;
        
        private Grid _mainGrid;
        private InputSolver _inputSolver;
        private MapInputSolver _mapInputSolver;
        private CityBuilder _cityBuilder;
        
        private List<Vector2Int> _clickedLocations = new List<Vector2Int>();

        public void Start() {
            _mainGrid = new Grid(gridSize.x, gridSize.y);
            _inputSolver = GetComponent<InputSolver>();
            _mapInputSolver = new MapInputSolver(ref _inputSolver.OnClickStarted, floorMask);
            _mapInputSolver.OnMapClicked += TryBuildingOnClick;
            _cityBuilder = GetComponent<CityBuilder>();
        }

        private void TryBuildingOnClick(Vector2Int location) {
            // if (CheckClickInBoundaries(location, _cityBuilder.currentSelectedBuild.size)) {
            //     Debug.Log("CanBuildThere");
            // }
        }

        [ContextMenu("Change Build : Blue")]
        public void ChangeSelectedBuildBlue() {
            _cityBuilder.SelectedBuild("Blue");
        }
        
        [ContextMenu("Change Build : Green")]
        public void ChangeSelectedBuildGreen() {
            _cityBuilder.SelectedBuild("Green");
        }

        // private bool CheckClickInBoundaries(Vector2Int location, Vector2Int[] size) {
        //     Debug.Log($"{check.x},{check.y} is Within current grid ? : {_mainGrid?.IsWithinGrid(check)}");
        //     if (_mainGrid?.IsWithinGrid(check) == true) {
        //         if (!_clickedLocations.Contains(check)) {
        //             _clickedLocations.Add(check);
        //         }
        //
        //         return true;
        //     }
        //
        //     return false;
        // }
        
        private bool CheckClickInBoundaries(Vector2Int check) {
            Debug.Log($"{check.x},{check.y} is Within current grid ? : {_mainGrid?.IsWithinGrid(check)}");
            if (_mainGrid?.IsWithinGrid(check) == true) {
                // if (!_clickedLocations.Contains(check)) {
                //     _clickedLocations.Add(check);
                // }

                return true;
            }

            return false;
        }

        private void OnDrawGizmos() {
            if (_mainGrid != null) {
                Gizmos.DrawLine(new Vector3(-0.5f, 1, -0.5f), new Vector3(_mainGrid.Width - 0.5f, 1, -0.5f));
                Gizmos.DrawLine(new Vector3(-0.5f, 1, -0.5f), new Vector3(-0.5f, 1, _mainGrid.Length - 0.5f));
                Gizmos.DrawLine(new Vector3(_mainGrid.Width - 0.5f, 1, -0.5f), new Vector3(_mainGrid.Width - 0.5f, 1, _mainGrid.Length - 0.5f));
                Gizmos.DrawLine(new Vector3(-0.5f, 1, _mainGrid.Length - 0.5f), new Vector3(_mainGrid.Width - 0.5f, 1, _mainGrid.Length - 0.5f));
            }

            if (_cityBuilder) {
                if (!_cityBuilder.currentSelectedBuild) return;
                Vector2Int worldPosition = Vector2Int.zero;
                Vector2 mousePos = Mouse.current.position.ReadValue();
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                if (Physics.Raycast(
                        ray,
                        out RaycastHit hit,
                        Mathf.Infinity,
                        floorMask))
                {
                    worldPosition = new Vector2Int((int)hit.point.x, (int)hit.point.z);
                }
                foreach (var position in _cityBuilder.currentSelectedBuild.size) {
                    Gizmos.color = CheckClickInBoundaries(worldPosition + position)? _cityBuilder.currentSelectedBuild.buildingColor : Color.red;
                    Gizmos.DrawCube(new Vector3(worldPosition.x + position.x, 1, worldPosition.y + position.y), new Vector3(1f, 0.1f, 1f));
                }
            }
            
            foreach (var location in _clickedLocations) {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(new Vector3(location.x, 1, location.y),  new Vector3(1f, 0.1f, 1f));
            }
        }
    }
}
