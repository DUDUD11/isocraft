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

            for (int i = 0; i <= steps; i++)
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
                                EntityManager.FindObject(new Point((int)y,(int)x));

                            ret+=(int)coverable.Covering_Vec(new Vector2(y, x));

                          
                            
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
public class BFS
{
    private static BFS _instance;
    private bool[,] simpleMap;
    private int range;
    private int enemy_bfs_val;
    private Point enemy_point;

    public int[,] bfsMap;

    private int[] h = { -1, 1, 0, 0 };
    private int[] h2 = { 0, 0, -1, 1 };

    public static bool Enemy_Can_MeleeAttack(Point point, out Heros hero)
    {
        hero = null;

        for (int i = 0; i < EntityManager.Heroes.Count; i++)
        {
            if (EntityManager.Heroes[i].Destroy) continue;

            float x = FlatMath.Distance(point.ToVector2(), EntityManager.Heroes[i].pos);

            if (x <= 1 + 0.05f)
            {
                hero = EntityManager.Heroes[i];

                return true;

            }


        }

        return false;

    }


    public bool Moveable(Point point)
    {

        //본인이면
        if (point.X == 0 && point.Y == 0) return false;
        // Direction(point);

        if (Math.Abs(point.X) > range || MathF.Abs(point.Y) > range)
        {
            return false;
        }

        if (bfsMap[point.Y + range, point.X + range] <= range)
        {


            return true;
        }
        return false;
    }


    // range(길찾기)는 1.5배로 할것이고
    // 실제 sight는 안에 들어오는지확인
    // for문을 이안에서 돌려야될거같은데
    // enemy의 point를 넣는다
    public bool Enemy_Can_Detect_360deg_melee(Point point, int sight, out Heros hero)
    {

        ReachAble(point, sight);

        hero = null;
        enemy_point = point;
        int val = int.MaxValue / 2;

        for (int i = 0; i < EntityManager.Heroes.Count; i++)
        {



            if (EntityManager.Heroes[i].Destroy) continue;

            Point tmp = point - EntityManager.Heroes[i].pos.ToPoint();

            if (Math.Abs(tmp.X) > range || MathF.Abs(tmp.Y) > range)
            {
                continue;
            }

            //도달할수없으면 detect 안되게 일단
            // 일단 list에 가까운순서로 공격하게된다 같이 어그로가끌리면


            for (int j = 0; j < 4; j++)
            {
                int tmp_y = range - tmp.Y + h[j];
                int tmp_x = range - tmp.X + h2[j];

                if (tmp_y < 0 || tmp_y >= Coordinate.Instance.height || tmp_x < 0 || tmp_x >= Coordinate.Instance.width) continue;

                if (bfsMap[tmp_y, tmp_x] < sight - 1)
                {
                    val = Math.Min(val, bfsMap[tmp_y, tmp_x]);


                }

            }

            if (val != int.MaxValue / 2)
            {
                enemy_bfs_val = val;
                hero = EntityManager.Heroes[i];
                return true;
            }








        }


        return false;
    }

    public Stack<Point> EnemyMeleeMove(Point point, int move, int act, int offset, out int ret, out Point fn_point)
    {
        int can_go = move * act;
        fn_point = Point.Zero;
        bool flag = false;
        Stack<Point> stack = new();
        Stack<Point> stack_ret = new();

        ret = enemy_bfs_val;

        Point point_adjust = enemy_point - new Point(range, range);

        stack.Push(new Point(range + point.X, range + point.Y));

        while (can_go != 0 && enemy_bfs_val != 0)
        {
            Point tmp = stack.Peek();



            Point[] pt = new Point[4];

            pt[0] = new Point(tmp.X - 1, tmp.Y);
            pt[1] = new Point(tmp.X + 1, tmp.Y);
            pt[2] = new Point(tmp.X, tmp.Y + 1);
            pt[3] = new Point(tmp.X, tmp.Y - 1);

            for (int i = 0; i < 4; i++)
            {

                if (pt[i].X >= 0 && pt[i].X <= range * 2 && pt[i].Y >= 0 && pt[i].Y <= range * 2
                    && bfsMap[pt[i].Y, pt[i].X] == enemy_bfs_val)
                {

                    stack.Push(pt[i]);
                    enemy_bfs_val--;

                    if (bfsMap[pt[i].Y, pt[i].X] <= can_go)
                    {
                        can_go--;

                        if (!flag)
                        {
                            flag = true;
                            fn_point = pt[i] + point_adjust;


                        }

                        stack_ret.Push(pt[i] + point_adjust);
                    }

                }

            }

        }



        return stack_ret;




    }


    public Stack<Point> Move(Point point)
    {
        Stack<Point> stack = new();

        int val = bfsMap[range + point.Y, range + point.X];

        stack.Push(new Point(range + point.X, range + point.Y));


        while (val != 0)
        {
            Point tmp = stack.Peek();

            Point[] pt = new Point[4];

            pt[0] = new Point(tmp.X - 1, tmp.Y);
            pt[1] = new Point(tmp.X + 1, tmp.Y);
            pt[2] = new Point(tmp.X, tmp.Y + 1);
            pt[3] = new Point(tmp.X, tmp.Y - 1);

            for (int i = 0; i < 4; i++)
            {

                if (pt[i].X >= 0 && pt[i].X <= range * 2 && pt[i].Y >= 0 && pt[i].Y <= range * 2
                    && bfsMap[pt[i].Y, pt[i].X] == val - 1)
                {

                    stack.Push(pt[i]);
                    val--;
                }

            }

        }

        // 첫경로 삭제
        stack.Pop();

        return stack;

    }

    #region
    public int Direction(Point Start, Point End)
    {

        return Direction(End - Start);


    }

    public int Direction(Point val)
    {
        return Direction(val.ToVector2());
    }

    public int Direction(Vector2 Start, Vector2 End)
    {
        return Direction(End - Start);
    }

    public int Direction(Vector2 val)
    {
        Vector2 de = new Vector2(0f, 1f);
        Vector2 vec = val;

        float dot = Vector2.Dot(de, vec);
        float cross = de.X * vec.Y - de.Y * vec.X;
        float angle;


        angle = MathF.Atan2(cross, dot);

        angle = angle * 4 / MathF.PI;
        int ret = (int)MathF.Round(angle);
        if (ret < 0) ret += 8;
        if (ret < 0 || ret >= 8)
            throw new Exception("direction err");

        return (ret + 5) % 8;





    }
    #endregion


    private void GetMap(Point point, int range)
    {
        simpleMap = new bool[2 * range + 1, 2 * range + 1];
        bfsMap = new int[2 * range + 1, 2 * range + 1];
        this.range = range;
        enemy_bfs_val = 0;

        for (int i = 0; i < range * 2 + 1; i++)
        {
            for (int j = 0; j < range * 2 + 1; j++)
            {
                bfsMap[i, j] = int.MaxValue / 2;

            }

        }

        for (int i = -range; i < range + 1; i++)
        {
            for (int j = -range; j < range + 1; j++)
            {

                Point tmp = new Point(point.X + j, point.Y + i);

                if (tmp.X < 0 || tmp.Y < 0 || tmp.X >= Coordinate.Instance.width || tmp.Y >= Coordinate.Instance.height)
                {
                    continue;
                }

                if (Coordinate.Instance.Info[tmp.Y, tmp.X].X == (int)GameEnums.Type.None && Coordinate.Instance.TileMapping[tmp.Y, tmp.X])
                {
                    simpleMap[i + range, j + range] = true;

                }
                else
                {
                    simpleMap[i + range, j + range] = false;
                }
            }

        }

    }

    //sight는 실제sight에 1.5배로 타협
    //public int[,] Enemy_Move(Point point, int sight, int range, int act)
    //{
    //    ReachAble(point, sight);

    //    if (Moveable(point,out int cost))
    //    {
    //        if (cost > range&& cost <=range)
    //        {


    //        }

    //        else
    //        { 


    //        }



    //    }

    //    else
    //    { 
    //        // 감시모드

    //    }

    //}





    public int[,] ReachAble(Point point, int range)
    {
        GetMap(point, range);

        Queue<Point> queue = new();
        bfsMap[range, range] = 0;

        queue.Enqueue(new Point(range, range));


        while (queue.Count != 0)
        {



            Point center = queue.Dequeue();

            int val = bfsMap[center.Y, center.X];

            if (val >= range) continue;

            Point[] pt = new Point[4];


            pt[0] = new Point(center.X - 1, center.Y);
            pt[1] = new Point(center.X + 1, center.Y);
            pt[2] = new Point(center.X, center.Y + 1);
            pt[3] = new Point(center.X, center.Y - 1);

            for (int i = 0; i < 4; i++)
            {

                if (pt[i].X >= 0 && pt[i].X <= range * 2 && pt[i].Y >= 0 && pt[i].Y <= range * 2
                    && simpleMap[pt[i].Y, pt[i].X] && bfsMap[pt[i].Y, pt[i].X] == int.MaxValue / 2)
                {
                    bfsMap[pt[i].Y, pt[i].X] = val + 1;
                    queue.Enqueue(pt[i]);
                }

            }

        }

        return bfsMap;
    }
}


 

