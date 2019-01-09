using vyger.Core.Models;

namespace vyger.Core.Repositories
{
    public interface IWorkoutLogRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        WorkoutLogCollection GetWorkoutLogs();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logs"></param>
        void SaveWorkoutLogs(WorkoutLogCollection logs);
    }

    class WorkoutLogRepository : BaseRepository, IWorkoutLogRepository
    {
        #region Constructors

        public WorkoutLogRepository(ISecurityActor actor) : base(actor)
        {

        }

        #endregion

        #region Interface

        public WorkoutLogCollection GetWorkoutLogs()
        {
            return ReadData<WorkoutLogCollection>();
        }

        public void SaveWorkoutLogs(WorkoutLogCollection logs)
        {
            SaveData(logs);
        }

        #endregion

        #region Properties

        protected override string FileName
        {
            get
            {
                return typeof(WorkoutLog).Name;
            }
        }

        #endregion
    }
}
