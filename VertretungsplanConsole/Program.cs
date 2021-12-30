﻿using AngleSharp.Common;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
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

            // Create a new 
            var klassen = new List<Klasse>();

            var vertretung = new Vertretung();

            foreach (var row in rows)
            {
                if (row.InnerHtml.Replace(" ", string.Empty).StartsWith("<th") && row.InnerText.Replace(" ", string.Empty) != "TrainRaum")
                {
                    klassen.Add(new Klasse(row.InnerText.Replace(" ", string.Empty)));
                }
                else
                {
                    foreach (var node in row.ChildNodes)
                    {
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


                    klassen.Last().Vertretungen.Add(vertretung);

                    // TODO: Empty the variable without wiping it in the class
                    vertretung = new Vertretung();
                }
            }

            foreach (var klasse in klassen)
            {
                Console.WriteLine(klasse.Name);

                foreach (var _vertretung in klasse.Vertretungen)
                {
                    Console.WriteLine($"{_vertretung.Stunde} {_vertretung.LehrerUndFach} ------> {_vertretung.VertretungsLehrer} {_vertretung.Message}");
                }
            }
        }
    }
}
