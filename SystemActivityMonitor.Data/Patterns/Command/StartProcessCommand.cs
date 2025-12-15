using SystemActivityMonitor.Data.Patterns.Command;

namespace SystemActivityMonitor.Data.Patterns.Command
{
    public class StartProcessCommand : ICommand
    {
        private SystemController _controller;
        private string _name;
        private int _cpu;
        private int _ram;

        public StartProcessCommand(SystemController controller, string name, int cpu, int ram)
        {
            _controller = controller;
            _name = name;
            _cpu = cpu;
            _ram = ram;
        }

        public void Execute()
        {
            _controller.StartProcess(_name, _cpu, _ram);
        }
    }
}