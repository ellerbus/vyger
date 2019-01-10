from sqlalchemy import (Boolean, Column, DateTime, Float, Integer, MetaData,
                        String, Table, create_engine)


def create_exercise_table(meta):
    t = Table('Exercise',
              meta,
              Column('id', String(3),
                     primary_key=True, nullable=False),
              Column('name', String(150), nullable=False),
              Column('group', String(150), nullable=False),
              Column('category', String(150), nullable=False))
    return t


def create_routine_table(meta):
    t = Table('Routine',
              meta,
              Column('id', String(3),
                     primary_key=True, nullable=False),
              Column('name', String(150), nullable=False),
              Column('weeks', Integer(), nullable=False),
              Column('days', Integer(), nullable=False),
              Column('sets', String(150), nullable=False))
    return t


def create_routine_exercise_table(meta):
    t = Table('RoutineExercise',
              meta,
              Column('id', String(3),
                     primary_key=True, nullable=False),
              Column('name', String(150), nullable=False),
              Column('group', String(150), nullable=False),
              Column('category', String(150), nullable=False),
              Column('week', Integer(), nullable=False),
              Column('day', Integer(), nullable=False),
              Column('sequence', Integer(), nullable=False),
              Column('sets', String(150), nullable=False))
    return t


def create_log_exercise_table(meta):
    t = Table('LogExercise',
              meta,
              Column('id', String(3),
                     primary_key=True, nullable=False),
              Column('name', String(150), nullable=False),
              Column('group', String(150), nullable=False),
              Column('category', String(150), nullable=False),
              Column('date', String(10), nullable=False),
              Column('sets', String(150), nullable=False))
    return t


def create_cycle_table(meta):
    t = Table('Cycle',
              meta,
              Column('id', String(3),
                     primary_key=True, nullable=False),
              Column('name', String(150), nullable=False),
              Column('weeks', Integer(), nullable=False),
              Column('days', Integer(), nullable=False),
              Column('sequence', Integer(), nullable=False),
              Column('last_logged', String(3), nullable=False))
    return t


def create_cycle_input_table(meta):
    t = Table('CycleInput',
              meta,
              Column('id', String(3),
                     primary_key=True, nullable=False),
              Column('name', String(150), nullable=False),
              Column('group', String(150), nullable=False),
              Column('category', String(150), nullable=False),
              Column('weight', Float(), nullable=False),
              Column('reps', Integer(), nullable=False),
              Column('pullback', Integer(), nullable=False),
              Column('requires_input', Boolean(), nullable=False))
    return t


def create_cycle_exercise_table(meta):
    t = Table('CycleExercise',
              meta,
              Column('id', String(3),
                     primary_key=True, nullable=False),
              Column('name', String(150), nullable=False),
              Column('group', String(150), nullable=False),
              Column('category', String(150), nullable=False),
              Column('week', Integer(), nullable=False),
              Column('day', Integer(), nullable=False),
              Column('sequence', Integer(), nullable=False),
              Column('sets', String(150), nullable=False),
              Column('plan', String(150), nullable=False))
    return t


def create_database_engine():
    db_uri = 'sqlite://'
    engine = create_engine(db_uri)
    meta = MetaData(engine)

    create_exercise_table(meta)
    create_routine_table(meta)
    create_routine_exercise_table(meta)
    create_log_exercise_table(meta)
    create_cycle_table(meta)
    create_cycle_input_table(meta)
    create_cycle_exercise_table(meta)

    # Create all tables in meta
    meta.create_all()

    return engine
