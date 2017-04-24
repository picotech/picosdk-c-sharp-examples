/**************************************************************************
 *
 * Filename: PS5000ABlockCapture.cs
 * 
 * Description:
 *   Provide the main entry point for the application.
 *   
 * Copyright (C) 2013 - 2017 Pico Technology Ltd. See LICENSE file for terms.
 *
 **************************************************************************/

using System;
using System.Windows.Forms;

namespace PS5000A
{
    static class PS5000ABlockCapture
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PS5000ABlockForm());
        }
    }
}
