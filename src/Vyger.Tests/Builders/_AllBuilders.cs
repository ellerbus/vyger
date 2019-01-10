namespace Vyger.Tests
{
    internal static class CreateA
    {
        public static MemberBuilder Member { get { return new MemberBuilder(); } }
        public static ExerciseBuilder Exercise { get { return new ExerciseBuilder(); } }
        public static RoutineBuilder Routine { get { return new RoutineBuilder(); } }
        public static CycleBuilder Cycle { get { return new CycleBuilder(); } }
    }
}