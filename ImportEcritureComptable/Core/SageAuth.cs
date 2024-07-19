using Objets100cLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportEcritureComptable.Core
{
    public class SageAuths
    {
        public static BSCIALApplication100c OpenOM()
        {
            BSCIALApplication100c OM_gesco = new BSCIALApplication100c();
            try
            {
                    OM_gesco.Name = Properties.Settings.Default.SageFile;
                    OM_gesco.Loggable.UserName = Properties.Settings.Default.SageUser;
                    OM_gesco.Loggable.UserPwd = Properties.Settings.Default.SagePassword;

            }
            catch (Exception ex)
            {
                OM_gesco = null;
                Console.WriteLine("Impossible d'ouvrir la connexion Objets Métiers Sage !");
                Console.WriteLine("Erreur : " + ex.ToString());
                Core.Log.WriteLog("Impossible d'ouvrir la connexion Objets Métiers Sage !");
                Core.Log.WriteLog("Erreur : " + ex.ToString());
            }
            return OM_gesco;
        }

    }
}
