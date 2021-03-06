import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { SortablejsModule } from 'angular-sortablejs';
import { DirectivesModule } from 'src/directives/directives.module';
import { AuthenticationGuard } from 'src/guards/authentication.guard';
import { LogCycleComponent } from './components/log-cycle/log-cycle.component';
import { LogExerciseAddComponent } from './components/log-exercise-add/log-exercise-add.component';
import { LogExerciseEditComponent } from './components/log-exercise-edit/log-exercise-edit.component';
import { LogExercisePickerComponent } from './components/log-exercise-picker/log-exercise-picker.component';
import { LogExerciseSetComponent } from './components/log-exercise-set/log-exercise-set.component';
import { LogExerciseSetsComponent } from './components/log-exercise-sets/log-exercise-sets.component';
import { LogHistoryComponent } from './components/log-history/log-history.component';
import { LogImportComponent } from './components/log-import/log-import.component';
import { LogWeekHeaderComponent } from './components/log-week-header/log-week-header.component';
import { LogWeekComponent } from './components/log-week/log-week.component';


const routes: Routes = [
    {
        path: 'logs',
        canActivate: [AuthenticationGuard],
        children: [
            { path: '', component: LogWeekComponent },
            { path: 'add', component: LogExerciseAddComponent },
            { path: 'edit/:id', component: LogExerciseEditComponent },
            { path: 'cycle/:id', component: LogCycleComponent },
            { path: 'import', component: LogImportComponent },
            { path: 'history', component: LogHistoryComponent },
        ]
    },
];

@NgModule({
    declarations: [
        LogWeekComponent,
        LogWeekHeaderComponent,
        LogExerciseAddComponent,
        LogExercisePickerComponent,
        LogExerciseSetsComponent,
        LogExerciseSetComponent,
        LogExerciseEditComponent,
        LogImportComponent,
        LogHistoryComponent,
        LogCycleComponent],
    imports: [
        BrowserModule,
        FormsModule,
        RouterModule,
        DirectivesModule,
        SortablejsModule,
        RouterModule.forRoot(routes, { useHash: true })
    ]
})
export class LogsModule { }
