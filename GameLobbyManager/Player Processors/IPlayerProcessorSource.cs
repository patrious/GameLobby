using System.Collections.Generic;

namespace GameLobby
{
    interface IPlayerProcessorSource
    {
        IEnumerable<BasePlayerProcessor> GetProcessorList();
    }
}
