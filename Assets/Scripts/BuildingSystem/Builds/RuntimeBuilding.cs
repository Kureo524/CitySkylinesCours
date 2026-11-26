using UnityEngine;

public class RuntimeBuilding {
    public BuildDataSO BuildData { get; private set; }
    public Vector2Int Location { get; }
    public GameObject MapObject { get; }

    public RuntimeBuilding(BuildDataSO baseData, Vector2Int location, GameObject mapObject = null) {
        BuildData = baseData;
        Location = location;
        MapObject = mapObject;
    }
}