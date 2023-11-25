using DataBreeze.Application.Interfaces;

namespace DataBreeze.Application.Services;

public class RegistrationMonitor : IRegistrationMonitor
{
    private bool _isRegistered;
    
    public bool IsRegistered()
    {
        return _isRegistered;
    }

    public void SetRegistered()
    {
        _isRegistered = true;
    }
}