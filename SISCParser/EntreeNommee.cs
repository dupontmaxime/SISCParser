using System.Collections.Generic;
using System.Linq;

namespace SISCParser
{
    internal class EntreeNommee
    {
        public static List<string> nomDesChamps { get; set; }
        public static int GetFieldIndex(string fieldName) => nomDesChamps.FindIndex(x => x.ToLower() == fieldName.ToLower());

        public static void NomDesChamps(string[] fields)
        {
            nomDesChamps = fields.ToList();
        }
    }
}