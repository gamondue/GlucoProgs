using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using GlucoMan;

namespace TestGlucoMan
{
    [TestFixture]
    public class AlarmSchedulerTests
    {
        // Test di integrazione: richiede un device/emulatore Android avviato.
        [Test]
        public async Task Alarm_Fires_Within_Expected_Timeframe()
        {
#if ANDROID
            var scheduler = new SystemAlarmScheduler();

            var alarm = new Alarm
            {
                IdAlarm = Random.Shared.Next(100000, 999999),
                ReminderText = "Test Alarm",
                TimeStart = new DateTimeAndText { DateTime = DateTime.Now },
                // Impostiamo il prossimo trigger direttamente tra 20 secondi
                NextTriggerTime = DateTime.Now.AddSeconds(20),
                TriggerInterval = TimeSpan.FromSeconds(20),
                Interval = null
            };

            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            using var reg = AlarmFireReceiverTestProbe.Register(alarm.IdAlarm.Value, tcs);

            await scheduler.ScheduleAsync(alarm);

            // Timeout di sicurezza (35s)
            var completed = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(35)));
            Assert.IsTrue(tcs.Task.IsCompleted, "L'allarme non è stato ricevuto entro il tempo previsto.");
#else
            Assert.Inconclusive("Questo test è valido solo su Android.");
#endif
        }
    }
}