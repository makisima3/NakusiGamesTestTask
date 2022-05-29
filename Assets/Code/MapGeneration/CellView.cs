using System;
using System.Linq;
using Code.Enums;
using Code.InitDatas;
using Plugins.SimpleFactory;
using UnityEngine;

namespace Code.MapGeneration
{
    public class CellView : MonoBehaviour, IInitialized<CellViewInitData>
    {
        [Serializable]
        private class Pair
        {
            public CellType type;
            public Material material;
        }

        [SerializeField] private Pair[] _pairs;
       
        public void Initialize(CellViewInitData initData)
        {
            GetComponent<MeshRenderer>().sharedMaterial = _pairs.First(p => p.type == initData.Cell.Type).material;
        }
    }
}