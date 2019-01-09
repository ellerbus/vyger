using vyger.Core.Models;

namespace vyger.Core.Repositories
{
    public interface IExerciseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ExerciseCollection GetExercises();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exercises"></param>
        void SaveExercises(ExerciseCollection exercises);
    }

    class ExerciseRepository : BaseRepository, IExerciseRepository
    {
        #region Constructors

        public ExerciseRepository(ISecurityActor actor) : base(actor)
        {

        }

        #endregion

        #region Interface

        public ExerciseCollection GetExercises()
        {
            return ReadData<ExerciseCollection>();
        }

        public void SaveExercises(ExerciseCollection exercises)
        {
            SaveData(exercises);
        }

        #endregion

        #region Properties

        protected override string FileName
        {
            get
            {
                return typeof(Exercise).Name;
            }
        }

        #endregion
    }
}
