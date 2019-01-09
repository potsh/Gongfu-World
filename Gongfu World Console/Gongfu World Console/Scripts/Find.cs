using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gongfu_World_Console.Scripts
{
    public static class Find
    {
        //public static string RootPath = "D:/__Study/Projects/Unity3d/Gongfu-World/Assets/";

        public static string RootPath = "../../../../Assets/";

        public static string LogPath => RootPath + "Log/";

        public static string DataPath => RootPath + "Data/";

        public static string DataExcelPath => DataPath + "excel/";

        public static string DataCsvPath => DataPath + "csv/";

        public const double FloatPrecision = 1e-6;
    }
}
