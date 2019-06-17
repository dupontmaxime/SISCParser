using System;

namespace SISCParser
{
   public class Fonction
   {
      public enum TypeDeFonction
      {
         AD,
         AE,
         AR,
         CA,
         CD,
         CJ,
         CN,
         CO,
         CR,
         DG,
         DI,
         FO,
         GA,
         GC,
         GP,
         ME,
         PD,
         PN,
         PR,
         RD,
         RE,
         RF,
         SD,
         SE,
         TR,
         VP,
      };

      public string[] TypeDeFonctionDescription = {
         "Administrateur", //AD
         "Animateur", //AE
         "Animateur Responsable", //AR
         "Commisaire Adjoint", //CA
         "Commisaire District", //CD
         "Conseiller Juridique", //CJ
         "Commisaire National", //CN
         "Commissaire", //CO
         "Coordonateur", //CR
         "Directeur Général", //DG
         "Directeur", //DI
         "Formateur", //FO
         "Chef de Groupe Adjoint", //GA
         "Chef de Groupe", //GC
         "President de Groupe", //GP
         "Membre", //ME
         "President District", //PD
         "President National", //PN
         "President", //PR
         "RD", //RD
         "Responsable", //RE
         "Responsabel Formation", //RF
         "Secrétaire District", //SD
         "Secrétaire", //SE
         "Tresorier", //TR
         "Vice-président" //VP
      };

      public string TitreFonction()
      {
         return TypeDeFonctionDescription[(int)typeDeFonction];
      }

      public TypeDeFonction typeDeFonction;
      public Fonction(string fonction)
      {
         switch (fonction)
         {
            case nameof(TypeDeFonction.AD): typeDeFonction = TypeDeFonction.AD; break;
            case nameof(TypeDeFonction.AE): typeDeFonction = TypeDeFonction.AE; break;
            case nameof(TypeDeFonction.AR): typeDeFonction = TypeDeFonction.AR; break;
            case nameof(TypeDeFonction.CA): typeDeFonction = TypeDeFonction.CA; break;
            case nameof(TypeDeFonction.CD): typeDeFonction = TypeDeFonction.CD; break;
            case nameof(TypeDeFonction.CJ): typeDeFonction = TypeDeFonction.CJ; break;
            case nameof(TypeDeFonction.CN): typeDeFonction = TypeDeFonction.CN; break;
            case nameof(TypeDeFonction.CO): typeDeFonction = TypeDeFonction.CO; break;
            case nameof(TypeDeFonction.CR): typeDeFonction = TypeDeFonction.CR; break;
            case nameof(TypeDeFonction.DG): typeDeFonction = TypeDeFonction.DG; break;
            case nameof(TypeDeFonction.DI): typeDeFonction = TypeDeFonction.DI; break;
            case nameof(TypeDeFonction.FO): typeDeFonction = TypeDeFonction.FO; break;
            case nameof(TypeDeFonction.GA): typeDeFonction = TypeDeFonction.GA; break;
            case nameof(TypeDeFonction.GC): typeDeFonction = TypeDeFonction.GC; break;
            case nameof(TypeDeFonction.GP): typeDeFonction = TypeDeFonction.GP; break;
            case nameof(TypeDeFonction.ME): typeDeFonction = TypeDeFonction.ME; break;
            case nameof(TypeDeFonction.PD): typeDeFonction = TypeDeFonction.PD; break;
            case nameof(TypeDeFonction.PN): typeDeFonction = TypeDeFonction.PN; break;
            case nameof(TypeDeFonction.PR): typeDeFonction = TypeDeFonction.PR; break;
            case nameof(TypeDeFonction.RD): typeDeFonction = TypeDeFonction.RD; break;
            case nameof(TypeDeFonction.RE): typeDeFonction = TypeDeFonction.RE; break;
            case nameof(TypeDeFonction.RF): typeDeFonction = TypeDeFonction.RF; break;
            case nameof(TypeDeFonction.SD): typeDeFonction = TypeDeFonction.SD; break;
            case nameof(TypeDeFonction.SE): typeDeFonction = TypeDeFonction.SE; break;
            case nameof(TypeDeFonction.TR): typeDeFonction = TypeDeFonction.TR; break;
            case nameof(TypeDeFonction.VP): typeDeFonction = TypeDeFonction.VP; break;
            default: break;
         };

      }
   }
}