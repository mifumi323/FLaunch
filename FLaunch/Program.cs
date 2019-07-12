using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

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
            Application.Run(new FormMain());
        }

        /// <summary>バージョンに依存しないユーザーのアプリケーションデータのパス</summary>
        public static string UserAppDataPath
        {
            get
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    "\\" + Application.CompanyName + "\\" + Application.ProductName;
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;
            }
        }

        private static string title = null;
        /// <summary>アプリケーションのタイトルを取得します。</summary>
        public static string Title
        {
            get
            {
                if (string.IsNullOrEmpty(title))
                {
                    title = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute))).Title;
                }
                return title;
            }
        }

        private static string copyright = null;
        /// <summary>アプリケーションの著作権情報を取得します。</summary>
        public static string Copyright
        {
            get
            {
                if (string.IsNullOrEmpty(copyright))
                {
                    copyright = ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute))).Copyright;
                }
                return copyright;
            }
        }
    }
}
