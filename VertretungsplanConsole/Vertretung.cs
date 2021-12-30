using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertretungsplanConsole
{
    public class Vertretung
    {
        public string Stunde { get; set; }

        public string LehrerUndFach { get; set; }

        public string VertretungsLehrer { get; set; }

        public string Message { get; set; }

        public void Clear()
        {
            Stunde = string.Empty;
            LehrerUndFach = string.Empty;
            VertretungsLehrer = string.Empty;
            Message = string.Empty;
        }
    }
}
