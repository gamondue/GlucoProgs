using gamon;

namespace GlucoMan.BusinessLayer
{
    /// <summary>
    /// Business-layer facade for working with PhysicalActivity records.
    /// Wraps the DataLayer to provide simple CRUD helpers plus a few
    /// compatibility methods that the current UI expects.
    /// </summary>
    public class BL_PhysicalActivity
    {
        private readonly DataLayer dl = DatabaseService.Instance.Database;

        public string StatusMessage { get; private set; } = string.Empty;

        /// <summary>
        /// Returns all activities within the optional time window.
        /// </summary>
        public List<PhysicalActivity> GetPhysicalActivities(DateTime? initial = null, DateTime? final = null)
        {
            try
            {
                return dl.GetPhysicalActivities(initial, final) ?? new List<PhysicalActivity>();
            }
            catch (Exception ex)
            {
                StatusMessage = "Failed to read physical activities";
                General.LogOfProgram?.Error("BL_PhysicalActivity - GetPhysicalActivities", ex);
                return new List<PhysicalActivity>();
            }
        }

        /// <summary>
        /// Compatibility helper for legacy callers that still pass injection-related filters.
        /// Only the date range is honored for now.
        /// </summary>
        public List<PhysicalActivity> GetActivities(
            DateTime? initial,
            DateTime? final,
            Common.TypeOfInsulinAction _ = Common.TypeOfInsulinAction.NotSet,
            Common.ZoneOfPosition __ = Common.ZoneOfPosition.NotSet,
            bool ___ = false,
            bool ____ = false,
            bool _____ = false,
            bool ______ = false)
        {
            return GetPhysicalActivities(initial, final);
        }

        /// <summary>
        /// Returns a single activity or null if not found.
        /// </summary>
        public PhysicalActivity? GetOnePhysicalActivity(int? idActivity)
        {
            try
            {
                return dl.GetOnePhysicalActivity(idActivity);
            }
            catch (Exception ex)
            {
                StatusMessage = "Failed to read the selected activity";
                General.LogOfProgram?.Error("BL_PhysicalActivity - GetOnePhysicalActivity", ex);
                return null;
            }
        }

        /// <summary>
        /// Persists an activity, assigning defaults when needed.
        /// </summary> 
        public int? SaveOnePhysicalActivity(PhysicalActivity activity)
        {
            try
            {
                if (activity == null)
                    return null;

                activity.EventTime ??= DateTime.Now;
                activity.ActivityLevel ??= 1;
                activity.DurationMinutes ??= 0;

                var result = dl.SaveOnePhysicalActivity(activity);
                StatusMessage = result.HasValue ? "Activity saved" : "Unable to save activity";
                return result;
            }
            catch (Exception ex)
            {
                StatusMessage = "Failed to save activity";
                General.LogOfProgram?.Error("BL_PhysicalActivity - SaveOnePhysicalActivity", ex);
                return null;
            }
        }

        /// <summary>
        /// Compatibility alias kept for the current UI layer.
        /// </summary>
        public int? SaveOneActivity(PhysicalActivity activity) => SaveOnePhysicalActivity(activity);

        /// <summary>
        /// Deletes the provided activity.
        /// </summary>
        public bool DeleteOnePhysicalActivity(PhysicalActivity activity)
        {
            try
            {
                if (activity?.IdActivity == null)
                    return false;

                dl.DeleteOnePhysicalActivity(activity);
                StatusMessage = "Activity deleted";
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = "Failed to delete activity";
                General.LogOfProgram?.Error("BL_PhysicalActivity - DeleteOnePhysicalActivity", ex);
                return false;
            }
        }

        /// <summary>
        /// Compatibility alias used by the UI.
        /// </summary>
        public bool DeleteOneActivity(PhysicalActivity activity) => DeleteOnePhysicalActivity(activity);
    }
}
