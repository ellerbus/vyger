import { utilities } from 'src/models/utilities';

export enum Groups
{
    Abs = 'Abs',
    Biceps = 'Biceps',
    Chest = 'Chest',
    Forearms = 'Forearms',
    Legs = 'Legs',
    LowerBack = 'LowerBack',
    Shoulders = 'Shoulders',
    Traps = 'Traps',
    Triceps = 'Triceps',
    UpperBack = 'UpperBack',
}

export enum Categories
{
    Barbell = 'Barbell',
    Dumbbell = 'Dumbbell',
    Machine = 'Machine',
    Hammer = 'Hammer',
    Body = 'Body',
}

export interface IExercise
{
    id?: string;
    group: Groups | string;
    category: Categories | string;
    name: string;
}

export class Exercise implements IExercise
{
    id: string = utilities.generateId('x', 2);
    group: Groups;
    category: Categories;
    name: string;

    constructor(source?: IExercise)
    {
        const keys = ['id', 'group', 'category', 'name'];

        utilities.extend(this, source, keys);
    }

    static matches(e: Exercise, category: Categories, name: string): boolean
    {
        if (e.category == category)
        {
            if (e.name != null && name != null)
            {
                return e.name.toLowerCase() == name.toLowerCase();
            }
        }

        return false;
    }

    static defaultList(): Exercise[]
    {
        return [
            new Exercise({ group: 'Abs', category: 'Machine', name: 'Crunch' }),
            new Exercise({ group: 'Abs', category: 'Body', name: 'Crunch' }),

            new Exercise({ group: 'Biceps', category: 'Barbell', name: 'Curls' }),
            new Exercise({ group: 'Biceps', category: 'Dumbbell', name: 'Curls' }),

            new Exercise({ group: 'Chest', category: 'Barbell', name: 'Decline Bench Press' }),
            new Exercise({ group: 'Chest', category: 'Dumbbell', name: 'Decline Bench Press' }),
            new Exercise({ group: 'Chest', category: 'Hammer', name: 'Decline Bench Press' }),
            new Exercise({ group: 'Chest', category: 'Barbell', name: 'Flat Bench Press' }),
            new Exercise({ group: 'Chest', category: 'Dumbbell', name: 'Flat Bench Press' }),
            new Exercise({ group: 'Chest', category: 'Hammer', name: 'Flat Bench Press' }),
            new Exercise({ group: 'Chest', category: 'Barbell', name: 'Incline Bench Press' }),
            new Exercise({ group: 'Chest', category: 'Dumbbell', name: 'Incline Bench Press' }),
            new Exercise({ group: 'Chest', category: 'Hammer', name: 'Incline Bench Press' }),
            new Exercise({ group: 'Chest', category: 'Machine', name: 'Flyes' }),

            new Exercise({ group: 'Forearms', category: 'Barbell', name: 'Standing Wrist Curls' }),
            new Exercise({ group: 'Forearms', category: 'Barbell', name: 'Reverse Wrist Curls' }),

            new Exercise({ group: 'Legs', category: 'Barbell', name: 'Front Squats' }),
            new Exercise({ group: 'Legs', category: 'Barbell', name: 'Rear Squats' }),
            new Exercise({ group: 'Legs', category: 'Machine', name: 'Leg Press' }),

            new Exercise({ group: 'LowerBack', category: 'Barbell', name: 'Deadlifts' }),
            new Exercise({ group: 'LowerBack', category: 'Dumbbell', name: 'Deadlifts' }),

            new Exercise({ group: 'Shoulders', category: 'Dumbbell', name: 'Front Lat Raises' }),
            new Exercise({ group: 'Shoulders', category: 'Hammer', name: 'Press' }),
            new Exercise({ group: 'Shoulders', category: 'Dumbbell', name: 'Rear Lat Raises' }),
            new Exercise({ group: 'Shoulders', category: 'Dumbbell', name: 'Side Lat Raises' }),

            new Exercise({ group: 'Traps', category: 'Barbell', name: 'Shrugs' }),
            new Exercise({ group: 'Traps', category: 'Dumbbell', name: 'Shrugs' }),
            new Exercise({ group: 'Traps', category: 'Hammer', name: 'Shrugs' }),

            new Exercise({ group: 'Triceps', category: 'Body', name: 'Dips' }),

            new Exercise({ group: 'UpperBack', category: 'Hammer', name: 'High Rows' }),
            new Exercise({ group: 'UpperBack', category: 'Hammer', name: 'Low Rows' }),
        ];
    }

    static compare(a: Exercise, b: Exercise): number
    {
        var group = a.group.localeCompare(b.group);

        if (group != 0)
        {
            return group;
        }

        var category = a.category.localeCompare(b.category);

        if (category != 0)
        {
            return category;
        }

        return a.name.localeCompare(b.name);
    }
}
