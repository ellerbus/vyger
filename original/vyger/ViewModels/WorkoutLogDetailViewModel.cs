using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vyger.Core.Models;

namespace vyger.ViewModels
{
    public class WorkoutLogDetailViewModel
    {
        public WorkoutLogDetailViewModel()
        {
        }

        [DisplayName("Workout Date"), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime LogDate { get; set; }

        public List<WorkoutLog> Logs { get; set; } = new List<WorkoutLog>();

        #region Plan View

        public object GetPlanRouteValues(DateTime date)
        {
            return new
            {
                routine = RoutineId,
                plan = PlanId,
                cycle = CycleId,
                week = WeekId,
                day = DayId,
                date = date.ToYMD()
            };
        }

        public bool CanChangeDate { get; set; }
        public string RoutineId { get; set; }
        public int PlanId { get; set; }
        public int CycleId { get; set; }
        public int WeekId { get; set; }
        public int DayId { get; set; }

        #endregion
    }
}