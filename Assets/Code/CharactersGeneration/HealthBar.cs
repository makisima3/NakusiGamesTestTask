using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.CharactersGeneration
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image health;
        [SerializeField] private Image delta;
        [SerializeField] private float timeToDeltaFill;

        private int _maxHealth;
        private float _fillAmountStep;
        
        public void SetHealth(int maxHealth)
        {
            _maxHealth = maxHealth;
            _fillAmountStep = 1f / _maxHealth;

            health.fillAmount = 1f;
            delta.fillAmount = 1f;
        }

        public void ShowDamage(int damage)
        {
            health.fillAmount -= _fillAmountStep * damage;
            delta.DOFillAmount(health.fillAmount, timeToDeltaFill);
        }
    }
}