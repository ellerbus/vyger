using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Augment;
using vyger.Core;
using vyger.Core.Models;
using vyger.Core.Services;
using vyger.ViewModels;

namespace vyger.Controllers
{
    [RoutePrefix("Exercises"), MvcAuthorizeRoles(Constants.Roles.ActiveMember)]
    public partial class ExercisesController : BaseController
    {
        #region Members

        private IExerciseService _service;

        #endregion

        #region Constructors

        public ExercisesController(IExerciseService service)
        {
            _service = service;
        }

        #endregion

        #region List Methods

        [HttpGet, Route("Index")]
        public virtual ActionResult Index(ExerciseGroups group = ExerciseGroups.None, ExerciseCategories category = ExerciseCategories.None)
        {
            IList<Exercise> exercises = _service
                .GetExercises()
                .Filter(group, category)
                .ToList();

            ExerciseIndexViewModel vm = new ExerciseIndexViewModel()
            {
                Items = exercises,
                SelectedCategory = category,
                SelectedGroup = group
            };

            return View(vm);
        }

        #endregion

        #region Create Methods

        [HttpGet, Route("Create")]
        public virtual ActionResult Create(ExerciseGroups group = ExerciseGroups.None, ExerciseCategories category = ExerciseCategories.None)
        {
            Exercise vm = new Exercise();

            vm.Group = group;

            vm.Category = category;

            return View(vm);
        }

        [HttpPost, Route("Create"), ValidateAntiForgeryToken]
        public virtual ActionResult Create(Exercise post)
        {
            if (ModelState.IsValid)
            {
                ExerciseCollection exercises = _service.GetExercises();

                exercises.Add(post);

                _service.SaveExercises();

                AddFlashMessage(FlashMessageType.Success, "Exercise created successfully");

                return RedirectToAction("Index");
            }

            Exercise vm = new Exercise(post);

            return View(vm);
        }

        #endregion

        #region Edit Methods

        [HttpGet, Route("Edit/{id}")]
        public virtual ActionResult Edit(string id)
        {
            Exercise exercise = _service.GetExercises().GetByPrimaryKey(id);

            if (exercise == null)
            {
                return RedirectToAction("Index");
            }

            Exercise vm = new Exercise(exercise);

            return View(vm);
        }

        [HttpPost, Route("Edit/{id}"), ValidateAntiForgeryToken]
        public virtual ActionResult Edit(string id, Exercise post)
        {
            if (ModelState.IsValid)
            {
                ExerciseCollection exercises = _service.GetExercises();

                Exercise exercise = exercises.GetByPrimaryKey(id);

                exercise.OverlayWith(post);

                _service.SaveExercises();

                AddFlashMessage(FlashMessageType.Success, "Exercise saved successfully");

                return RedirectToAction("Index");
            }

            Exercise vm = new Exercise(post);

            return View(vm);
        }

        #endregion

        #region Json Query Methods

        [HttpGet, Route("Fetch")]
        public virtual ActionResult Fetch(string id)
        {
            Exercise exercise = null;

            _service.GetExercises().TryGetByPrimaryKey(id, out exercise);

            return Json(exercise ?? new Exercise(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("Query")]
        public virtual ActionResult Query(ExerciseGroups group = ExerciseGroups.None, ExerciseCategories category = ExerciseCategories.None)
        {
            IList<Exercise> exercises = _service
                .GetExercises()
                .Filter(group, category)
                .OrderBy(x => x.DetailName)
                .ToList();

            return Json(exercises, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
