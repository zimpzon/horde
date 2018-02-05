using UnityEngine;

namespace HordeEngine
{
    public class GameStateHub : GameStateHandler
    {
        MapData hubData_;

        public override bool TryEnterState()
        {
            return true;
        }
    }
}
