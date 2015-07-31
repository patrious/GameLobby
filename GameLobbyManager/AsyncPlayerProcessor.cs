using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GameLobby
{
    internal abstract class AsyncPlayerProcessor
    {
        readonly BufferBlock<PlayerGroup> _playerBuffer = new BufferBlock<PlayerGroup>();

        protected async Task PlayerAsyncProcessor()
        {
            while (await _playerBuffer.OutputAvailableAsync())
            {
                var playerGroup = _playerBuffer.Receive();
                ProcessPlayerGroup(playerGroup);
            }
        }

        protected abstract void ProcessPlayerGroup(PlayerGroup playerGroup);


        protected void AddPlayerToProcessing(PlayerGroup playerGroup)
        {
            _playerBuffer.Post(playerGroup);
        }

        protected void StopProcessing()
        {
            _playerBuffer.Complete();
        }
    }
}
