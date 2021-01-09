using Komastar.Interface;

namespace Komastar.Combat
{
    public class Damagable : IDamagable
    {
        public int Hp;

        public int GetHp()
        {
            return Hp;
        }

        public void SetHp(int hp)
        {
            Hp = hp;
        }

        public void TakeDamage(int damage)
        {
            Hp -= damage;
        }
    }
}
