import { Exercise } from './exercise';
import { utilities } from './utilities';
import { WorkoutSet } from './workout-set';

export enum LogEvaluation
{
    None = 0,
    FellShort = -1,
    Matched = 1,
    Exceeded = 2
}
export class LogExercise extends Exercise
{
    ymd: string;
    sets: string[] = [];
    sequence: number = 1;

    oneRepMaxSet: number;
    oneRepMax: number;

    evaluation: LogEvaluation;

    constructor(source?: any)
    {
        super(source);

        const keys = ['ymd', 'sequence', 'oneRepMaxSet', 'oneRepMax', 'evaluation'];

        utilities.extend(this, source, keys);

        if (source && source.sets)
        {
            this.sets = [...source.sets];
        }

        this.oneRepMaxSet = this.oneRepMaxSet || 0;
    }

    updateOneRepMax()
    {
        this.oneRepMax = null;

        for (let i = 0; i < this.sets.length; i++)
        {
            let set = new WorkoutSet(this.sets[i]);

            if (this.oneRepMax == null || this.oneRepMax < set.oneRepMax)
            {
                this.oneRepMaxSet = i;
                this.oneRepMax = set.oneRepMax;
            }
        }
    }

    static compare(a: LogExercise, b: LogExercise): number
    {
        const ymd = a.ymd.localeCompare(b.ymd);

        if (ymd != 0)
        {
            return ymd;
        }

        const seq = a.sequence - b.sequence;

        if (seq != 0)
        {
            return seq;
        }

        return Exercise.compare(a, b);
    }
}