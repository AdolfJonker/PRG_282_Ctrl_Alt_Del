using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_282_Project.Business_Layer
{
    internal class FormatHandler
    {
        public List<Superhero> FormatData()
{
    List<Superhero> hero = new List<Superhero>();

    FileHandler handler = new FileHandler();
    foreach (var item in handler.ReadFiles())
    {
        string[] p = item.Split(',');

        hero.Add(new Superhero(p[0], p[1], p[2], p[3], p[4]));
    }

    return hero;
}
    }
}
