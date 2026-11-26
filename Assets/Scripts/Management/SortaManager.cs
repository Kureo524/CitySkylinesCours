using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using BuildingSystem;
using Inputs;
using Map;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Management
{
    [RequireComponent(typeof(InputSolver))]
    public class SortaManager : MonoBehaviour
    {
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private List<Vector2Int> gridSizeChecks;

        [SerializeField] private LayerMask floorMask;

        private Grid _mainGrid;
        private InputSolver _inputSolver;
        private MapInputSolver _mapInputSolver;
        private CityBuilder _cityBuilder;
        private MapContainer _map;

        private List<Vector2Int> _clickedLocations = new List<Vector2Int>();

        public void Start()
        {
            _mainGrid = new Grid(gridSize.x, gridSize.y);
            _inputSolver = GetComponent<InputSolver>();
            _mapInputSolver = new MapInputSolver(ref _inputSolver.OnClickStarted, floorMask);
            _mapInputSolver.OnMapClicked += TryBuildingOnClick;
            _cityBuilder = GetComponent<CityBuilder>();
            _map = new MapContainer(gridSize);
        }

        private void TryBuildingOnClick(Vector2Int location)
        {
            if (!_cityBuilder.currentSelectedBuild) return;
            if (CheckBuildingPlacementValid(location, _cityBuilder.currentSelectedBuild.size))
            {
                Debug.Log("CanBuildThere");
                var building = new RuntimeBuilding(_cityBuilder.currentSelectedBuild, location);
                _map.PlaceOnMap(building);
            }
            else
            {
                Debug.Log("CannotBuildThere");
            }
        }

        [ContextMenu("Change Build : Blue")]
        public void ChangeSelectedBuildBlue()
        {
            _cityBuilder.SelectedBuild("Blue");
        }

        [ContextMenu("Change Build : Green")]
        public void ChangeSelectedBuildGreen()
        {
            _cityBuilder.SelectedBuild("Green");
        }

        private bool CheckBuildingPlacementValid(Vector2Int location, Vector2Int[] checks)
        {
            if (!CheckBuildingInBoundaries(location, checks)) return false;
            Debug.Log("Can build in boundaries");
            if (!CheckBuildingValidOnMap(location, checks)) return false;
            Debug.Log("Can build in map");
            return true;
        }

        private bool CheckBuildingValidOnMap(Vector2Int location, Vector2Int[] size)
        {
            if (_map == null) return false;
            foreach (Vector2Int check in size)
            {
                if (!CheckBuildingValidOnMap(location + check)) return false;
            }

            return true;
        }

        private bool CheckBuildingValidOnMap(Vector2Int check)
        {
            if (_map?.IsValidOnMap(check) == true) return true;
            return false;
        }

        private bool CheckBuildingInBoundaries(Vector2Int location, Vector2Int[] size)
        {
            if (size == null || size.Length == 0) return false;
            foreach (Vector2Int check in size)
            {
                if (!CheckBuildingInBoundaries(location + check)) return false;
            }

            return true;
        }

        private bool CheckBuildingInBoundaries(Vector2Int check)
        {
            if (_mainGrid?.IsWithinGrid(check) == true)
            {
                return true;
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            if (_mainGrid != null)
            {
                Gizmos.DrawLine(new Vector3(-0.5f, 1, -0.5f), new Vector3(_mainGrid.Width - 0.5f, 1, -0.5f));
                Gizmos.DrawLine(new Vector3(-0.5f, 1, -0.5f), new Vector3(-0.5f, 1, _mainGrid.Length - 0.5f));
                Gizmos.DrawLine(new Vector3(_mainGrid.Width - 0.5f, 1, -0.5f),
                    new Vector3(_mainGrid.Width - 0.5f, 1, _mainGrid.Length - 0.5f));
                Gizmos.DrawLine(new Vector3(-0.5f, 1, _mainGrid.Length - 0.5f),
                    new Vector3(_mainGrid.Width - 0.5f, 1, _mainGrid.Length - 0.5f));
            }

            if (_cityBuilder)
            {
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

                foreach (var position in _cityBuilder.currentSelectedBuild.size)
                {
                    Gizmos.color =
                        (CheckBuildingInBoundaries(worldPosition + position) &&
                         CheckBuildingValidOnMap(position + worldPosition))
                            ? _cityBuilder.currentSelectedBuild.buildingColor
                            : Color.red;
                    Gizmos.DrawCube(new Vector3(worldPosition.x + position.x, 0.2f, worldPosition.y + position.y),
                        new Vector3(1f, 0.1f, 1f));
                }
            }

            if (_map != null)
            {
                for (int i = 0; i < _map.MapX; i++)
                {
                    for (int j = 0; j < _map.MapY; j++)
                    {
                        MapContainer.MapNode node = _map.Map[i, j];
                        if (node != null && node.LinkedBuilding != null)
                        {
                            Gizmos.color = node.LinkedBuilding.BuildData.buildingColor;
                            Gizmos.DrawCube(new Vector3(i, 0.1f, j), new Vector3(1f, 0.1f, 1f));
                        }
                    }
                }
            }
        }
    }
}