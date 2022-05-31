using DefaultNamespace;
using UnityEngine;

namespace Code.Interfaces
{
    public interface ISpawner
    {
        Grid Grid { get; }
        
        void Spawn(Cell[,] cells );

        void StopSpawn();
    }
}