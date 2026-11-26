using UnityEngine;

public class Grid {
    public int Width { get; }
    public int Length { get; }

    public Grid(int width = 10, int length = 10) {
        Width = width;
        Length = length;
    }
    
    public bool IsWithinGrid(int x, int y) {
        return x >= 0 && x < Width && y >= 0 && y < Length;
    }
    
    public bool IsWithinGrid(Vector2Int pos) {
        return IsWithinGrid(pos.x, pos.y);
    }
}
