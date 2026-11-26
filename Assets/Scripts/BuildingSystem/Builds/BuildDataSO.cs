using UnityEngine;

[CreateAssetMenu(fileName = "BuildData", menuName = "ScriptableObjects/BuildData", order = 1)]
public class BuildDataSO : ScriptableObject
{
    public string buildingName;
    public Vector2Int[] size;
    public Color buildingColor;
}