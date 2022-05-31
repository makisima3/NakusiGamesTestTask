namespace Code.Interfaces
{
    public interface IDamageable
    {
        bool IsAlive { get; set; }
        int Health { get; set; }
        void TakeDamage(int damage);
        void Death();
    }
}