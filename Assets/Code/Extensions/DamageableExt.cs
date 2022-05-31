using Code.Interfaces;

namespace Code.Extensions
{
    public static class DamageableExt
    {
        public static void TakeDamage(this IDamageable damageable, int damage)
        {
            if (!damageable.IsAlive)
                return;

            damageable.Health -= damage;

            if (damageable.Health <= 0)
                damageable.Death();
        }

        public static void Death(this IDamageable damageable)
        {
            damageable.IsAlive = false;
        }
    }
}