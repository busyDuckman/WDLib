/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WD_toolbox.AplicationFramework;

namespace WDLibApplicationFramework.AplicationFramework
{
    public abstract class AppConfig
    {
        protected string configSaveDir = Path.Combine(Application.UserAppDataPath, Application.ProductName);

        private AppConfig()
        {
            try
            {
                if (!Directory.Exists(configSaveDir))
                {
                    Directory.CreateDirectory(configSaveDir);
                }
            }
            catch (Exception ex)
            {
                WDAppLog.logException(ErrorLevel.TerminalError, ex);
            }
        }

        public virtual void loadConfig(string fileName)
        {

        }
        
        public virtual void saveConfig(string fileName)
        {
        }
    }
}
