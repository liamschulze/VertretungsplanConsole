using System;

namespace VertretungsplanConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set the URL
            var url = @"http://www.kleist-schule.de/vertretungsplan/schueler/aktuelle%20plaene/1/vp.html";

            // Parse the Vertretungsplan and write the data to the variable
            var klassen = VertretungsplanParser.ParseVertretungsplan(url);

            // List all classes
            for (int i = 0; i < klassen.Count; i++)
            {
                Console.WriteLine($"{i}: {klassen[i].Name}");
            }

            Console.WriteLine("Bitte wählen sie eine Klasse aus, indem Sie die Nummer eingeben, die davor steht...");

            // Default selected class to -1 so that it is invalid
            int selectedClass;

            while (true)
            {
                // Get input from the user
                string input = Console.ReadLine();

                // Try to parse to input into an integer
                if (!int.TryParse(input, out selectedClass))
                    Console.WriteLine("Bitte eine gültige Zahl eingeben");
                // Check if the input is in the given range
                else if (selectedClass < 0 || selectedClass > klassen.Count)
                    Console.WriteLine("Bitte eine gültige Zahl eingeben");
                // Exit the loop
                else
                    break;
            }

            // List all the Vertretungen for the selected class
            foreach (var _vertretung in klassen[selectedClass].Vertretungen)
            {
                Console.WriteLine($"{_vertretung.Stunde} {_vertretung.LehrerUndFach} ---> {_vertretung.VertretungsLehrer} {_vertretung.Message}");
            }

            // Wait for an input to end the program
            Console.WriteLine("Bitte drücken Sie eine beliebige Taste, um das Programm zu beenden...");
            Console.ReadKey();
        }
    }
}
