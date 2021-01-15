using Komastar.Interface;
using UnityEngine;

namespace Komastar.Factory
{
    public class SlowBrick : Brick
    {
        public override void SetUp(GameObject owner)
        {
            base.SetUp(owner);
            owner.layer = LayerMask.NameToLayer("Item");
            owner.tag = "Item";
        }

        public override void Take(IPlayer player)
        {
            Debug.Log("Slow Take");
            BrickGenerator.Instance.SlowBrick();
            ownObject.SetActive(false);
            PlayerObject.Score++;
        }
    }
}