using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem {
    public class CityBuilder : MonoBehaviour {
        public List<BuildDataSO> availableBuilds = new List<BuildDataSO>();
        public BuildDataSO currentSelectedBuild;

        public void SelectedBuild(string buildName) {
            foreach (var build in availableBuilds) {
                if (build.buildingName == buildName) {
                    currentSelectedBuild = build;
                    break;
                }
            }
        }
    }
}
