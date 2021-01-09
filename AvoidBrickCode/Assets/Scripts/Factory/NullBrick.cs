using Komastar.Interface;
using UnityEngine;

namespace Komastar.Factory
{
    public abstract class Brick : IBrick
    {
        protected GameObject ownObject;
        public virtual void SetOwner(GameObject owner)
        {
            ownObject = owner;
        }

        public abstract void Take(IPlayer player);
    }

    public class NullBrick : Brick
    {
        public override void Take(IPlayer player)
        {
            //  DO NOTHING
        }
    }
}
