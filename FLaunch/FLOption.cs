using System;
using System.IO;

namespace FLaunch
{
    class FLOption
    {
        private static string myFileName = null;
        public static string FileName
        {
            get
            {
                myFileName ??= Path.Combine(Program.UserAppDataPath, "Option.tsv");
                return myFileName;
            }
        }

        private bool dirty = false;
        public void Dirty() => dirty = true;

        private int myWidth = 0;
        public int Width
        {
            get => myWidth;
            set
            {
                if (myWidth != value)
                {
                    myWidth = value;
                    Dirty();
                }
            }
        }
        private int myHeight;
        public int Height
        {
            get => myHeight;
            set
            {
                if (myHeight != value)
                {
                    myHeight = value;
                    Dirty();
                }
            }
        }
        private bool myExpandEnvironmentVariables = true;
        public bool ExpandEnvironmentVariables
        {
            get => myExpandEnvironmentVariables;
            set
            {
                if (myExpandEnvironmentVariables != value)
                {
                    myExpandEnvironmentVariables = value;
                    Dirty();
                }
            }
        }

        public FLOption()
        {
            try
            {
                using TextReader sr = new StreamReader(FileName);
                var type = GetType();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    try
                    {
                        string[] item = line.Split('\t');
                        if (item.Length < 2) continue;

                        var pi = type.GetProperty(item[0]);
                        if (!pi.CanWrite) continue;

                        switch (Type.GetTypeCode(pi.PropertyType))
                        {
                            case TypeCode.Boolean:
                                pi.SetValue(this, bool.Parse(item[1]), null);
                                break;
                            case TypeCode.Byte:
                                pi.SetValue(this, byte.Parse(item[1]), null);
                                break;
                            case TypeCode.Char:
                                pi.SetValue(this, char.Parse(item[1]), null);
                                break;
                            case TypeCode.DBNull:
                                break;
                            case TypeCode.DateTime:
                                pi.SetValue(this, DateTime.Parse(item[1]), null);
                                break;
                            case TypeCode.Decimal:
                                pi.SetValue(this, decimal.Parse(item[1]), null);
                                break;
                            case TypeCode.Double:
                                pi.SetValue(this, double.Parse(item[1]), null);
                                break;
                            case TypeCode.Empty:
                                break;
                            case TypeCode.Int16:
                                pi.SetValue(this, short.Parse(item[1]), null);
                                break;
                            case TypeCode.Int32:
                                pi.SetValue(this, int.Parse(item[1]), null);
                                break;
                            case TypeCode.Int64:
                                pi.SetValue(this, long.Parse(item[1]), null);
                                break;
                            case TypeCode.Object:
                                pi.SetValue(this, item[1], null);
                                break;
                            case TypeCode.SByte:
                                pi.SetValue(this, sbyte.Parse(item[1]), null);
                                break;
                            case TypeCode.Single:
                                pi.SetValue(this, float.Parse(item[1]), null);
                                break;
                            case TypeCode.String:
                                pi.SetValue(this, item[1], null);
                                break;
                            case TypeCode.UInt16:
                                pi.SetValue(this, ushort.Parse(item[1]), null);
                                break;
                            case TypeCode.UInt32:
                                pi.SetValue(this, uint.Parse(item[1]), null);
                                break;
                            case TypeCode.UInt64:
                                pi.SetValue(this, ulong.Parse(item[1]), null);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception) { }
                }
            }
            catch (FileNotFoundException) { }
            dirty = false;
        }

        public void Save()
        {
            if (!dirty) return;
            using (TextWriter sw = new StreamWriter(FileName))
            {
                foreach (var pi in GetType().GetProperties())
                {
                    if (!pi.CanWrite) continue;
                    sw.WriteLine($"{pi.Name}\t{pi.GetValue(this, null)}");
                }
            }
            dirty = false;
        }
    }
}
