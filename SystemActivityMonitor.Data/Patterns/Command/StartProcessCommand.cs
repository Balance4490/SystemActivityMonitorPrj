using SystemActivityMonitor.Data.Patterns.Command;

namespace SystemActivityMonitor.Data.Patterns.Command
{
    public class StartProcessCommand : ICommand
    {
        private SystemController _controller;
        private string _name;
        private float _cpu;
        private float _ram;

        public StartProcessCommand(SystemController controller, string name, float cpu, float ram)
        {
            _controller = controller;
            _name = name;
            _cpu = cpu;
            _ram = ram;
        }

        public void Execute()
        {
            _controller.StartProcess(_name, (int)_cpu, (int)_ram);
        }
    }
}