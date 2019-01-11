import { Directive, Input } from '@angular/core';
import { FormControl, ValidationErrors, NG_ASYNC_VALIDATORS, AsyncValidator } from '@angular/forms';
import { Observable, from } from 'rxjs';


import { Exercise } from '../../models/exercise';
import { ExercisesRepository } from '../exercises.repository';

@Directive({
    selector: '[uniqueExerciseName][ngModel]',
    providers: [
        { provide: NG_ASYNC_VALIDATORS, useExisting: ExerciseNameValidatorDirective, multi: true }
    ]
})
export class ExerciseNameValidatorDirective implements AsyncValidator {
    @Input('uniqueExerciseName') exercise: Exercise;

    constructor(
        private exercisesRepository: ExercisesRepository) { }

    validate(c: FormControl): Observable<ValidationErrors> {
        let p = this.exercisesRepository
            .getExercises()
            .then(exercises => {
                for (let i = 0; i < exercises.length; i++) {
                    let ex: Exercise = exercises[i];
                    if (this.exercise.id != ex.id) {
                        if (Exercise.matches(ex, this.exercise.category, c.value)) {
                            return true;
                        }
                    }
                }

                return false;
            })
            .then(exists => exists ? { uniqueExerciseName: 'x' } : null);

        return from(p);
    }
}
