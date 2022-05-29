using System;
using Code.InitDatas;
using Plugins.SimpleFactory;
using UnityEngine;

namespace Code.MapGeneration
{
    public class CellView : MonoBehaviour, IInitialized<CellViewInitData>
    {
       
        public void Initialize(CellViewInitData initData)
        {
            GetComponent<MeshRenderer>().material.color = initData.Color;
        }
    }
}