import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';

import { ExerciseCategoryComponent } from './exercise-category.component';
import { Exercise } from 'src/models/exercise';

describe('ExerciseCategoryComponent', () =>
{
    let component: ExerciseCategoryComponent;
    let fixture: ComponentFixture<ExerciseCategoryComponent>;

    beforeEach(async(() =>
    {
        const options = {
            declarations: [ExerciseCategoryComponent],
            imports: [FormsModule],
            providers: [NgForm],
            schemas: [NO_ERRORS_SCHEMA]
        };
        TestBed.configureTestingModule(options).compileComponents();
    }));

    beforeEach(() =>
    {
        fixture = TestBed.createComponent(ExerciseCategoryComponent);
        component = fixture.componentInstance;
        component.exercise = new Exercise();
        fixture.detectChanges();
    });

    it('should create', () =>
    {
        expect(component).toBeTruthy();
        expect(component.categories).toBeDefined();
    });
});
