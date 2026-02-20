// See https://aka.ms/new-console-template for more information

using AZ_204_Exercises;

Console.WriteLine("What's your name?");
var name = Console.ReadLine();

if (name != null)
{
    var blob = new BlobExercise();
    blob.ProcessAsync();
}
