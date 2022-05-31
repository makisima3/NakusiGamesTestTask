using System;
using System.Collections.Generic;
using System.Linq;
using Code.Enums;
using Code.Factories;
using Code.InitDatas;
using Code.Interfaces;
using DefaultNamespace;
using Mono.CompilerServices.SymbolWriter;
using Plugins.RobyyUtils;
using Random = UnityEngine.Random;

namespace Code.CharactersGeneration
{
    public class SimpleCharactersGenerator : ICharacterGenerator
    {
        public List<ICharacter> Generate(Cell[,] cells, int count)
        {
            var characters = new List<ICharacter>();

            var charactersPlaces = cells.Cast<Cell>().ToList()
                .Where(cell => cell.Type == CellType.Empty && !cell.HasDamageable).ChooseManyUnique(count).ToArray();

            for (int i = 0; i < count; i++)
            {
                var cell = charactersPlaces[i];

                var character = new SimpleCharacter()
                {
                    Health = 10,
                    Position = cell.Position,
                };
                
                characters.Add(character);
            }

            return characters;
        }
    }
}