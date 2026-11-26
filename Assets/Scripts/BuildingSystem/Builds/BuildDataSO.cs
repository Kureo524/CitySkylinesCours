using System.Collections.Generic;
using Currencies;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildData", menuName = "ScriptableObjects/BuildData", order = 1)]
public class BuildDataSO : ScriptableObject {

    public struct FCurrencyData {
        public ECurrencyType type;
        public int amount;
    }
    
    
    public string buildingName;
    public Vector2Int[] size;
    public Color buildingColor;
    public List<FCurrencyData> costs;
}