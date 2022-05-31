using UnityEngine;
using UnityEngine.Events;

namespace Code.Interfaces
{
    public interface IBomb
    {
        Vector2Int Position { get; }
        int Damage { get;  }
        int Radius { get;  }
        
        float TimeToExplode { get; }
    }
}