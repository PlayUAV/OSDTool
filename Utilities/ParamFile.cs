using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using MissionPlanner.Controls;

namespace MissionPlanner.Utilities
{
    public class ParamFile
    {
        private static readonly ILog log =
          LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Hashtable loadParamFile(string Filename)
        {
            Hashtable param = new Hashtable();

            StreamReader sr = new StreamReader(Filename);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                if (line.Contains("NOTE:") || line.StartsWith("#"))
                {
                    //CustomMessageBox.Show(line, "Saved Note");
                    continue;  
                }

                if (!line.Contains("="))
                {
                    //CustomMessageBox.Show(line, "Saved Note");
                    //continue;
                    string str_warn = "The setting file is saved in old format. Please manual open the file:\n 1:Remove the comments in the first line.\n 2:Replace all the ',' to '='.\n 3:Save the file and load again.";
                    CustomMessageBox.Show(str_warn, "Warning!");
                    break;

                }


                //string[] items = line.Split(new char[] { ' ', ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                string[] items = line.Split(new char[] { ' ', '=', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (items.Length != 2)
                    continue;

                string name = items[0];
                float value = 0;
                try
                {
                    value = float.Parse(items[1], System.Globalization.CultureInfo.InvariantCulture);// new System.Globalization.CultureInfo("en-US"));
                }
                catch (Exception ex) { log.Error(ex); throw new FormatException("Invalid number on param " + name + " : " + items[1].ToString()); }

                if (name == "SYSID_SW_MREV")
                    continue;
                if (name == "WP_TOTAL")
                    continue;
                if (name == "CMD_TOTAL")
                    continue;
                if (name == "FENCE_TOTAL")
                    continue;
                if (name == "SYS_NUM_RESETS")
                    continue;
                if (name == "ARSPD_OFFSET")
                    continue;
                if (name == "GND_ABS_PRESS")
                    continue;
                if (name == "GND_TEMP")
                    continue;
                if (name == "CMD_INDEX")
                    continue;
                if (name == "LOG_LASTFILE")
                    continue;
                if (name == "FORMAT_VERSION")
                    continue;

                param[name] = value;
            }
            sr.Close();

            return param;
        }

        public static void SaveParamFile(string fn, Hashtable paramlist)
        {
            StreamWriter sw = new StreamWriter(File.Open(fn, FileMode.Create));
            //string input = DateTime.Now + " Comments : ";
            //InputBox.Show("Custom Note", "Enter your Notes/Frame Type etc", ref input);
            //if (input != "")
            //    sw.WriteLine("#NOTE: " + input.Replace(',', '|'));

            var list = new SortedList(paramlist);

            foreach (var item in list.Keys)
            {
                float value = float.Parse(paramlist[item].ToString());

                //sw.WriteLine(item + "," + value.ToString(new System.Globalization.CultureInfo("en-US")));
                sw.WriteLine(item + "=" + value.ToString(new System.Globalization.CultureInfo("en-US")));
            }
            sw.Close();
        }
    }
}
