using Komastar.Interface;
using UnityEngine;

namespace Komastar.Factory
{
    public class DamageBrick : Brick
    {
        public override void SetUp(GameObject owner)
        {
            base.SetUp(owner);
            owner.layer = LayerMask.NameToLayer("Brick");
            owner.tag = "Brick";
        }

        public override void Take(IPlayer player)
        {
            player.GetDamagable().TakeDamage(1);
        }
    }
}