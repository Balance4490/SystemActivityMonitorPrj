namespace SystemActivityMonitor.Data.Patterns.Command
{
    public class GenerateDataCommand : ICommand
    {
        private readonly SystemController _receiver;

        public GenerateDataCommand(SystemController receiver)
        {
            _receiver = receiver;
        }

        public void Execute()
        {
            _receiver.GenerateTestData();
        }
    }
}