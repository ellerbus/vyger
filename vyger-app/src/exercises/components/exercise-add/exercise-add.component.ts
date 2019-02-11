import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Exercise } from 'src/models/exercise';
import { PageTitleService } from 'src/services/page-title.service';
import { ExerciseService } from 'src/services/exercise.service';

@Component({
    selector: 'app-exercise-add',
    templateUrl: './exercise-add.component.html',
    styleUrls: ['./exercise-add.component.css']
})
export class ExerciseAddComponent implements OnInit
{
    exercise: Exercise;
    saving: boolean;

    constructor(
        private router: Router,
        private pageTitleService: PageTitleService,
        private ExerciseService: ExerciseService) { }

    ngOnInit()
    {
        this.pageTitleService.setTitle('Add Exercise');

        this.exercise = new Exercise();
        this.exercise.group = null;
        this.exercise.category = null;
    }

    cancel(): void
    {
        this.router.navigateByUrl('/exercises');
    }

    save(): void
    {
        this.saving = true;

        this.ExerciseService
            .add(this.exercise)
            .then(() =>
            {
                this.saving = false;
                this.router.navigateByUrl('/exercises');
            });
    }
}