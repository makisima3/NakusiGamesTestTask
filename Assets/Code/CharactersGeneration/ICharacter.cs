using UnityEngine;

namespace Code.CharactersGeneration
{
    public interface ICharacter
    {
        bool IsAlive { get; set; }
        int Health { get; set; }

        Vector2Int Position { get; set; }

    }
}