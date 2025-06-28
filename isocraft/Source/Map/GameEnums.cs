
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace isocraft
{
    public static class GameEnums
    {
        #region data

        public static int[,] b_1x1 = { { 100 } };
        public static int[,] b_2x2_0_0_half = { { 50, 100 }, {100, 100 } };
        public static int[,] b_2x2_0_0_3quarter = { { 75, 100 }, { 100, 100 } };
        public static int[,] b_2x2 = { { 100, 100 }, {100, 100 } };

        public static int[,] b_3x2 = { { 100, 100, 100 }, { 100, 100, 100 } };
        public static int[,] b_2x3 = { { 100, 100 }, { 100, 100 }, { 100, 100 } };
        public static int[,] b_3x3 = { { 100, 100, 100 },{ 100, 100,100 },{ 100,100,100} };
        public static int[,] b_3x3_edge_half = { { 50, 100, 50 }, { 100, 100, 100 }, { 50, 100, 50 } };
        public static int[,] b_3x3_edge_33 = { { 33, 100, 33 }, { 100, 100, 100 }, { 33, 100, 33 } };

        public static int[,] b_5x5 = { { 100, 100, 100,100,100 }, { 100, 100, 100,100,100 }, { 100, 100, 100,100,100 }, { 100, 100, 100, 100, 100 }, { 100, 100, 100, 100, 100 } };

        #endregion

        public static Dictionary<string, int[,]> BuildingDictionary = new Dictionary<string, int[,]>();
        public static Dictionary<string, string> VillianDictionary = new Dictionary<string, string>();

        public static string BuildingDirPath = "Object\\Building\\";

        public static void Init()
        {
            Building_init();
            Villian_init();


        }

        public static int Villian_type(string str)
        {
            if (str.Equals("Character\\Editor\\Emafia")) return (int)GameEnums.Enemy.solidier;
            if (str.Equals("Character\\Editor\\zombie")) return (int)GameEnums.Enemy.zombie;

            throw new Exception("err");
        }

        public static Point Type_ret(SpriteEntity sprite)
        {
            if (sprite is Heros)
            {
                if (sprite is male)
                {
                    return new Point((int)Type.Hero, (int)Hero.male);
                }
            }

            else if (sprite is Villain)
            {
                if (sprite is Zombie)
                {
                    return new Point((int)Type.Enemy, (int)Enemy.zombie);
                }

                if (sprite is Solidier)
                {
                    return new Point((int)Type.Enemy, (int)Enemy.solidier);
                }
            }

            else if (sprite is Building)
            {
               
                    return new Point((int)Type.Buliding, (int)Objects.building);
          
            }

            throw new Exception("Check");


        }


        private static void Building_init()
        {
            //[0, 0] = 50;

            BuildingDictionary.Add("Building - size 1 - height 1 - type A", b_2x2_0_0_half);


            BuildingDictionary.Add("Building - size 1 - height 1 - type B.i", b_2x2);
            BuildingDictionary.Add("Building - size 1 - height 1 - type B.ii", b_2x2);
            BuildingDictionary.Add("Building - size 1 - height 1 - type C", b_2x2);

            BuildingDictionary.Add("Building - size 2 - height 2 - type A.i", b_3x3);
            BuildingDictionary.Add("Building - size 2 - height 2 - typeA.ii", b_3x3);
            BuildingDictionary.Add("Building - size 2 - height 2 - typeA.iii", b_3x3);
            BuildingDictionary.Add("Building - size 2 - height 2 - typeA.iv", b_3x3);

            BuildingDictionary.Add("Building - Size 2 - height 2 - typeB -  white-i", b_3x3);
            BuildingDictionary.Add("Building - Size 2 - height 2 - typeB -  white-ii", b_3x3);
            BuildingDictionary.Add("Building - Size 2 - height 2 - typeB -  yellow-i", b_3x3);
            BuildingDictionary.Add("Building - Size 2 - height 2 - typeB - blue-i", b_3x3);
            BuildingDictionary.Add("Building - Size 2 - height 2 - typeB - blue-ii", b_3x3);
            BuildingDictionary.Add("Building - Size 2 - height 2 - typeB - yellow-ii", b_3x3);

            BuildingDictionary.Add("Building - Size 2 - height 2 - typeC - blue-i", b_2x2_0_0_3quarter);
            BuildingDictionary.Add("Building - Size 2 - height 2 - typeC - red-i", b_2x2_0_0_3quarter);
            BuildingDictionary.Add("Building - Size 2 - height 2 - typeC - white-i", b_2x2_0_0_3quarter);

            BuildingDictionary.Add("Building - Size 2 - height 2 - typeD - white-i", b_2x2);
            BuildingDictionary.Add("Building - Size 2 - height 2 - typeD - white-ii", b_2x2);

            BuildingDictionary.Add("Building - Size 2 - height 2 - typeE - white-i", b_2x2);
            BuildingDictionary.Add("Building - Size 2 - height 2 - typeE - white-ii", b_2x2);

            BuildingDictionary.Add("Building - size 2 - height 3 - typeA.i", b_2x2);
            BuildingDictionary.Add("Building - size 2 - height 3 - typeA.ii", b_2x2);
            BuildingDictionary.Add("Building - size 2 - height 3 - typeA.iii", b_2x2);
            BuildingDictionary.Add("Building - size 2 - height 3 - typeA.iv", b_2x2);
            BuildingDictionary.Add("Building - size 2 - height 3 - typeA.v", b_2x2);

            BuildingDictionary.Add("Building - size 2 - height 3 - typeB concrete-i", b_2x2);
            BuildingDictionary.Add("Building - size 2 - height 3 - typeB concrete-ii", b_2x2);
            BuildingDictionary.Add("Building - size 2 - height 3 - typeB white-ii", b_2x2);
            BuildingDictionary.Add("Building - size 2 - height 3 typeB - white-i", b_2x2);
            BuildingDictionary.Add("Building - size 2 - height 3 typeC - white-i", b_2x2);
            BuildingDictionary.Add("Building - size 2 - height 3 typeC - white-ii", b_2x2);
            BuildingDictionary.Add("Building - size 2 - height 3 typeD - white-i", b_2x2);
            BuildingDictionary.Add("Building - size 2 - height 3 typeD - white-ii", b_2x2);

            BuildingDictionary.Add("Building - size 2 - height 5 - typeB concrete-i", b_1x1);
            BuildingDictionary.Add("Building - size 2 - height 5 - typeB concrete-ii", b_1x1);
            BuildingDictionary.Add("Building - size 2 - height 5 - typeB white-i", b_1x1);
            BuildingDictionary.Add("Building - size 2 - height 5 - typeB white-ii", b_1x1);

            BuildingDictionary.Add("Building - size 2x3 - height 2 - TypeA.i", b_3x2);
            BuildingDictionary.Add("Building - size 2x3 - height 2 - TypeA.iv", b_3x2);
            BuildingDictionary.Add("Building - size 2x3 - height 2 - TypeD-i", b_3x2);
            BuildingDictionary.Add("Building - size 2x3 - height 3 - TypeB.i", b_3x2);
            BuildingDictionary.Add("Building - size 2x3 - height 3 - TypeB.ii", b_3x2);
            BuildingDictionary.Add("Building - size 2x3 - height 3 - TypeB.iii", b_3x2);
            BuildingDictionary.Add("Building - size 2x3 - height 3 - TypeC.i", b_3x2);
            BuildingDictionary.Add("Building - size 2x3 - height 3 - TypeC.iii", b_3x2);
            BuildingDictionary.Add("Building - size 2x3 - height 4 - TypeA.i", b_3x2);

            BuildingDictionary.Add("Building - size 2x3 - height 2 - TypeA.ii", b_2x3);
            BuildingDictionary.Add("Building - size 2x3 - height 2 - TypeA.iii", b_2x3);
            BuildingDictionary.Add("Building - size 2x3 - height 2 - TypeD-ii", b_2x3);
            BuildingDictionary.Add("Building - size 2x3 - height 3 - TypeB.iv", b_2x3);
            BuildingDictionary.Add("Building - size 2x3 - height 3 - TypeB.v", b_2x3);
            BuildingDictionary.Add("Building - size 2x3 - height 3 - TypeB.vi", b_2x3);
            BuildingDictionary.Add("Building - size 2x3 - height 3 - TypeC.ii", b_2x3);
            BuildingDictionary.Add("Building - size 2x3 - height 3 - TypeC.iv", b_2x3);
            BuildingDictionary.Add("Building - size 2x3 - height 4 - TypeA.ii", b_2x3);

            BuildingDictionary.Add("Building - size 3 - height 1 typeA - white-i", b_3x3_edge_half);

            BuildingDictionary.Add("Building - size 3 - height 3 - typeA", b_3x3);

            BuildingDictionary.Add("Building - size 3 - height 4 - type A - blue-i", b_3x3_edge_33);
            BuildingDictionary.Add("Building - size 3 - height 4 - type- white-ii", b_3x3_edge_33);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeA - blue-ii", b_3x3_edge_33);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeA - red-i", b_3x3_edge_33);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeA - red-ii", b_3x3_edge_33);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeA - white-i", b_3x3_edge_33);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeB - concrete-i", b_3x3_edge_33);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeB - red-i", b_3x3_edge_33);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeB - red-ii", b_3x3_edge_33);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeB - white-i", b_3x3_edge_33);

            BuildingDictionary.Add("Building - size 3 - height 4 - typeC - concrete-i", b_3x3);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeC - concrete-ii", b_3x3);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeC - red-i", b_3x3);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeC - red-ii", b_3x3);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeC - white-i", b_3x3);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeC - white-ii", b_3x3);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeC - white-iii", b_3x3);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeD - red-i", b_3x3);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeD - red-ii", b_3x3);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeD - white-i", b_3x3);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeD - white-ii", b_3x3);

            BuildingDictionary.Add("Building - size 3 - height 4 - typeE - white-i", b_3x3_edge_33);
            BuildingDictionary.Add("Building - size 3 - height 4 - typeE - white-ii", b_3x3_edge_33);

            BuildingDictionary.Add("Building - Size 3 - heihgt 2 - typeC - blue-i", b_3x3_edge_half);
            BuildingDictionary.Add("Building - Size 3 - heihgt 2 - typeC - red-i", b_3x3_edge_half);
            BuildingDictionary.Add("Building - Size 3 - heihgt 2 - typeC - white-i", b_3x3_edge_half);

            BuildingDictionary.Add("Building size 5 - heihgt 5 - typeA - red-i", b_5x5);
            BuildingDictionary.Add("Building size 5 - heihgt 5 - typeA - white-i", b_5x5);

            BuildingDictionary.Add("Crane - Base Tower - A1", b_1x1);
            BuildingDictionary.Add("Crane - Base Tower - A2", b_1x1);






            //BuildingDictionary["Building - size 1 - height 1 - type B.i"] = b_2x2;
            //BuildingDictionary["Building - size 1 - height 1 - type B.ii"] = b_2x2;
            //BuildingDictionary["Building - size 1 - height 1 - type C"] = b_2x2;

            ////----2x2




            //BuildingDictionary["Building - size 2 - height 2 - type A.i"] = b_3x3;
            //BuildingDictionary["Building - size 2 - height 2 - typeA.ii"] = b_3x3;
            //BuildingDictionary["Building - size 2 - height 2 - typeA.iii"] = b_3x3;
            //BuildingDictionary["Building - size 2 - height 2 - typeA.iv"] = b_3x3;

            //BuildingDictionary["Building - Size 2 - height 2 - typeB -  white-i"] = b_3x3;
            //BuildingDictionary["Building - Size 2 - height 2 - typeB -  white-ii"] = b_3x3;
            //BuildingDictionary["Building - Size 2 - height 2 - typeB -  yellow-i"] = b_3x3;
            //BuildingDictionary["Building - Size 2 - height 2 - typeB - blue-i"] = b_3x3;
            //BuildingDictionary["Building - Size 2 - height 2 - typeB - blue-ii"] = b_3x3;
            //BuildingDictionary["Building - Size 2 - height 2 - typeB - yellow-ii"] = b_3x3;

            ////-----3x3






            //// all       [0, 0] = 75;
            ////3개

            //BuildingDictionary["Building - Size 2 - height 2 - typeC - blue-i"] = b_2x2_0_0_3quarter;
            //BuildingDictionary["Building - Size 2 - height 2 - typeC - red-i"] = b_2x2_0_0_3quarter;
            //BuildingDictionary["Building - Size 2 - height 2 - typeC - white-i"] = b_2x2_0_0_3quarter;
            ////----2x2







            //BuildingDictionary["Building - Size 2 - height 2 - typeD - white-i"] = b_2x2;
            //BuildingDictionary["Building - Size 2 - height 2 - typeD - white-ii"] = b_2x2;

            ////----2x2




            //BuildingDictionary["Building - Size 2 - height 2 - typeE - white-i"] = b_2x2;
            //BuildingDictionary["Building - Size 2 - height 2 - typeE - white-ii"] = b_2x2;

            ////----2x2




            //BuildingDictionary["Building - size 2 - height 3 - typeA.i"] = b_2x2;
            //BuildingDictionary["Building - size 2 - height 3 - typeA.ii"] = b_2x2;
            //BuildingDictionary["Building - size 2 - height 3 - typeA.iii"] = b_2x2;
            //BuildingDictionary["Building - size 2 - height 3 - typeA.iv"] = b_2x2;
            //BuildingDictionary["Building - size 2 - height 3 - typeA.v"] = b_2x2;

            ////----2x2






            //BuildingDictionary["Building - size 2 - height 3 - typeB concrete-i"] = b_2x2;
            //BuildingDictionary["Building - size 2 - height 3 - typeB concrete-ii"] = b_2x2;
            //BuildingDictionary["Building - size 2 - height 3 - typeB white-ii"] = b_2x2;
            //BuildingDictionary["Building - size 2 - height 3 typeB - white-i"] = b_2x2;
            //BuildingDictionary["Building - size 2 - height 3 typeC - white-i"] = b_2x2;
            //BuildingDictionary["Building - size 2 - height 3 typeC - white-ii"] = b_2x2;
            //BuildingDictionary["Building - size 2 - height 3 typeD - white-i"] = b_2x2;
            //BuildingDictionary["Building - size 2 - height 3 typeD - white-ii"] = b_2x2;
            ////----2x2






            //BuildingDictionary["Building - size 2 - height 5 - typeB concrete-i"] = b_1x1;
            //BuildingDictionary["Building - size 2 - height 5 - typeB concrete-ii"] = b_1x1;
            //BuildingDictionary["Building - size 2 - height 5 - typeB white-i"] = b_1x1;
            //BuildingDictionary["Building - size 2 - height 5 - typeB white-ii"] = b_1x1;

            ////----1x1






            //BuildingDictionary["Building - size 2x3 - height 2 - TypeA.i"] = b_3x2;
            //BuildingDictionary["Building - size 2x3 - height 2 - TypeA.iv"] = b_3x2;
            //BuildingDictionary["Building - size 2x3 - height 2 - TypeD-i"] = b_3x2;
            //BuildingDictionary["Building - size 2x3 - height 3 - TypeB.i"] = b_3x2;
            //BuildingDictionary["Building - size 2x3 - height 3 - TypeB.ii"] = b_3x2;
            //BuildingDictionary["Building - size 2x3 - height 3 - TypeB.iii"] = b_3x2;
            //BuildingDictionary["Building - size 2x3 - height 3 - TypeC.i"] = b_3x2;
            //BuildingDictionary["Building - size 2x3 - height 3 - TypeC.iii"] = b_3x2;
            //BuildingDictionary["Building - size 2x3 - height 4 - TypeA.i"] = b_3x2;
            ////----3x2




            //BuildingDictionary["Building - size 2x3 - height 2 - TypeA.ii"] = b_2x3;
            //BuildingDictionary["Building - size 2x3 - height 2 - TypeA.iii"] = b_2x3;
            //BuildingDictionary["Building - size 2x3 - height 2 - TypeD-ii"] = b_2x3;
            //BuildingDictionary["Building - size 2x3 - height 3 - TypeB.iv"] = b_2x3;
            //BuildingDictionary["Building - size 2x3 - height 3 - TypeB.v"] = b_2x3;
            //BuildingDictionary["Building - size 2x3 - height 3 - TypeB.vi"] = b_2x3;
            //BuildingDictionary["Building - size 2x3 - height 3 - TypeC.ii"] = b_2x3;
            //BuildingDictionary["Building - size 2x3 - height 3 - TypeC.iv"] = b_2x3;
            //BuildingDictionary["Building - size 2x3 - height 4 - TypeA.ii"] = b_2x3;
            ////----2x3



            ////z=1,
            ////[50,100,50
            ////[100,100,100
            ////[50,100,50]
            //BuildingDictionary["Building - size 3 - height 1 typeA - white-i"] = b_3x3_edge_half;

            ////----3x3






            //BuildingDictionary["Building - size 3 - height 3 - typeA"] = b_3x3;
            ////----3x3





            ////[33,100,33
            ////[100,100,100
            ////[33,100,33]
            //// 여기부터 끝까지
            //BuildingDictionary["Building - size 3 - height 4 - type A - blue-i"] = b_3x3_edge_33;
            //BuildingDictionary["Building - size 3 - height 4 - type- white-ii"] = b_3x3_edge_33;
            //BuildingDictionary["Building - size 3 - height 4 - typeA - blue-ii"] = b_3x3_edge_33;
            //BuildingDictionary["Building - size 3 - height 4 - typeA - red-i"] = b_3x3_edge_33;
            //BuildingDictionary["Building - size 3 - height 4 - typeA - red-ii"] = b_3x3_edge_33;
            //BuildingDictionary["Building - size 3 - height 4 - typeA - white-i"] = b_3x3_edge_33;
            //BuildingDictionary["Building - size 3 - height 4 - typeB - concrete-i"] = b_3x3_edge_33;
            //BuildingDictionary["Building - size 3 - height 4 - typeB - red-i"] = b_3x3_edge_33;
            //BuildingDictionary["Building - size 3 - height 4 - typeB - red-ii"] = b_3x3_edge_33;
            //BuildingDictionary["Building - size 3 - height 4 - typeB - white-i"] = b_3x3_edge_33;

            ////----3x3





            //BuildingDictionary["Building - size 3 - height 4 - typeC - concrete-i"] = b_3x3;
            //BuildingDictionary["Building - size 3 - height 4 - typeC - concrete-ii"] = b_3x3;
            //BuildingDictionary["Building - size 3 - height 4 - typeC - red-i"] = b_3x3;
            //BuildingDictionary["Building - size 3 - height 4 - typeC - red-ii"] = b_3x3;
            //BuildingDictionary["Building - size 3 - height 4 - typeC - white-i"] = b_3x3;
            //BuildingDictionary["Building - size 3 - height 4 - typeC - white-ii"] = b_3x3;
            //BuildingDictionary["Building - size 3 - height 4 - typeC - white-iii"] = b_3x3;
            //BuildingDictionary["Building - size 3 - height 4 - typeD - red-i"] = b_3x3;
            //BuildingDictionary["Building - size 3 - height 4 - typeD - red-ii"] = b_3x3;
            //BuildingDictionary["Building - size 3 - height 4 - typeD - white-i"] = b_3x3;
            //BuildingDictionary["Building - size 3 - height 4 - typeD - white-ii"] = b_3x3;

            ////---3x3



            ////[33,100,33
            ////[100,100,100
            ////[33,100,33]
            //// 여기부터 끝까지

            //BuildingDictionary["Building - size 3 - height 4 - typeE - white-i"] = b_3x3_edge_33;
            //BuildingDictionary["Building - size 3 - height 4 - typeE - white-ii"] = b_3x3_edge_33;


            ////---3x3



            ////[50,100,50
            ////[100,100,100
            ////[50,100,50]
            //// 여기부터 끝까지

            //BuildingDictionary["Building - Size 3 - heihgt 2 - typeC - blue-i"] = b_3x3_edge_half;
            //BuildingDictionary["Building - Size 3 - heihgt 2 - typeC - red-i"] = b_3x3_edge_half;
            //BuildingDictionary["Building - Size 3 - heihgt 2 - typeC - white-i"] = b_3x3_edge_half;

            ////---3x3


            //BuildingDictionary["Building size 5 - heihgt 5 - typeA - red-i"] = b_5x5;
            //BuildingDictionary["Building size 5 - heihgt 5 - typeA - white-i"] = b_5x5;


            ////---5x5

            //BuildingDictionary["Crane - Base Tower - A1"] = b_1x1;
            //BuildingDictionary["Crane - Base Tower - A2"] = b_1x1;

            //---1x1

            var updated = new Dictionary<string, int[,]>();
           

            foreach (var kvp in BuildingDictionary)
            {
                string newKey = BuildingDirPath + kvp.Key;
                updated[newKey] = kvp.Value;
            }

            BuildingDictionary = updated;

        }

        private static void Villian_init()
        {
            VillianDictionary["Character\\Editor\\Emafia"] = "Character\\Animation\\MafiaEnemy\\mafiaE_default";

            VillianDictionary["Character\\Editor\\zombie"] = "Character\\Animation\\Zombie\\zombie_default";



        }


        public enum Type
        { 
            None = 0,
            Hero = 1,
            Enemy = 2,
            Buliding = 3,
            Trap = 4,
            Wall = 5,
        }

        public enum Hero
        { 
            male = 0,
        
        }

        public enum Enemy
        {
            zombie = 0,
            solidier = 1,

        }

        public enum Objects
        {
            building = 0,

        }



    }
}
