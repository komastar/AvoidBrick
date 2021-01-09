using Komastar.Interface;

namespace Komastar.Factory
{
    public class DamageBrick : Brick
    {
        public override void Take(IPlayer player)
        {
            player.GetDamagable().TakeDamage(1);
        }
    }
}