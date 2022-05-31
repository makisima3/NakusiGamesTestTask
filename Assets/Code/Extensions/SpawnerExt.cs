using Code.Interfaces;
using UnityEngine;

namespace Code.Extensions
{
    public static class SpawnerExt
    {
        public static Vector3 GetWorldPosition(this ISpawner spawner, Vector3Int gridPosition)
        {
            return spawner.Grid.CellToWorld(gridPosition);
        }
    }
}