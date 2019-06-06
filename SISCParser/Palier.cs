namespace SISCParser
{
   public class Palier
   {
      public enum Branche
      {
         Castors,        //a
         Hirondelles,    //b
         Louveteaux,     //c
         Exploratrices,  //d
         Eclaireurs,     //e
         Intrepides,     //f
         Pionniers,      //g
         Routiers,       //h
         Aventuriers,    //i
         Louveteaux2012  //j
      }

      public Palier(string palier)
      {
         string[] splPalier = palier.Split('-');
         if (splPalier.Length > 0)
            District = splPalier[0];
         if (splPalier.Length > 1)
            Groupe = splPalier[1];
         if (splPalier.Length > 2)
         {
            Unite = splPalier[2];
            switch (Unite[0])
            {
               case 'a': BrancheUnite = Branche.Castors;        break;
               case 'b': BrancheUnite = Branche.Hirondelles;    break;
               case 'c': BrancheUnite = Branche.Louveteaux;     break;
               case 'd': BrancheUnite = Branche.Exploratrices;  break;
               case 'e': BrancheUnite = Branche.Eclaireurs;     break;
               case 'f': BrancheUnite = Branche.Intrepides;     break;
               case 'g': BrancheUnite = Branche.Pionniers;      break;
               case 'h': BrancheUnite = Branche.Routiers;       break;
               case 'i': BrancheUnite = Branche.Aventuriers;    break;
               case 'j': BrancheUnite = Branche.Louveteaux2012; break;
               default:                                         break;
            }
         }
      }
      public string District { get; set; }
      public string Groupe { get; set; }
      public string Unite { get; set; }
      public Branche BrancheUnite { get; set; }
   }


}