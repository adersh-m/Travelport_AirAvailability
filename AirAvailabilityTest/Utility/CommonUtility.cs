﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AirAvailabilityTest.Utility
{
    class CommonUtility
    {
        //private static ConfigurationManager _moduleConfiguration;

        #region public method
        public static string GetConfigValue(string Key)
        {
            return ConfigurationManager.AppSettings[Key];
        }


        #endregion
    }
}
