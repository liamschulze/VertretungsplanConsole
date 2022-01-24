using System;

namespace VertretungsplanConsole
{
    class Program
    {
        static int Main(string[] args)
        {
            // Set the URL
            string[] urls = { "http://www.kleist-schule.de/vertretungsplan/schueler/aktuelle%20plaene/1/vp.html", "http://www.kleist-schule.de/vertretungsplan/schueler/aktuelle%20plaene/2/vp.html" };

            Console.WriteLine("0: Vertretungsplan heute\n1: Vertretungsplan morgen");
            Console.WriteLine("Bitte wählen Sie einen Tag aus");

            int selectedDay;

            while (true)
            {
                string input = Console.ReadLine();

                // Try to parse to input into an integer
                if (!int.TryParse(input, out selectedDay))
                    Console.WriteLine("Bitte eine gültige Zahl eingeben");
                // Check if the input is in the given range
                else if (selectedDay != 0 && selectedDay != 1)
                    Console.WriteLine("Bitte eine Zahl eingeben, die sich im angegebenen Bereich befindet.");
                // Exit the loop
                else
                    break;
            }

            Console.WriteLine("==============================================");

            string url = selectedDay == 0 ? urls[0] : urls[1];

            // Parse the Vertretungsplan and write the data to the variable
            var klassen = VertretungsplanParser.ParseVertretungsplan(url);

            // Terminate the program if no plan is found
            if (klassen.Count == 0)
            {
                Console.WriteLine("Es ist noch kein Vertretungsplan vorhanden");
                Console.ReadKey();

                return 1;
            }

            // List all classes
            for (int i = 0; i < klassen.Count; i++)
            {
                Console.WriteLine($"{i}: {klassen[i].Name}");
            }

            Console.WriteLine("\nBitte wählen sie eine Klasse aus, indem Sie die Nummer eingeben, die davor steht...");

            // Initialize the selected class variable
            int selectedClass;

            while (true)
            {
                // Get input from the user
                string input1 = Console.ReadLine();

                // Try to parse to input into an integer
                if (!int.TryParse(input1, out selectedClass))
                    Console.WriteLine("Bitte eine gültige Zahl eingeben");
                // Check if the input is in the given range
                else if (selectedClass < 0 || selectedClass > klassen.Count)
                    Console.WriteLine("Bitte eine Zahl eingeben, die sich im angegebenen Bereich befindet.");
                // Exit the loop
                else
                    break;
            }

            Console.WriteLine("==============================================");

            // List all the Vertretungen for the selected class
            foreach (var _vertretung in klassen[selectedClass].Vertretungen)
            {
                Console.WriteLine($"{_vertretung.Stunde} {_vertretung.LehrerUndFach} ---> {_vertretung.VertretungsLehrer} {_vertretung.Message}");
            }

            // Wait for an input to end the program
            Console.WriteLine("Bitte drücken Sie eine beliebige Taste, um das Programm zu beenden...");
            Console.ReadKey();

            return 0;
        }
    }
}
