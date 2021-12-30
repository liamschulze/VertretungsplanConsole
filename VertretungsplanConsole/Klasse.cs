using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
