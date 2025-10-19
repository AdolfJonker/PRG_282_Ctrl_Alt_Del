using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRG_282_Project.Data_Layer;

namespace PRG_282_Project.Business_Layer
{
    internal class FormatHandler
    {
        public List<Superhero> FormatData()
        {
            List<Superhero> heroes = new List<Superhero>();
            FileHandler handler = new FileHandler();
            foreach (var item in handler.ReadFiles())
            {
                string[] p = item.Split(',');
                if (p.Length >= 7) // Ensure all fields are present
                {
                    heroes.Add(new Superhero(p[0], p[1], p[2], p[3], p[4]));
                }
            }
            return heroes;
        }
    }
}
