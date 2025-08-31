#if ANDROID
using Android.App;
using Android.Content;
using AndroidX.Core.App;
using GlucoMan; // for Alarm if needed in future

namespace GlucoMan.Maui.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    public class AlarmFireReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            int id = intent.GetIntExtra("AlarmId", 0);
            string text = intent.GetStringExtra("ReminderText") ?? "Alarm";

            var builder = new NotificationCompat.Builder(context, "glucoman_alarms")
                .SetContentTitle("GlucoMan")
                .SetContentText(text)
                .SetSmallIcon(global::Android.Resource.Drawable.IcDialogInfo) // Usa la risorsa di sistema Android
                .SetPriority((int)NotificationPriority.High)
                .SetAutoCancel(true);

            var manager = NotificationManagerCompat.From(context);
            manager.Notify(id, builder.Build());
        }
    }
}
#endif
