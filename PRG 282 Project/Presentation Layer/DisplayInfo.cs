using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRG_282_Project.Business_Layer;
using PRG_282_Project.Data_Layer;

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
