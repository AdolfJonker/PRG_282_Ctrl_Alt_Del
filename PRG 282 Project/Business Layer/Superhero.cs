using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_282_Project.Business_Layer
{
    internal class Superhero
    {
         public string heroID { get; set; }
 public string name { get; set; }
 public string age { get; set; }
 public string superpower { get; set; }
 public string score { get; set; }


 public Superhero(string heroID, string name, string age, string superpower, string score)
 {
     this.heroID = heroID;
     this.name = name;
     this.age = age;
     this.superpower = superpower;
     this.score = score;
 }

 internal static void AddNewHero(object text1, object text2, object text3, object text4, object text5)
 {
     throw new NotImplementedException();
 }

 //public override string ToString()
 //{
 //return $"HeroID: {this.heroID} | Name: {this.name} | Age: {this.age} | Superpower: {this.superpower} | Score: {this.score}";
 //}
    }
}
