﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace FLaunch
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                FLData.Add(args);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        /// <summary>バージョンに依存しないユーザーのアプリケーションデータのパス</summary>
        public static string UserAppDataPath
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    "\\" + Application.CompanyName + "\\" + Application.ProductName;
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;
            }
        }
    }
}