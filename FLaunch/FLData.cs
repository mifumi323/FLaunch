using System;
using System.Collections.Generic;
using System.IO;
using util;

namespace FLaunch
{
    public class FLItem
    {
        public FLItem(string name, string file, string dir, string arguments, string comment, string tag)
        {
            this.name = name;
            this.file = file;
            this.dir = dir;
            this.score = 1.0;
            this.arguments = arguments;
            this.comment = comment;
            this.Tag = tag;
        }
        public FLItem(string line)
        {
            string[] item = line.Split('\t');
            name = item[0];
            file = item[1];
            dir = item[2];
            score = Double.Parse(item[3]);
            date = DateTime.Parse(item[4]);
            arguments = item.Length > 5 ? item[5] : "";
            comment = item.Length > 6 ? item[6] : "";
            Tag = item.Length > 7 ? item[7] : "";
        }
        public override string ToString()
        {
            return
                name.Replace('\t', ' ') + "\t" +
                file.Replace('\t', ' ') + "\t" +
                dir.Replace('\t', ' ') + "\t" +
                score.ToString() + "\t" +
                date.ToString() + "\t" +
                arguments.Replace('\t', ' ') + "\t" +
                comment.Replace('\t', ' ') + "\t" +
                Tag.Replace('\t', ' ');
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType()) return false;
            FLItem o = (FLItem)obj;
            return o.name == name
                && o.file == file
                && o.dir == dir
                && o.score == score
                && o.date == date
                && o.arguments == arguments;
        }
        public override int GetHashCode() => base.GetHashCode();
        public void CopyFrom(FLItem from)
        {
            name = from.name;
            file = from.file;
            dir = from.dir;
            score = from.score;
            date = from.date;
            arguments = from.arguments;
            comment = from.comment;
            tag = from.tag;
        }

        public string name;
        public string file;
        public string dir;
        public double score;
        public DateTime date;
        public string arguments;
        public string comment;
        public string[] tag;

        public string Tag
        {
            get { return string.Join(sepalator, tag); }
            set { tag = value.Split(sepalators, StringSplitOptions.RemoveEmptyEntries); }
        }

        static readonly string sepalator = ",";
        static readonly char[] sepalators = new char[] { ',', ' ', '\t', '\r', '\n' };
    }

    public class FLList : LinkedList<FLItem> { }

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

        readonly FLList list = new FLList();

        delegate bool Operator(FLItem item);

        private FLData()
        {
            try
            {
                using (TextReader sr = new StreamReader(FileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        try { list.AddLast(new FLItem(line)); }
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
                    ShellLink sl = new ShellLink(p);
                    list.AddLast(new FLItem(
                        Path.GetFileNameWithoutExtension(p), p, sl.WorkingDirectory, "", sl.Description, ""
                        ));
                    return;
                }
                catch (Exception) { }
            }
            list.AddLast(new FLItem(Path.GetFileNameWithoutExtension(p), p, Path.GetDirectoryName(p), "", "", ""));
        }
        private void Save(Operator o)
        {
            using (TextWriter sw = new StreamWriter(FileName))
            {
                foreach (FLItem item in list)
                {
                    if (o(item)) sw.WriteLine(item.ToString());
                }
            }
        }
        private void Save() { Save(delegate (FLItem item) { return true; }); }

        public static void Add(string p)
        {
            FLData data = new FLData();
            data.InnerAdd(p);
            data.Save();
        }
        public static void Add(string[] p)
        {
            FLData data = new FLData();
            foreach (string q in p) data.InnerAdd(q);
            data.Save();
        }
        public static FLList Get()
        {
            FLData data = new FLData();
            return data.list;
        }
        public static FLItem[] GetArray()
        {
            FLList list = Get();
            FLItem[] array = new FLItem[list.Count];
            int i = 0;
            foreach (FLItem item in list) array[i++] = item;
            return array;
        }
        public static void Score(FLItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            FLData data = new FLData();
            double d = (double)data.list.Count / (data.list.Count + 1);
            data.Save(delegate (FLItem item2)
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
            FLData data = new FLData();
            data.Save(delegate (FLItem item2) { return !item.Equals(item2); });
        }
        public static void Replace(FLItem from, FLItem to)
        {
            FLData data = new FLData();
            data.Save(delegate (FLItem item)
            {
                if (item.Equals(from)) item.CopyFrom(to);
                return true;
            });
        }
    }
}
