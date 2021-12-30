using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VertretungsplanConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set the URL
            var html = @"http://www.kleist-schule.de/vertretungsplan/schueler/aktuelle%20plaene/1/vp.html";

            // Create a new instance of HtmlWeb
            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);

            // Select the tables from the Document
            var nodes = htmlDoc.DocumentNode.SelectNodes("//body/table");

            // Select the table with the plan
            var vertretungsplan = nodes[2];

            // Select all rows in the table
            var rows = vertretungsplan.SelectNodes("tr");

            // Create a new list of Klassen
            var klassen = new List<Klasse>();

            // Create a new instance of Vertretung
            var vertretung = new Vertretung();

            foreach (var row in rows)
            {
                // Check if the row is a table heading
                if (row.InnerHtml.Replace(" ", string.Empty).StartsWith("<th") && row.InnerText.Replace(" ", string.Empty) != "TrainRaum")
                    // Create a new klasse and set the content of the table as the name
                    klassen.Add(new Klasse(row.InnerText.Replace(" ", string.Empty)));
                else if (row.InnerText.Replace(" ", string.Empty) == "TrainRaum")
                    break;
                else
                {
                    foreach (var node in row.ChildNodes)
                    {
                        // Add the content of the Vertretungsplan to vertretung
                        if (node.InnerHtml.Contains("class=\"Eins\""))
                        {
                            vertretung.Stunde = node.InnerText;
                        }
                        else if (node.InnerHtml.Contains("class=\"Zwei\""))
                        {
                            vertretung.LehrerUndFach = node.InnerText;
                        }
                        else if (node.InnerHtml.Contains("class=\"Vier\""))
                        {
                            vertretung.VertretungsLehrer = node.InnerText;
                        }
                        else if (node.InnerHtml.Contains("class=\"Fuenf\""))
                        {
                            vertretung.Message = node.InnerText.Replace("&auml;", "ä");
                        }
                    }

                    // Add the variable vertretung to the last class of the list
                    klassen.Last().Vertretungen.Add(vertretung);

                    // Whipe the data from vertretung
                    vertretung = new Vertretung();
                }
            }

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
