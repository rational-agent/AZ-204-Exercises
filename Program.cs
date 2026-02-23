// See https://aka.ms/new-console-template for more information

using AZ_204_Exercises;

Console.WriteLine("What exercise do you want to run? (Blob, CosmosDB, Functions, LogicApps, ServiceBus, StorageQueue)");
var name = Console.ReadLine();

if (name != "Blob")
{
    var blob = new BlobExercise();
    blob.ProcessAsync();
}
