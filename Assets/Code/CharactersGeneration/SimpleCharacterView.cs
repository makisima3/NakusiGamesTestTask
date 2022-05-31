using Code.Extensions;
using Code.InitDatas;
using Code.Interfaces;
using DG.Tweening;
using Plugins.SimpleFactory;
using UnityEngine;

namespace Code.CharactersGeneration
{
    public class SimpleCharacterView : MonoBehaviour, IDamageable ,IInitialized<SimpleCharacterViewInitData>
    {
        [SerializeField] private Vector3 attachedDeathScale = Vector3.one * 0.2f;
        [SerializeField] private float deathAnimDuration = 0.2f;
        [SerializeField] protected HealthBar healthBar;

        private ICharacter _character;
        
        public void Initialize(SimpleCharacterViewInitData initData)
        {
            transform.position = initData.WorldPosition + Vector3.up;
            _character = initData.Character;
            
            healthBar.SetHealth(initData.Character.Health);
        }

        public bool IsAlive
        {
            get => _character.IsAlive;
            set => _character.IsAlive = value;
        }

        public int Health
        {
            get => _character.Health;
            set => _character.Health = value;
        }

        public void TakeDamage(int damage)
        {
            DamageableExt.TakeDamage(this, damage);
            healthBar.ShowDamage(damage);
        }

        public void Death()
        {
            DamageableExt.Death(this);
            DOTween.Sequence()
              .Append(transform.DOScale(transform.localScale + attachedDeathScale, deathAnimDuration))
              .Append(transform.DOScale(Vector3.zero, deathAnimDuration))
              .OnComplete(() =>
              {
                  Destroy(gameObject);
              });
        }

       
    }
}