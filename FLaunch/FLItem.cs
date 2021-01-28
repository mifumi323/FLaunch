using System;

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
            var item = line.Split('\t');
            name = item[0];
            file = item[1];
            dir = item[2];
            score = double.Parse(item[3]);
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
            var o = (FLItem)obj;
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
            get => string.Join(sepalator, tag);
            set => tag = value.Split(sepalators, StringSplitOptions.RemoveEmptyEntries);
        }

        public static readonly string sepalator = ",";
        public static readonly char[] sepalators = new char[] { ',', ' ', '\t', '\r', '\n' };
    }
}
