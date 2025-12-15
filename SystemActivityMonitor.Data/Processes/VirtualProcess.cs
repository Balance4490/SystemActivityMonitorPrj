using System;

namespace SystemActivityMonitor.Data.Processes
{
    public class VirtualProcess
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; }
        public int BaseCpuCost { get; set; } 
        public int RamCostMb { get; set; }
        
        private IProcessState _state; 

        public VirtualProcess(string name, int baseCpu, int ram)
        {
            Name = name;
            BaseCpuCost = baseCpu;
            RamCostMb = ram;
            _state = new RunningState(); 
        }

        public void SetState(IProcessState state)
        {
            _state = state;
        }

        public string GetStatus()
        {
            return _state.GetStatusName();
        }

        public float GetCurrentCpuUsage()
        {
            return _state.CalculateCpuUsage(BaseCpuCost);
        }

        public float GetCurrentRamUsage()
        {
            if (_state is TerminatedState) return 0;
            return RamCostMb;
        }
    }
}