using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_282_Project.Business_Layer
{
    public class Superhero
    {
        public string heroID { get; set; }
        public string name { get; set; }
        public string age { get; set; }
        public string superpower { get; set; }
        public string score { get; set; }
        public string rank { get; set; }
        public string threatLevel { get; set; }

        public Superhero(string heroID, string name, string age, string superpower, string score, string rank = "", string threatLevel = "")
        {
            this.heroID = heroID;
            this.name = name;
            this.age = age;
            this.superpower = superpower;
            this.score = score;
            this.rank = rank;
            this.threatLevel = threatLevel;
        }

        public override string ToString()
        {
            return $"{heroID},{name},{age},{superpower},{score},{rank},{threatLevel}";
        }
    }
}
