using System;
using UnityEngine;

namespace MyGrid.Code
{
    public static class Extension
    {
        public static Vector2Int GetCoordinate(this Vector2Int coordinate, Direction direction)
        {
            return direction switch
            {
                Direction.Up => new Vector2Int(coordinate.x, coordinate.y + 1),
                Direction.Down => new Vector2Int(coordinate.x, coordinate.y - 1),
                Direction.Right => new Vector2Int(coordinate.x + 1, coordinate.y),
                Direction.Left => new Vector2Int(coordinate.x - 1, coordinate.y),
                Direction.UpRight => new Vector2Int(coordinate.x + 1, coordinate.y + 1),
                Direction.UpLeft => new Vector2Int(coordinate.x - 1, coordinate.y + 1),
                Direction.DownRight => new Vector2Int(coordinate.x + 1, coordinate.y - 1),
                Direction.DownLeft => new Vector2Int(coordinate.x - 1, coordinate.y - 1),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}