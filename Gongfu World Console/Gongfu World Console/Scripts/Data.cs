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
        private static bool _dataLoaded = false;

        public static Dictionary<object, GongfaDef> GongfaDefData;

        public static Dictionary<object, BodyPartDef> BodyPartDefData;

        public static Dictionary<object, CharacterData> CharacterTableData;



        public static bool LoadData()
        {
            if (_dataLoaded)
            {
                return true;
            }

            GongfaDefData = (Dictionary<object, GongfaDef>) CsvUtil<GongfaDef>.LoadObjectsToDict<GongfaDef>(Find.DataCsvPath + "GongfaDef.csv");
            if (GongfaDefData == null)
            {
                return false;
            }
            foreach (var gongfa in GongfaDefData.Values)
            {
                gongfa.PostLoadData();
            }

            BodyPartDefData = (Dictionary<object, BodyPartDef>)CsvUtil<BodyPartDef>.LoadObjectsToDict<BodyPartDef>(Find.DataCsvPath + "BodyPartDef.csv");
            if (BodyPartDefData == null)
            {
                return false;
            }
            LinkAllBodyPartDefs();
            CalcCoverageRecursively(BodyPartDefData[BodyPartEnum.Body]);
            CalcCoverageAbsRecursively(BodyPartDefData[BodyPartEnum.Body]);
            

            CharacterTableData = (Dictionary<object, CharacterData>)CsvUtil<CharacterData>.LoadObjectsToDict<CharacterData>(Find.DataCsvPath + "CharacterData.csv");
            if (CharacterTableData == null)
            {
                return false;
            }

            _dataLoaded = true;

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

        private static void LinkAllBodyPartDefs()
        {
            foreach (BodyPartDef partDef in BodyPartDefData.Values)
            {
                if (partDef.ParentName == default(BodyPartEnum))
                {
                    continue;
                }
                partDef.Parent = BodyPartDefData[partDef.ParentName];
                BodyPartDefData[partDef.ParentName].Children.Add(partDef);
            }
        }

        private static void CalcCoverageRecursively(BodyPartDef part)
        {
            //BodyPartDef root = BodyPartDefData[BodyPartEnum.Body];
            float totalCoverageOfChildren = 0;
            foreach (BodyPartDef childPart in part.Children)
            {
                totalCoverageOfChildren += childPart.CoverageAbsWithChildren;
                CalcCoverageRecursively(childPart);
            }

            part.Coverage = 1 - totalCoverageOfChildren;
        }

        private static void CalcCoverageAbsRecursively(BodyPartDef part)
        {
            if (part.Parent == null)
            {
                part.CoverageAbsWithChildren = 1.0f;
                part.CoverageAbs = 0.0f;
            }
            else
            {
                part.CoverageAbsWithChildren = part.Parent.CoverageAbsWithChildren * part.CoverageWithChildren;
                part.CoverageAbs = part.Parent.CoverageAbsWithChildren * part.Coverage;

                foreach (var child in part.Children)
                {
                    CalcCoverageAbsRecursively(child);
                }                
            }
        }
    }
}
