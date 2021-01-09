using Komastar.Interface;
using UnityEngine;

namespace Komastar.Factory
{
    public class SlowBrick : Brick
    {
        public override void Take(IPlayer player)
        {
            Debug.Log("Slow Take");
            BrickGenerator.Instance.SlowBrick();
            ownObject.SetActive(false);
        }
    }
}