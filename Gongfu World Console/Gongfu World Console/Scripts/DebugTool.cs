using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gongfu_World_Console.Scripts
{
    public struct DamageDebug
    {
        public bool IsDebugCsvInit;

        public string DInfoString;

        public int DInfoCount;

        public int AttackCount;
    }

    public static class DebugTool
    {
        public static int PenetrateOutCount = 0;

        public static int OverkillCount = 0;

        public static DamageDebug DmgDebug = new DamageDebug();

        public static DamageWorker DmgWorker = new DamageWorker();


        public static void Debug_DeathMatch_Once(Character ch1, Character ch2)
        {
            int turnCount = Debug_DeathMatch(ch1, ch2);

            Console.WriteLine($"\nTotal turn = {turnCount}");
            Console.WriteLine($"PenetrateOutCount = {DebugTool.PenetrateOutCount}");
            Console.WriteLine("{0} HP = {1} / {2}, {3}", ch1.Name, ch1.Health.Hp, ch1.Health.MaxHp, ch1.IsAlive);
            Console.WriteLine("{0} HP = {1} / {2}, {3}\n", ch2.Name, ch2.Health.Hp, ch2.Health.MaxHp, ch2.IsAlive);
        }

        private static Gongfa Debug_RandGongfa(Character ch)
        {
            var gfDict = ch.Gongfa.GongfaDict;
            List<Gongfa> gfList = new List<Gongfa>();

            if (gfDict.ContainsKey(GongfaTypeEnum.拳掌))
            {
                gfList.AddRange(gfDict[GongfaTypeEnum.拳掌].Values.ToList());
            }

            if (gfDict.ContainsKey(GongfaTypeEnum.腿法))
            {
                gfList.AddRange(gfDict[GongfaTypeEnum.腿法].Values.ToList());
            }

            if (gfDict.ContainsKey(GongfaTypeEnum.剑法))
            {
                gfList.AddRange(gfDict[GongfaTypeEnum.剑法].Values.ToList());
            }

            if (gfDict.ContainsKey(GongfaTypeEnum.刀法))
            {
                gfList.AddRange(gfDict[GongfaTypeEnum.刀法].Values.ToList());
            }

            //            int randValue = Utility.Rand.Next(0, gfList.Count);
            //
            //            return gfList[randValue];

            return gfList[0];
        }

        public static int Debug_DeathMatch(Character ch1, Character ch2)
        {
            bool turn = true;
            int turnCount = 0;
            while (ch1.IsAlive && ch2.IsAlive)
            {
                DebugTool.DmgDebug.AttackCount++;
                if (turn)
                {
                    DmgWorker.GongfaAttack(Debug_RandGongfa(ch1), ch2);
                }
                else
                {
                    DmgWorker.GongfaAttack(Debug_RandGongfa(ch2), ch1);
                }

                turn = !turn;
                turnCount++;
            }

            return turnCount;
        }

        public static int Debug_DeathMatch_Statistics(CharacterData ch1d, CharacterData ch2d, int round, bool outPut = true)
        {
            bool sente = true;
            int win = 0;
            long remainHpTotal1 = 0;
            long remainHpTotal2 = 0;
            long hp1 = 0, hp2 = 0;
            bool first = true;
            long turnTotal = 0;

            for (int i = 0; i < round; i++)
            {
                Character ch1 = new Character(ch1d);
                Character ch2 = new Character(ch2d);

                if (first)
                {
                    hp1 = ch1.Health.MaxHp;
                    hp2 = ch2.Health.MaxHp;
                    first = false;
                }

                if (sente)
                {
                    turnTotal += Debug_DeathMatch(ch1, ch2);
                }
                else
                {
                    turnTotal += Debug_DeathMatch(ch2, ch1);
                }

                if (ch1.IsAlive)
                {
                    win++;
                    remainHpTotal1 += ch1.Health.Hp;
                }
                else
                {
                    remainHpTotal2 += ch2.Health.Hp;
                }

                sente = !sente;
            }

            if (outPut)
            {
                Console.WriteLine($"\nPenetrateOutCount = {DebugTool.PenetrateOutCount}");
                Console.WriteLine($"\n{ch1d.Name} Vs {ch2d.Name}");
                Console.WriteLine($"Average Turn Num = {(double)turnTotal / round:0.00}");
                Console.WriteLine($"Win Rate：{ch1d.Name} = {(double)win / round:0.000%}, {ch2d.Name} = {1 - (double)win / round:0.000%}");
                Console.WriteLine($"Remain Hp Rate：{ch1d.Name} = {((double)remainHpTotal1 / (win * hp1)):0.000%}, {ch2d.Name} = {((double)remainHpTotal2 / ((round - win) * hp2)):0.000%}");
            }

            return win;
        }


        public static int Debug_ArmorTest_Once(CharacterData ch1d, CharacterData ch2d, int armor, int round, bool outPut)
        {            
            ch1d.Armor = ch2d.Armor = armor;
            return Debug_DeathMatch_Statistics(ch1d, ch2d, round, outPut);
        }

        public static void Debug_ArmorTest_Statistics(CharacterData ch1d, CharacterData ch2d, int armorMin, int armonMax, int round)
        {
            Logger.Csv.WriteLog($"Armor,{ch1d.Name},{ch2d.Name}");
            for (int i = armorMin; i <= armonMax; i++)
            {
                int win = Debug_ArmorTest_Once(ch1d, ch2d, i, round, false);
                Console.WriteLine(i);
                Logger.Csv.WriteLog($"{i},{win * 1.0 / round:0.000%},{(round - win) * 1.0 / round:0.000%}");
            }
        }







    }
}
