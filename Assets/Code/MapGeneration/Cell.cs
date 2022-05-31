using Code.CharactersGeneration;
using Code.Enums;
using Code.InitDatas;
using Code.Interfaces;
using Plugins.SimpleFactory;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class Cell : IInitialized<CellInitData>
    {
        public CellType Type { get; private set; }
        
        public Vector2Int Position { get; private set; }

        public IDamageable Damageable { get; private set; }

        public bool HasDamageable => Damageable != null;

        public void Initialize(CellInitData initData)
        {
            Type = initData.Type;
            Position = initData.Position;
        }
        
        public void SetType(CellType type)
        {
            Type = type;
        }

        public void SetDamageable(IDamageable damageable)
        {
            Damageable = damageable;
        }
        
    }
}