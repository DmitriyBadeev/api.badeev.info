using System.Timers;

namespace Portfolio.Finance.API.Services.Interfaces
{
    public interface ITimerService
    {
        string Subscribe(ElapsedEventHandler handler);
        void Unsubscribe(string handlerId);
        void Reload();
    }
}