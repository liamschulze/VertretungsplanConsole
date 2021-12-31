using System.Collections.Generic;

namespace VertretungsplanConsole
{
    public class Klasse
    {
        public string Name { get; set; }

        public List<Vertretung> Vertretungen { get; set; } = new List<Vertretung>();

        public Klasse(string name)
        {
            Name = name;
        }
    }
}
