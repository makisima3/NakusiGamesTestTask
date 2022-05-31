using UnityEngine;

namespace Code.CharactersGeneration
{
    public class SimpleCharacter : ICharacter
    {
        public bool IsAlive { get; set; }
        public int Health { get; set; }
        public Vector2Int Position { get; set; }

        public SimpleCharacter()
        {
            IsAlive = true;
        }
    }
}