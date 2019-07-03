using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISCParser
{
   class MetriqueGroupe
   {
      public MetriqueGroupe(IdentifiantGroupe groupe, List<KeyValuePair<string, Membre>> membres)
      {
         EvaluerGroupe(groupe, membres);
      }

      public IdentifiantGroupe IdGroupe { get; set; }
      public int AnimateurActif { get; set; }
      public int MultiplePosteSup { get; set; }
      public int MultiplePoste { get; set; }
      public int VAJIncomplete { get; set; }
      public int PJIncomplete { get; set; }
      public int CCIncomplete { get; set; }

      public void EvaluerGroupe(IdentifiantGroupe groupe, List<KeyValuePair<string, Membre>> membres)
      {
         IdGroupe = groupe;
         AnimateurActif = membres.Where(m => m.Value.NbPostes > 0).Count();
         MultiplePosteSup = membres.Where(m => m.Value.NbPostesSup > 1).Count();
         MultiplePoste = membres.Where(m => m.Value.NbPostes > 1).Count();
         VAJIncomplete = membres.Where(m => (m.Value.Vaj.Statut == VAJ.VAJStatut.NON_REMPLIE) || (m.Value.Vaj.Statut == VAJ.VAJStatut.INCOMPLETE)).Count();
         PJIncomplete = membres.Where(m => m.Value.PrioriteJeunesse == null).Count();
         CCIncomplete = membres.Where(m => m.Value.CodeConduite == null).Count();
      }

   }
}
