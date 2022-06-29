using System;

namespace VertretungsplanConsole
{
    class Program
    {
        // urls to the Vertretungsplan
        private static string[] urls = { "http://www.kleist-schule.de/vertretungsplan/schueler/aktuelle%20plaene/1/vp.html", "http://www.kleist-schule.de/vertretungsplan/schueler/aktuelle%20plaene/2/vp.html" };

        static int Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("(1): Informationen ausgeben");
                Console.WriteLine("(2): Vertretungsplan ausgeben");
                Console.WriteLine("(3): Ersatzraumplan ausgeben");
                Console.WriteLine("(4): Fehlende Lehrer ausgeben");
                Console.WriteLine("(5): Programm beenden");
                Console.WriteLine("Bitte wählen Sie eine Option: ");
                Console.Write(">");

                // Option einlesen
                string input = Console.ReadLine();

                if (!int.TryParse(input, out var option))
                    Console.WriteLine("Bitte eine gültige Zahl eingeben.");

                switch (option)
                {
                    case 1:
                        PrintInformation();
                        break;
                    case 2:
                        PrintVertretungsplan();
                        break;
                    case 3:
                        PrintErstzraumplan();
                        break;
                    case 4:
                        PrintFehlendeLehrer();
                       break;
                    case 5:
                        // Programm beenden
                        return 0;
                    default:
                        Console.WriteLine("Bitte eine Zahl eingeben, die sich im angegebenen Bereich befindet.\n");
                        break;
                }
            }
        }

        public static void PrintInformation()
        {
            int day = SelectDay();

            var vertretungsplanParser = new VertretungsplanParser(urls[day]);

            Console.WriteLine("==============================================");
            Console.WriteLine(vertretungsplanParser.ParseInformation());

            // Add a blank line
            Console.WriteLine();
        }

        public static void PrintVertretungsplan()
        {
            // Get the day
            int day = SelectDay();

            // Create new Vertretungsplanparser
            var vertretungsplanParser = new VertretungsplanParser(urls[day]);

            // Parse the Vertretungsplan and write the data to the variable
            var klassen = vertretungsplanParser.ParseVertretungsplan();

            if (klassen.Count == 0)
            {
                Console.WriteLine("Es ist noch kein Vertretungsplan vorhanden");
                return;
            }

            // List all classes
            for (int i = 0; i < klassen.Count; i++)
            {
                Console.WriteLine($"({i + 1}): {klassen[i].Name}");
            }

            // Print the date of the plan
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine(vertretungsplanParser.parseDate());
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("\nBitte wählen sie eine Klasse aus, indem Sie die Nummer eingeben, die davor steht...");

            int selectedClass;

            while (true)
            {
                // Get input from the user
                Console.Write(">");
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
            foreach (var _vertretung in klassen[selectedClass - 1].Vertretungen)
            {
                Console.WriteLine($"{_vertretung.Stunde} {_vertretung.LehrerUndFach} ---> {_vertretung.VertretungsLehrer} {_vertretung.Message}");
            }

            // Add a blank line
            Console.WriteLine();
        }

        public static void PrintErstzraumplan()
        {
            // Get the day
            int day = SelectDay();

            // Create new Vertretungsplanparser
            var vertretungsplanParser = new VertretungsplanParser(urls[day]);

            // Parse the Vertretungsplan and write the data to the variable
            var klassen = vertretungsplanParser.ParseErsatzraumplan();

            // Check if the plan exists
            if (klassen.Count == 0)
            {
                Console.WriteLine("Es ist noch kein Ersatzraumplan vorhanden");
                return;
            }

            // List all classes
            for (int i = 0; i < klassen.Count; i++)
            {
                Console.WriteLine($"({i + 1}): {klassen[i].Name}");
            }

            // Print the date of the plan
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine(vertretungsplanParser.parseDate().Replace("Vertretungsplan", "Ersatzraumplan"));
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("\nBitte wählen sie eine Klasse aus, indem Sie die Nummer eingeben, die davor steht...");

            int selectedClass;

            while (true)
            {
                // Get input from the user
                Console.Write(">");
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
            foreach (var _vertretung in klassen[selectedClass - 1].Vertretungen)
            {
                Console.WriteLine($"{_vertretung.Stunde} {_vertretung.LehrerUndFach} ---> {_vertretung.VertretungsLehrer} {_vertretung.Message}");
            }

            // Add a blank line
            Console.WriteLine();
        }

        public static void PrintFehlendeLehrer()
        {
            // Get the day
            int day = SelectDay();

            // Create new VertretunsplanParser
            var vertretungsplanParser = new VertretungsplanParser(urls[day]);

            // Get the missing teachers
            var fehlendeLehrer = vertretungsplanParser.ParseFehlendeLehrer();

            // List missing teachers
            foreach (var lehrer in fehlendeLehrer)
            {
                Console.WriteLine(lehrer);
            }

            if (fehlendeLehrer.Count == 0)
            {
                Console.WriteLine("Es ist noch kein Vertretungsplan vorhanden.");
            }

            // Add a blank line
            Console.WriteLine();
        }

        /// <summary>
        /// Select the day for the Vertretungsplan
        /// </summary>
        /// <returns>The day</returns>
        private static int SelectDay()
        {
            while (true)
            {
                Console.WriteLine("==============================================");
                Console.WriteLine("(1): Vertretungsplan heute\n(2): Vertretungsplan nächster Schultag");
                Console.WriteLine("Bitte wählen Sie einen Tag aus...");
                Console.Write(">");

                // Get input from user
                string input = Console.ReadLine();

                if (!int.TryParse(input.Trim(), out int day))
                    Console.WriteLine("Bitte eine gültige Zahl eingeben:");
                else if (day != 1 && day != 2)
                    Console.WriteLine("Bitte einen Wert im angegebenen Bereich eingeben.");
                else
                    return day - 1;
            }
        }
    }
}
