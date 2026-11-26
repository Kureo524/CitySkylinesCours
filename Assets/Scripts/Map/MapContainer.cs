using UnityEngine;

namespace Map {
    public class MapContainer {
        public class MapNode {
            public RuntimeBuilding LinkedBuilding;
        }
        
        public MapNode[,] Map { get; }
        public int MapX { get; private set; }
        public int MapY { get; private set; }

        public MapContainer(Vector2Int mapSize) {
            Map = new MapNode[mapSize.x, mapSize.y];
            MapX = mapSize.x;
            MapY = mapSize.y;
        }

        public void PlaceOnMap(RuntimeBuilding building) {
            foreach (var size in building.BuildData.size) {
                MapNode mapNode = new MapNode();
                mapNode.LinkedBuilding = building;
                Map[building.Location.x + size.x, building.Location.y + size.y] = mapNode;
            }
        }

        public bool IsValidOnMap(Vector2Int location) {
            if(MapX <= location.x || MapY <= location.y || location.x < 0 || location.y < 0) {
                return false;
            }
            if (Map[location.x, location.y] != null && Map[location.x, location.y].LinkedBuilding != null) return false;
            return true;
        }
    }
}
