using System.Collections.Generic;
using vyger.Core.Models;

namespace vyger.ViewModels
{
    public class WorkoutPlanCycleEditViewModel
    {
        public WorkoutPlan Plan { get; set; }

        public int CycleId { get; set; }

        public IList<WorkoutPlanCycle> Cycle { get; set; }
    }
}