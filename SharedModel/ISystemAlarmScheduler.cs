using System.Threading.Tasks;

namespace GlucoMan
{
    public interface ISystemAlarmScheduler
    {
        Task ScheduleAsync(Alarm alarm);
        Task CancelAsync(int idAlarm);
        Task CancelAllAsync();
    }
}
