using System.Collections.Generic;

namespace SystemActivityMonitor.Data.Patterns.Command
{
    public class CommandInvoker
    {
        private ICommand _command;
        
        private List<ICommand> _history = new List<ICommand>();

        public void SetCommand(ICommand command)
        {
            _command = command;
        }

        public void Run()
        {
            if (_command != null)
            {
                _command.Execute();
                _history.Add(_command);
            }
        }
    }
}