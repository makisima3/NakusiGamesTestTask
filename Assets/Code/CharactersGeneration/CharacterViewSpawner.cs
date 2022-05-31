using System.Collections.Generic;
using Code.Extensions;
using Code.InitDatas;
using Code.Interfaces;
using DefaultNamespace;
using Plugins.SimpleFactory;
using UnityEngine;

namespace Code.CharactersGeneration
{
    public class CharacterViewSpawner : ISpawner
    {
        public Grid Grid { get; set; }
        
        public SimpleFactory Factory { get; set; }
        public List<ICharacter> Characters { get; set; }

        public void Spawn(Cell[,] cells)
        {
            foreach (var character in Characters)
            {
                if (character is SimpleCharacter simpleCharacter)
                {
                    var view = Factory.Create<SimpleCharacterView, SimpleCharacterViewInitData>(
                        new SimpleCharacterViewInitData()
                        {
                            Character = simpleCharacter,
                            WorldPosition = this.GetWorldPosition((Vector3Int) simpleCharacter.Position)
                        });
                    
                    cells[simpleCharacter.Position.x, simpleCharacter.Position.y].SetDamageable(view);
                }
            }

        }

        public void StopSpawn()
        {
            
        }
    }
}