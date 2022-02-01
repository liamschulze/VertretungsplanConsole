using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace VertretungsplanConsole
{
    public class VertretungsplanParser
    {
        /// <summary>
        /// Parses the Vertretungsplan
        /// </summary>
        /// <param name="url">The url to the Vertretungsplan</param>
        /// <returns>The parsed data</returns>
        public static List<Klasse> ParseVertretungsplan(string url)
        {
            // Create a new instance of HtmlWeb
            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(url);

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
                }
                catch
                {
                    continue;
                }
            }

            return klassen;
        }
    }
}
