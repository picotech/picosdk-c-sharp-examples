/*******************************************************************************
 *
 * Filename: PicoHRDLCSGui.cs
 * 
 * Description:
 *  Provide the main entry point for the application.
 * 
 * Copyright © 2016-2017 Pico Technology Ltd. See LICENSE file for terms.
 *
 ******************************************************************************/

using System;
using System.Windows.Forms;

namespace PicoHRDLGui
{
    static class PicoHRDLCSGui
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PicoHRDLCSGuiForm());
        }
    }
}
