using System;

namespace FTS.Tools.Utilities
{
    public abstract class Timer
    {
        protected readonly float initialTime;
        protected float Time { get; set; }
        protected bool IsRunning { get; private set; }
        public float Progress => Time / initialTime;
        public readonly Action OnTimeStart = delegate { };
        public Action OnTimeStop = delegate { };

        protected Timer(float value)
        {
            initialTime = value;
            IsRunning = false;
        }

        public void Start()
        {
            Time = initialTime;
            if (IsRunning) 
                return;
            
            IsRunning = true;
            OnTimeStart.Invoke();
        }

        public void Stop()
        {
            if (!IsRunning) 
                return;
            
            IsRunning = false;
            OnTimeStop?.Invoke();
        }

        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;

        public abstract void Tick(float deltaTime);
    }
    
    public class CountdownTimer : Timer
    {
        public CountdownTimer(float value) : base(value) { }

        public override void Tick(float deltaTime)
        {
            if (IsRunning && Time > 0)
                Time -= deltaTime;
            
            if (IsRunning && Time <= 0)
                Stop();
        }

        public bool IsFinished => Time <= 0;
        public void Reset() => Time = initialTime;
        public void Reset(float newTime)
        {
            Time = newTime;
            Reset();
        }
    }
}