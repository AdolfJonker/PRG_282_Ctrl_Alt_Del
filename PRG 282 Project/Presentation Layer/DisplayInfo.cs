using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_282_Project.Presentation_Layer
{
    internal class DisplayInfo
    {
         public List<Superhero> GetHero()
 {
     FormatHandler handler = new FormatHandler();
     return handler.FormatData();
 }
    }
}
