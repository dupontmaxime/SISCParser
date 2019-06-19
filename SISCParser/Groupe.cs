using System.Collections.Generic;
using System.Linq;

namespace SISCParser
{
   internal class Groupe : EntreeNommee
   {
      public string Numero { get; set; }
      public string Identifiant { get; set; }

      public Groupe()
      {

      }
      public Groupe(string[] fields) : this()
      {
         Numero = fields.ElementAt(GetFieldIndex("numero"));
         string [] IdentifiantLong = fields.ElementAt(GetFieldIndex("identifiant")).Split();
         Identifiant = IdentifiantLong[1];
      }
   }
}