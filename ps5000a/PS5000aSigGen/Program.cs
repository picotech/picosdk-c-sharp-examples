/**************************************************************************
 *
 * Filename: Program.cs
 * 
 * Description:
 *  Provide the main entry point for the application.
 * 
 * Copyright (C) 2014-2018 Pico Technology Ltd. See LICENSE file for terms.
 *
 **************************************************************************/

using System;
using System.Windows.Forms;

namespace PS5000ASigGen
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AWG_SIGGEN());
        }
    }
}
