using System;

namespace SystemActivityMonitor.Data.Patterns.Command
{
    public class KillProcessCommand : ICommand
    {
        private SystemController _controller;
        private Guid _processId;

        public KillProcessCommand(SystemController controller, Guid processId)
        {
            _controller = controller;
            _processId = processId;
        }

        public void Execute()
        {
            _controller.KillProcess(_processId);
        }
    }
}