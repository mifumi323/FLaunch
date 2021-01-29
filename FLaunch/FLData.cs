using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            Load(FileName);
        }

        private FLData(string fileName)
        {
            Load(fileName);
        }

        private void Load(string fileName)
        {
            try
            {
                using TextReader sr = new StreamReader(fileName);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    try { list.Add(new FLItem(line)); }
                    catch (Exception) { }
                }
            }
            catch (FileNotFoundException) { }
        }

        private FLItem InnerAdd(string p)
        {
            if (p.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var sl = new ShortcutLink(p);
                    var slItem = new FLItem(
                        Path.GetFileNameWithoutExtension(p), p, sl.WorkingDirectory, "", sl.Description, ""
                        );
                    list.Add(slItem);

                    return slItem;
                }
                catch (Exception) { }
            }
            var normalItem = new FLItem(Path.GetFileNameWithoutExtension(p), p, Path.GetDirectoryName(p), "", "", "");
            list.Add(normalItem);

            return normalItem;
        }
        private void Save(Func<FLItem, bool> o)
        {
            using TextWriter sw = new StreamWriter(FileName);
            foreach (var item in list)
            {
                if (o(item)) sw.WriteLine(item.ToString());
            }
        }
        private void Save()
        {
            Save(item => true);
        }

        public static FLItem Add(string p)
        {
            var data = new FLData();
            var item = data.InnerAdd(p);
            data.Save();

            return item;
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

        public static void Merge(string fileName)
        {
            var data = new FLData();
            var add = new FLData(fileName);
            data.list.AddRange(add.list);
            data.Save();
        }

        public static void AddTag(FLItem item, string tag)
        {
            new FLData().Save(i =>
            {
                if (item.Equals(i))
                {
                    i.tag = i.tag.Union(new[] { tag }).OrderBy(t => t).ToArray();
                }
                return true;
            });
        }

        public static void RemoveTag(FLItem item, string tag)
        {
            new FLData().Save(i =>
            {
                if (item.Equals(i))
                {
                    i.tag = i.tag.Where(t => t != tag).ToArray();
                }
                return true;
            });
        }
    }
}
