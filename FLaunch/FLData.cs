using System;
using System.Collections.Generic;
using System.IO;
using util;

namespace FLaunch
{
    public class FLData
    {
        private static string myFileName = null;
        public static string FileName
        {
            get
            {
                if (myFileName == null) myFileName = Path.Combine(Program.UserAppDataPath, "List.tsv");
                return myFileName;
            }
        }

        readonly List<FLItem> list = new List<FLItem>();

        private FLData()
        {
            try
            {
                using (TextReader sr = new StreamReader(FileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        try { list.Add(new FLItem(line)); }
                        catch (Exception) { }
                    }
                }
            }
            catch (FileNotFoundException) { }
        }
        private void InnerAdd(string p)
        {
            if (p.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var sl = new ShellLink(p);
                    list.Add(new FLItem(
                        Path.GetFileNameWithoutExtension(p), p, sl.WorkingDirectory, "", sl.Description, ""
                        ));
                    return;
                }
                catch (Exception) { }
            }
            list.Add(new FLItem(Path.GetFileNameWithoutExtension(p), p, Path.GetDirectoryName(p), "", "", ""));
        }
        private void Save(Func<FLItem, bool> o)
        {
            using (TextWriter sw = new StreamWriter(FileName))
            {
                foreach (var item in list)
                {
                    if (o(item)) sw.WriteLine(item.ToString());
                }
            }
        }
        private void Save()
        {
            Save(item => true);
        }

        public static void Add(string p)
        {
            var data = new FLData();
            data.InnerAdd(p);
            data.Save();
        }
        public static void Add(string[] p)
        {
            var data = new FLData();
            foreach (string q in p) data.InnerAdd(q);
            data.Save();
        }
        public static List<FLItem> Get()
        {
            return new FLData().list;
        }
        public static FLItem[] GetArray()
        {
            return Get().ToArray();
        }
        public static void Score(FLItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var data = new FLData();
            var d = (double)data.list.Count / (data.list.Count + 1);
            data.Save(item2 =>
            {
                if (item.Equals(item2))
                {
                    item2.date = DateTime.Now;
                    item2.score++;
                }
                item2.score *= d;
                return true;
            });
        }
        public static void Delete(FLItem item)
        {
            new FLData().Save(item2 => !item.Equals(item2));
        }
        public static void Replace(FLItem from, FLItem to)
        {
            new FLData().Save(item =>
            {
                if (item.Equals(from)) item.CopyFrom(to);
                return true;
            });
        }
    }
}
