using System;
using System.Collections.Generic;

namespace SISCParser
{
    class ListeChamps : Dictionary<string, DateTime>
    {
        public ListeChamps()
        {
        }

        public ListeChamps(string field)
        {
            RemplirListe(field);
        }

        public void RemplirListe(string field)
        {
            Clear();
            string[] listeSepare = field.Split(' ');
            foreach (string entiteSepare in listeSepare)
            {
                if (!ContainsKey(entiteSepare))
                {
                    Add(entiteSepare, DateTime.MinValue);
                }
            }
        }
    }
}
