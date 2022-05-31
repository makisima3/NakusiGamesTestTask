using System.Collections.Generic;
using Code.CharactersGeneration;
using DefaultNamespace;

namespace Code.Interfaces
{
    public interface ICharacterGenerator
    {
        List<ICharacter> Generate(Cell[,] cells, int count);
    }
}