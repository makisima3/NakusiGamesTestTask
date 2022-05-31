using UnityEngine.Events;

namespace Code.Interfaces
{
    public interface IBombView
    {
        IBomb Bomb { get; }
        UnityEvent<IBomb> OnExplode { get; }
        
        void Fire();
    }
}