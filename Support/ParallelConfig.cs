using NUnit.Framework;

// Reqnroll turns each .feature file into an NUnit fixture. Running fixtures in parallel is safe
// because every scenario gets its own browser and data through SharedContext (no static state).
// Change the number of workers from the command line:
//   dotnet test -- NUnit.NumberOfTestWorkers=2
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(4)]
