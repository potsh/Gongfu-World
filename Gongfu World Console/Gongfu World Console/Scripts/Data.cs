using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Gongfu_World_Console.Scripts
{
    public static class Data
    {

        public static Dictionary<object, GongfaDef> GongfaDefData;

        public static Dictionary<object, BodyPartDef> BodyPartDefData;

        public static Dictionary<object, CharacterData> CharacterTableData;



        public static bool LoadData()
        {
            GongfaDefData = (Dictionary<object, GongfaDef>) CsvUtil<GongfaDef>.LoadObjectsToDict<GongfaDef>(Find.DataCsvPath + "GongfaDef.csv");
            if (GongfaDefData == null)
            {
                return false;
            }

            BodyPartDefData = (Dictionary<object, BodyPartDef>)CsvUtil<BodyPartDef>.LoadObjectsToDict<BodyPartDef>(Find.DataCsvPath + "BodyPartDef.csv");
            if (BodyPartDefData == null)
            {
                return false;
            }

            CharacterTableData = (Dictionary<object, CharacterData>)CsvUtil<CharacterData>.LoadObjectsToDict<CharacterData>(Find.DataCsvPath + "CharacterData.csv");
            if (CharacterTableData == null)
            {
                return false;
            }

            return true;
        }

        public static bool CheckData()
        {
            bool ret = CsvUtil<GongfaDef>.CheckCorrectness(GongfaDefData.Values);

            if (!CsvUtil<BodyPartDef>.CheckCorrectness(BodyPartDefData.Values))
            {
                ret = false;
            }

            if (!CsvUtil<CharacterData>.CheckCorrectness(CharacterTableData.Values))
            {
                ret = false;
            }

            return ret;
        }
    }
}
