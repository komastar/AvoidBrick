using UnityEngine;

namespace Komastar.Interface
{
    public interface IBrick
    {
        void SetUp(GameObject owner);
        void Take(IPlayer player);
    }
}
