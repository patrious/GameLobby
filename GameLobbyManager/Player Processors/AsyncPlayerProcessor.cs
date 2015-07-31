using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GameLobby
{
    internal abstract class AsyncPlayerProcessor
    {
        readonly BufferBlock<BasePlayerGroup> _playerBuffer = new BufferBlock<BasePlayerGroup>();

        protected async Task PlayerAsyncProcessor()
        {
            while (await _playerBuffer.OutputAvailableAsync())
            {
                var playerGroup = _playerBuffer.Receive();
                ProcessPlayerGroup(playerGroup);
            }
        }

        protected abstract void ProcessPlayerGroup(BasePlayerGroup basePlayerGroup);


        protected void AddPlayerToProcessing(BasePlayerGroup basePlayerGroup)
        {
            _playerBuffer.Post(basePlayerGroup);
        }

        protected void StopProcessing()
        {
            _playerBuffer.Complete();
        }
    }
}
