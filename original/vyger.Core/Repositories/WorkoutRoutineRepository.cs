using vyger.Core.Models;

namespace vyger.Core.Repositories
{
    public interface IWorkoutRoutineRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        WorkoutRoutineCollection GetWorkoutRoutines();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routines"></param>
        void SaveWorkoutRoutines(WorkoutRoutineCollection routines);
    }

    class WorkoutRoutineRepository : BaseRepository, IWorkoutRoutineRepository
    {
        #region Constructors

        public WorkoutRoutineRepository(ISecurityActor actor) : base(actor)
        {

        }

        #endregion

        #region Interface

        public WorkoutRoutineCollection GetWorkoutRoutines()
        {
            return ReadData<WorkoutRoutineCollection>();
        }

        public void SaveWorkoutRoutines(WorkoutRoutineCollection routines)
        {
            SaveData(routines);
        }

        #endregion

        #region Properties

        protected override string FileName
        {
            get
            {
                return typeof(WorkoutRoutine).Name;
            }
        }

        #endregion
    }
}
