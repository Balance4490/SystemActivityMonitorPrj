using SystemActivityMonitor.Data.Patterns.AbstractFactory;
namespace SystemActivityMonitor.Data.Patterns.Command
{
    public class GenerateDataCommand : ICommand
    {
        private readonly SystemController _receiver;
        private readonly IMonitorFactory _factory;

        public GenerateDataCommand(SystemController receiver, IMonitorFactory factory)
        {
            _receiver = receiver;
            _factory = factory;
        }

        public void Execute()
        {
            _receiver.GenerateDataWithFactory(_factory);
        }
    }
}