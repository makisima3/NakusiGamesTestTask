using Code.Enums;
using Code.InitDatas;
using Plugins.SimpleFactory;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class Cell : IInitialized<CellInitData>
    {
        public CellType Type { get; private set; }
        
        public Vector2Int Position { get; private set; }

        public void Initialize(CellInitData initData)
        {
            Type = initData.Type;
            Position = initData.Position;
        }
        
        public void SetType(CellType type)
        {
            Type = type;
        }

    }
}