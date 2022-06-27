using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System;

namespace VertretungsplanConsole
{
    public class VertretungsplanParser
    {
        #region Private fields

        private string _url = string.Empty;

        #endregion

        #region Constructor

        public VertretungsplanParser(string url)
        {
            _url = url;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Parses the Vertretungsplan
        /// </summary>
        /// <returns>The parsed data</returns>
        public List<Klasse> ParseVertretungsplan()
        {
            // Load the html document
            var htmlDoc = LoadHtml();

            // Select the tables from the Document
            var nodes = htmlDoc.DocumentNode.SelectNodes("//body/table");

            // Create a new list of Klassen
            var klassen = new List<Klasse>();

            // Create a new instance of Vertretung
            var vertretung = new Vertretung();

            // If there aren't any nodes return an empty object
            if (nodes == null)
                return klassen;

            for (int i = 0; i < nodes.Count; i++)
            {
                try
                {
                    // Select the table with the plan
                    var vertretungsplan = nodes[i];

                    // Select all rows in the table
                    var rows = vertretungsplan.SelectNodes("tr");

                    if (rows == null)
                        rows = vertretungsplan.SelectNodes("tbody/tr");

                    foreach (var row in rows)
                    {
                        row.InnerHtml = row.InnerHtml.Replace("\n", string.Empty);

                        // Check if the row is a table heading
                        if (row.InnerHtml.Replace(" ", string.Empty).Contains("<th") && row.InnerText.Replace(" ", string.Empty) != "TrainRaum")
                            // Create a new klasse and set the content of the table as the name
                            klassen.Add(new Klasse(row.InnerText.Replace(" ", string.Empty)));
                        else if (row.InnerText.Replace(" ", string.Empty) == "TrainRaum")
                            break;
                        else
                        {
                            foreach (var node in row.ChildNodes)
                            {
                                node.InnerHtml = node.InnerHtml.Replace("\n", string.Empty);

                                // Add the content of the Vertretungsplan to vertretung
                                if (node.InnerHtml.Contains("class=\"Eins\""))
                                {
                                    vertretung.Stunde = node.InnerText.Trim();
                                }
                                else if (node.InnerHtml.Contains("class=\"Zwei\""))
                                {
                                    vertretung.LehrerUndFach = node.InnerText.Replace("&auml;", "ä").Replace("&ouml;", "ö").Replace("&uuml;", "ü").Trim();
                                }
                                else if (node.InnerHtml.Contains("class=\"Vier\""))
                                {
                                    vertretung.VertretungsLehrer = node.InnerText.Replace("&auml;", "ä").Replace("&ouml;", "ö").Replace("&uuml;", "ü").Trim();
                                }
                                else if (node.InnerHtml.Contains("class=\"Fuenf\""))
                                {
                                    vertretung.Message = node.InnerText.Replace("&auml;", "ä").Replace("&ouml;", "ö").Replace("&uuml;", "ü").Trim();
                                }
                            }

                            // Add the variable vertretung to the last class of the list
                            klassen.Last().Vertretungen.Add(vertretung);

                            // Whipe the data from vertretung
                            vertretung = new Vertretung();
                        }
                    }

                    break;
                }
                catch
                {
                    continue;
                }
            }

            return klassen;
        }

        public List<Klasse> ParseErsatzraumplan()
        {
            // Load the html document
            var htmlDoc = LoadHtml();

            // Select the tables from the Document
            var nodes = htmlDoc.DocumentNode.SelectNodes("//body/table");

            // Create a new list of Klassen
            var klassen = new List<Klasse>();

            // Create a new instance of Vertretung
            var vertretung = new Vertretung();

            // If there aren't any nodes return an empty object
            if (nodes == null)
                return klassen;

            // Select the last table
            var ersatzraumplan = nodes[nodes.Count - 1];

            // Select all rows in the table
            var rows = ersatzraumplan.SelectNodes("tr");

            if (rows == null)
                rows = ersatzraumplan.SelectNodes("tbody/tr");

            foreach (var row in rows)
            {
                row.InnerHtml = row.InnerHtml.Replace("\n", string.Empty);

                // Check if the row is a table heading
                if (row.InnerHtml.Replace(" ", string.Empty).Contains("<th") && row.InnerText.Replace(" ", string.Empty) != "TrainRaum")
                    // Create a new klasse and set the content of the table as the name
                    klassen.Add(new Klasse(row.InnerText.Replace(" ", string.Empty)));
                else if (row.InnerText.Replace(" ", string.Empty) == "TrainRaum")
                    break;
                else
                {
                    foreach (var node in row.ChildNodes)
                    {
                        node.InnerHtml = node.InnerHtml.Replace("\n", string.Empty);

                        // Add the content of the Vertretungsplan to vertretung
                        if (node.InnerHtml.Contains("class=\"Eins\""))
                        {
                            vertretung.Stunde = node.InnerText.Trim();
                        }
                        else if (node.InnerHtml.Contains("class=\"Zwei\""))
                        {
                            vertretung.LehrerUndFach = node.InnerText.Replace("&auml;", "ä").Replace("&ouml;", "ö").Replace("&uuml;", "ü").Trim();
                        }
                        else if (node.InnerHtml.Contains("class=\"Vier\""))
                        {
                            vertretung.VertretungsLehrer = node.InnerText.Replace("&auml;", "ä").Replace("&ouml;", "ö").Replace("&uuml;", "ü").Trim();
                        }
                        else if (node.InnerHtml.Contains("class=\"Fuenf\""))
                        {
                            vertretung.Message = node.InnerText.Replace("&auml;", "ä").Replace("&ouml;", "ö").Replace("&uuml;", "ü").Trim();
                        }
                    }

                    // Add the variable vertretung to the last class of the list
                    klassen.Last().Vertretungen.Add(vertretung);

                    // Whipe the data from vertretung
                    vertretung = new Vertretung();
                }
            }

            // Check if the ersatzraumplan equals the vertretungsplan
            if (klassen != ParseVertretungsplan())
            {
                // If it isn't the same return it
                return klassen;
            }
            else
            {
                // else return null
                return null;
            }
        }

        public List<string> ParseFehlendeLehrer()
        {
            // Load html
            var htmlDoc = LoadHtml();

            // Select the table with the missing teachers
            var node = htmlDoc.DocumentNode.SelectSingleNode("//body/table[1]");

            // Select all rows
            var rows = node.SelectNodes("tbody/tr");

            if (rows == null)
            {
                rows = node.SelectNodes("tr");
            }

            // initialize the string that gets returned
            List<string> fehlendeLehrer = new List<string>();

            foreach (var row in rows)
            {
                // Remove all new lines
                row.InnerHtml = row.InnerHtml.Replace("\n", string.Empty);

                // Add Umlaute
                row.InnerHtml = row.InnerHtml.Replace("&auml;", "ä").Replace("&ouml;", "ö").Replace("&uuml;", "ü");

                // Select the divs in the row
                var divs = row.SelectNodes("td/div");


                if (divs.Count == 4)
                {
                    fehlendeLehrer.Add($"{divs[0].InnerText}   {divs[1].InnerText}");
                    fehlendeLehrer.Add($"{divs[2].InnerText}   {divs[3].InnerText}");
                }
                else if (divs.Count == 2)
                {
                    fehlendeLehrer.Add($"{divs[0].InnerText}   {divs[1].InnerText}");
                }
            }

            return fehlendeLehrer;
        }

        /// <summary>
        /// Gets important information about the shool day
        /// </summary>
        /// <returns>information</returns>
        public string ParseInformation()
        {
            // Load the html document
            var htmlDoc = LoadHtml();

            var text = htmlDoc.DocumentNode.SelectNodes("//body/big/big");

            if (text == null)
                return string.Empty;

            return text[0].InnerText.Replace("&auml;", "ä").Replace("&ouml;", "ö").Replace("&uuml;", "ü").Replace("*innen", string.Empty).Replace("&szlig;", "ß").Replace("&nbsp;", string.Empty).Trim();
        }

        /// <summary>
        /// Gets the date of the selected plan
        /// </summary>
        /// <returns>The date of the plan</returns>
        public string parseDate()
        {
            // Load the html document
            var htmlDoc = LoadHtml();

            // Get the date from the plan
            var date = htmlDoc.DocumentNode.SelectSingleNode("//body/h3[2]");

            return date.InnerText.Replace("&uuml;", "ü");
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Loads the html document
        /// </summary>
        /// <returns>the html document</returns>
        private HtmlDocument LoadHtml()
        {
            // Create a new instance of HtmlWeb
            HtmlWeb web = new HtmlWeb();

            HtmlDocument htmlDoc;

            try
            {
                htmlDoc = web.Load(_url);

                return htmlDoc;
            }
            catch
            {
                // Write an error message if no connection can be established and end the program
                Console.WriteLine("Es konnte keine Verbindung zum Sever hergestellt werden. \nBitte überprüfen Sie ihre Internetverbindung oder warten Sie ein paar Minuten.");
                Console.WriteLine("Bitte drücken Sie eine beliebige Taste, um das Programm zu beenden...");
                Console.ReadLine();
                return null;
            }
        }

        #endregion
    }
}
