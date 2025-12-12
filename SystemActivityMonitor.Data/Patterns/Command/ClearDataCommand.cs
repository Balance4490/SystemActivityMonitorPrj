namespace SystemActivityMonitor.Data.Patterns.Command
{
    public class ClearDataCommand : ICommand
    {
        private readonly SystemController _receiver;

        public ClearDataCommand(SystemController receiver)
        {
            _receiver = receiver;
        }

        public void Execute()
        {
            _receiver.ClearAllData();
        }
    }
}