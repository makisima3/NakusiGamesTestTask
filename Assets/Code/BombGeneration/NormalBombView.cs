using Code.InitDatas;
using Code.Interfaces;
using DG.Tweening;
using Plugins.SimpleFactory;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Code.BombGeneration
{
    public class NormalBombView : MonoBehaviour, IBombView, IInitialized<NormalBombViewInitData>
    {
        [SerializeField] private float heightOffset  = 5f;
        
        private Vector3 _fallPosition;
        public IBomb Bomb { get; private set; }
        public UnityEvent<IBomb> OnExplode { get; private set; }
        
        private NormalBomb NormalBomb => Bomb as NormalBomb;
        
        
        public void Initialize(NormalBombViewInitData initData)
        {
            OnExplode = new UnityEvent<IBomb>();
            Bomb = initData.Bomb;

            _fallPosition = initData.FallPosition + Vector3.up;

            transform.position = _fallPosition + Vector3.up * heightOffset;
        }

        public void Fire()
        {
            transform.DOMove(_fallPosition, Bomb.TimeToExplode)
                .OnComplete(() =>
                {
                    OnExplode.Invoke(Bomb);
                    
                    Destroy(gameObject);
                });
        }

    }
}