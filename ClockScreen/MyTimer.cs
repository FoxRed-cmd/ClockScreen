using System.Threading.Tasks;
using System;

internal class MyTimer
{
    private bool _enabled;
    private Task _task;
    public double Interval { get; set; }
    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            if (_enabled == true)
            {
                _task = new Task(Tick);
                _task.Start();
            }

        }
    }
    public event EventHandler<EventArgs>? Elapsed;
    public MyTimer()
    {
        Interval = 1000;
        Enabled = false;
        _task = new Task(Tick);
    }
    public void Start() => Enabled = true;
    public void Stop() => Enabled = false;
    private async void Tick()
    {
        while (Enabled)
        {
            Elapsed?.Invoke(this, EventArgs.Empty);
            await Task.Delay((int)Interval);
        }
    }
}