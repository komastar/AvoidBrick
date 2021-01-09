using UnityEngine;

namespace Komastar.Interface
{
    public interface IBrick
    {
        void SetOwner(GameObject owner);
        void Take(IPlayer player);
    }
}
