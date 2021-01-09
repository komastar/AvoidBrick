namespace Komastar.Interface
{
    public interface IPlayer
    {
        IDamagable GetDamagable();
    }

    public interface IDamagable
    {
        void SetHp(int hp);
        int GetHp();
        void TakeDamage(int damage);
    }
}
