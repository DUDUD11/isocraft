using Flat;
using isocraft;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isocraft
{
    //DDA 알고리즘 사용
    public class RayCasting
    {
        private static RayCasting _instance;
        private bool[,] simpleMap;


        public static double GetHitRate(double distance, double decayUnitDistance)
        {
            double baseDistance = decayUnitDistance; // 100% 기준 거리
            double targetRate = 0.33;                // 2배 거리에서의 목표 명중률

            double k = Math.Log(1 / targetRate) / decayUnitDistance;
            double rate = Math.Exp(-k * Math.Max(0, distance - baseDistance));

            return Math.Min(100,100.0 * rate);
        }

        public static bool Can_Detect(Point start, Point end)
        {

            Point[,] data = Coordinate.Instance.Info;

            float dx = end.X - start.X;
            float dy = end.Y - start.Y;

            float steps = Math.Max(Math.Abs(dx), Math.Abs(dy));

            float xInc = dx / steps;
            float yInc = dy / steps;

            float x = start.X;
            float y = start.Y;

            for (int i = 0; i < steps; i++)
            {
                x += xInc;
                y += yInc;

                if ((int)x == (int)start.X && (int)y == (int)start.Y)
                {
                    continue;
                }

                if ((int)x == end.X && (int)y == end.Y) break;



                if (data[(int)y, (int)x].X != (int)GameEnums.Type.None)
                {
                    switch (data[(int)y, (int)x].X)
                    {
   
                        case ((int)GameEnums.Type.Buliding):

                            return false;


                        case ((int)GameEnums.Type.Wall):

                            return false;
                            

                            //case ((int)GameEnums.Type.Buliding):

                            //    //현재는 그냥 불가능으로 넘기자
                            //    return 100;
                            //    break;
                            //default:
                            //    throw new Exception("err");

                    }



                }


            }

            return true;

        }



        public int Shooting_Coverage(Vector2 start,Point end)
        {

            int ret = 0;

            Point[,] data = Coordinate.Instance.Info;

            float dx = end.X - start.X;
            float dy = end.Y - start.Y;

            float steps = Math.Max(Math.Abs(dx), Math.Abs(dy)) * 2;

            float xInc = dx / steps;
            float yInc = dy / steps;

            float x = start.X;
            float y = start.Y;

            for (int i = 0; i <= steps; i++)
            {
                x += xInc;
                y += yInc;

             

                if (ret >= 100) return 100;

                if ((int)x == (int)start.X && (int)y == (int)start.Y)
                {
                    continue;
                }

                if ((int)x == end.X && (int)y == end.Y) break;



                if (data[(int)y, (int)x].X != (int)GameEnums.Type.None)
                {
                    switch (data[(int)y, (int)x].X)
                    {
                        case ((int)GameEnums.Type.Hero):

                            //현재는 그냥 불가능으로 넘기자
                            return 100;
                          //  break;
                        case ((int)GameEnums.Type.Enemy):

                            //현재는 그냥 불가능으로 넘기자
                            return 100;
                          //  break;

                        case ((int)GameEnums.Type.Buliding):

                            CoverableObject coverable=
                                EntityManager.FindObject(new Point((int)x,(int)y));

                            ret+=(int)coverable.Covering_Vec(new Vector2(x, y));

                          
                            
                            break;

                        //case ((int)GameEnums.Type.Buliding):

                        //    //현재는 그냥 불가능으로 넘기자
                        //    return 100;
                        //    break;
                        default:
                            throw new Exception("err");

                    }
                
                
                
                }


            }





            return ret;
        
        }



       public static RayCasting Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RayCasting();
                return _instance;
            }
        }
    }
}

 

