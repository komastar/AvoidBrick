using Komastar.Interface;
using UnityEngine;

namespace Komastar.Factory
{
    public enum EBrickType
    {
        Damage,
        Slow,
        Count
    }

    public class BrickFactory
    {
        public static IBrick GetBrick(EBrickType brickType, GameObject owner)
        {
            IBrick brick;
            switch (brickType)
            {
                case EBrickType.Damage:
                    brick = new DamageBrick();
                    break;
                case EBrickType.Slow:
                    brick = new SlowBrick();
                    break;
                default:
                    brick = new NullBrick();
                    break;
            }

            brick.SetOwner(owner);

            return brick;
        }
    }
}
