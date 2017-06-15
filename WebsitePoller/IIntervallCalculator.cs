using NodaTime;

namespace WebsitePoller
{
    public interface IIntervallCalculator
    {
        Duration CalculateDurationTillIntervall();
    }
}