using System;
using SystemActivityMonitor.Data.Patterns.Visitor;

namespace SystemActivityMonitor.Data.Processes
{
    public class VirtualProcess
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; }
        private float _baseCpu;
        private float _baseRam;
        private int _intensityLevel = 1;
        private IProcessState _state;

        public VirtualProcess(string name, int baseCpu, int ram)
        {
            Name = name;
            _baseCpu = baseCpu;
            _baseRam = ram;
            _state = new RunningState();
        }

        public VirtualProcess(Guid id, string name, IProcessState initialState, float baseCpu, float baseRam)
        {
            Id = id;
            Name = name;
            _state = initialState;
            _baseCpu = baseCpu;
            _baseRam = baseRam;
        }

        public void SetState(IProcessState state)
        {
            _state = state;
        }

        public string GetStatus()
        {
            return _state.GetStatusName();
        }

        public void IncreaseLoad()
        {
            _intensityLevel++;
        }

        public void DecreaseLoad()
        {
            if (_intensityLevel > 1)
            {
                _intensityLevel--;
            }
        }

        public float GetCurrentCpuUsage()
        {
            float multiplier = 1.0f + (_intensityLevel - 1) * 0.1f;
            return _state.CalculateCpuUsage((int)_baseCpu) * multiplier;
        }

        public float GetCurrentRamUsage()
        {
            if (_state is TerminatedState) return 0;
            float extraRam = (_intensityLevel - 1) * 100.0f;
            return _baseRam + extraRam;
        }

        public void Accept(IProcessVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}