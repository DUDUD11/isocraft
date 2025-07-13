using Flat;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static isocraft.GameEnums;

namespace isocraft
{
    public class BFS
    {

        private struct Enemy_info
        {
            public int enemy_bfs_val;
            public Point enemy_point;

        }


        private static BFS _instance;
        private bool[,] simpleMap;
        public int[,] bfsMap;

        private int sight;
        public static Vector2[] shooting_helper = { new Vector2(-0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, -0.5f), new Vector2(0f, 0.5f) };
        public static SpriteEntity isReady = null;

        Enemy_info enemy_Info;

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

        public bool Enemy_Can_Detect_360deg_penetration_melee(Point point, int sight, SpriteEntity entity, out Heros hero)
        {

            ReachAble(point, sight,entity);

            hero = null;
            enemy_Info.enemy_point = point;
            int val = int.MaxValue / 2;

            for (int i = 0; i < EntityManager.Heroes.Count; i++)
            {

                if (EntityManager.Heroes[i].Destroy) continue;

                Point tmp = point - EntityManager.Heroes[i].pos.ToPoint();

                if (Math.Abs(tmp.X) > this.sight || MathF.Abs(tmp.Y) > this.sight)
                {
                    continue;
                }

                //도달할수없으면 detect 안되게 일단
                // 일단 list에 가까운순서로 공격하게된다 같이 어그로가끌리면


                for (int j = 0; j < 4; j++)
                {
                    int tmp_y = this.sight - tmp.Y + h[j];
                    int tmp_x = this.sight - tmp.X + h2[j];

                    if (tmp_y < 0 || tmp_y >= Coordinate.Instance.height || tmp_x < 0 || tmp_x >= Coordinate.Instance.width) continue;

                    if (bfsMap[tmp_y, tmp_x] <= sight )
                    {
                        val = Math.Min(val, bfsMap[tmp_y, tmp_x]);
                    }
                }

                if (val != int.MaxValue / 2)
                {
                    enemy_Info.enemy_bfs_val = val;
                    hero = EntityManager.Heroes[i];
                    return true;
                }
            }


            return false;
        }








        public bool Enemy_Can_Detect_90deg_ImPenetration_Range(Point point, Vector2 Dir,SpriteEntity entity, int sight, out Heros hero)
        {
            int limit = (int)(sight * 1.5f);

            Enemy_ReachAble(point, sight, limit,entity);

            hero = null;
            enemy_Info.enemy_point = point;
            

            Dir = Vector2.Normalize(Dir);


            for (int i = 0; i < EntityManager.Heroes.Count; i++)
            {

                if (EntityManager.Heroes[i].Destroy) continue;

                Point tmp = point - EntityManager.Heroes[i].pos.ToPoint();

                if (Math.Abs(tmp.X) > this.sight || MathF.Abs(tmp.Y) > this.sight)
                {
                    continue;
                }

                float cosTheta = Vector2.Dot(Dir, Vector2.Normalize(tmp.ToVector2()));
                double angle = Math.Acos(cosTheta) * (180.0 / Math.PI);

          

                // 0.7072 를 설정하면 45도까지 볼수있다
                if (Math.Abs(angle) > 45)
                {
                    Console.WriteLine("Deg Can't");
                    continue;
                }

                if (!RayCasting.Can_Detect(point, EntityManager.Heroes[i].pos.ToPoint()))
                {
                    Console.WriteLine("Wall Can't");
                    continue;
                }

                

                if (MathHelper.Distance((float)(this.sight - tmp.Y), (float)(this.sight - tmp.X)) > limit)
                {
                    Console.WriteLine("Distacne limit");
                    continue;
                }


                return true;

             
            }

            return false;
        }


        // stack 이 비었으면 두번공격하는것으로 간주
        public Stack<Point> EnemyRangeMove(Point point, int move, int act, int atkrange, int default_accuracy, int target_accuracy,
            out int ret,out int Hit_Rate, out Point fn_point,out Heros hero, out Vector2 _ShootingPos,out Villain.Strategy strategy)
        {

            bool flag = false;
            int can_go = move * act;

            ret = enemy_Info.enemy_bfs_val;
            Hit_Rate = 0;
            fn_point = Point.Zero;
            hero = null;
            _ShootingPos = Vector2.Zero;
            strategy = Villain.Strategy.Hold;

       
            Stack<Point> stack = new();
            Stack<Point> stack_ret = new();

            //바로 총을 쏠가능성이 있으면 쏜다
            if (Enemy_Can_RangeAttack(point,atkrange,default_accuracy,target_accuracy,out Heros _directShootHero,out Vector2 _directShootPos,out int _HitRate))
            {

                Hit_Rate = _HitRate;
                _ShootingPos = _directShootPos;
                hero = _directShootHero;

                strategy = Villain.Strategy.Shoot;
                return null;
        
            }
            // 한칸 움직여서 쏠수있는 곳중에서 가장 정확도 높은곳으로 이동


            int walk_and_shoot_max_hitpercent = 0;


            for (int i = sight-move; i <= sight+ move; i++)
            {
                for (int j = sight - move; j <= sight + move; j++)
                {
                    if (Math.Abs(sight - i) + Math.Abs(sight - j) > move) continue;

                    if (bfsMap[i, j] == int.MaxValue / 2) continue;

                    if (Enemy_Can_RangeAttack(new Point(j, i), atkrange, default_accuracy, target_accuracy, out Heros _varHero, out Vector2 _varshootPos, out int _varret))
                    {
                        //ret에 hitpercent저장해서 return

                        if (_varret > walk_and_shoot_max_hitpercent)
                        {
                            walk_and_shoot_max_hitpercent = _varret;

                            enemy_Info.enemy_bfs_val = bfsMap[i, j];

                            ret = enemy_Info.enemy_bfs_val;
                            Hit_Rate = _varret;

                            fn_point = new Point(j, i);
                            _ShootingPos = _directShootPos;
                            hero = _directShootHero;
                            if (act == 1)
                            {
                                strategy = Villain.Strategy.Move;
                            }
                            else
                            {
                                strategy = Villain.Strategy.Move_and_Shoot;
                            }
                        }
                    }
                }             
            }

         

            if (walk_and_shoot_max_hitpercent >= target_accuracy)
            {
                act--;
                can_go -= move;

                Point point_adjust = enemy_Info.enemy_point - new Point(sight, sight);
          
                stack.Push(fn_point);

                if (enemy_Info.enemy_bfs_val <= can_go)
                {
                    // 반드시 도달가능함
                    // enemy_Info.enemy_bfs_val 도 경로에 들어가야된다
                    can_go--;
                    flag = true;
                    fn_point += point_adjust;
                    enemy_Info.enemy_bfs_val--;
                }
                else
                {
                    throw new Exception("Logical Error");
                }

                    while (can_go != 0 && enemy_Info.enemy_bfs_val != 0)
                    {
                        Point tmp = stack.Peek();

                        Point[] pt = new Point[4];

                        pt[0] = new Point(tmp.X - 1, tmp.Y);
                        pt[1] = new Point(tmp.X + 1, tmp.Y);
                        pt[2] = new Point(tmp.X, tmp.Y + 1);
                        pt[3] = new Point(tmp.X, tmp.Y - 1);

                        //여기부터

                        for (int i = 0; i < 4; i++)
                        {
                           

                            if (pt[i].X >= 0 && pt[i].X <= sight * 2 && pt[i].Y >= 0 && pt[i].Y <= sight * 2
                                && bfsMap[pt[i].Y, pt[i].X] == enemy_Info.enemy_bfs_val)
                            {

                                stack.Push(pt[i]);
                                enemy_Info.enemy_bfs_val--;  
                                can_go--;
                                stack_ret.Push(pt[i] + point_adjust);  

                            }

                        }

                    }

                return stack_ret;


                // 행동력이 1이든 0이든 길찾아서 return
            }

            else
            {
                // 일단 그냥 대기하자 ..
                //return
                //
                strategy = Villain.Strategy.Hold;
                ret = 0;
                Hit_Rate = 0;

                return null;
            }

            throw new Exception("Logical Error");
        }


        // 그자리에서 쏠수있는가
        public static bool Enemy_Can_RangeAttack(Point point, int attackrange, int default_Accuracy,int target_Accuracy, out Heros hero, out Vector2 shooting_pos, out int hitPercent)
        {
            hero = null;
            shooting_pos = Vector2.Zero;
            int cover = 100;


            //거리가 안되므로 항상 hitrate는 100 이상이다.
            hitPercent = default_Accuracy;

            for (int i = 0; i < EntityManager.Heroes.Count; i++)
            {
                if (EntityManager.Heroes[i].Destroy) continue;

                float x = FlatMath.Distance(point.ToVector2(), EntityManager.Heroes[i].pos);

            


                //적군의 경우 range를 넘으면 안쏘게

                if (x >= attackrange) continue;
          

         
                {
                    int tmp_cover_first = RayCasting.Instance.Shooting_Coverage(point.ToVector2(), EntityManager.Heroes[i].pos.ToPoint());

                    if (tmp_cover_first < cover)
                    {
                        shooting_pos = point.ToVector2();
                        cover = tmp_cover_first;
                        hero = EntityManager.Heroes[i];
                    }


                    for (int j = 0; j < 4; j++)
                    {
                        int tmp_cover = RayCasting.Instance.Shooting_Coverage(point.ToVector2() + shooting_helper[j], EntityManager.Heroes[i].pos.ToPoint());

                        if (tmp_cover < cover)
                        {
                            shooting_pos = point.ToVector2() + shooting_helper[j];
                            cover = tmp_cover;
                            hero = EntityManager.Heroes[i];

                        }

                    }

                }

              
            }

            if (cover >= 100) return false;

            //거리가 안되므로 항상 hitrate는 100 이상이다.
            //int hit_percent = default_Accuracy * gethitrate;

            double cover_val = hitPercent * (cover / 100f);
            hitPercent = hitPercent  - (int)(cover_val);

            return hitPercent >= target_Accuracy;

        }


        public void Attack_Hero(int hit_percent, int damage, Heros hero)
        {
            int random = RandomHelper.RandomInteger(0, 100);
            if (100 - hit_percent <= random)
            {
                SoundController.SoundChange("9mm Single");
                hero.Get_Hit(damage);
            }

            else
            {
                SoundController.SoundChange("Shooting_Miss");
            }

        }

        public bool MousePoint_Moveable(Point point)
        {
  

            //본인이면
            if (point.X == 0 && point.Y == 0) return false;
           // Direction(point);

            if (Math.Abs(point.X) > sight || MathF.Abs(point.Y) > sight)
            {
                return false;
            }

            if (bfsMap[point.Y + sight, point.X + sight] <= sight)
            {
               

                return true;
            }
            return false;
        }

        public bool MousePoint_Attackable(Point hero_ptr,Point enemy_ptr,int attackrange,int default_Accuracy,
            out Villain Villain,out int hit_percent, out Vector2 shooting_pos)
        {

            hit_percent = default_Accuracy;
            shooting_pos = Vector2.Zero;
            Villain = EntityManager.FindEnemy(enemy_ptr);

            if (Villain == null || Villain.Destroy) return false;

            int cover = 100;
            {
                int tmp_cover_first = RayCasting.Instance.Shooting_Coverage(hero_ptr.ToVector2(), enemy_ptr);

                if (tmp_cover_first < cover)
                {
                    shooting_pos = hero_ptr.ToVector2();
                    cover = tmp_cover_first;

                }


                for (int i = 0; i < 4; i++)
                {
                    int tmp_cover = RayCasting.Instance.Shooting_Coverage(hero_ptr.ToVector2() + shooting_helper[i], enemy_ptr);

                    if (tmp_cover < cover)
                    {
                        shooting_pos = hero_ptr.ToVector2() + shooting_helper[i];
                        cover = tmp_cover;

                    }

                }

            }

            if (cover >= 100) return false;

            hit_percent = (int)(RayCasting.GetHitRate(Vector2.Distance(hero_ptr.ToVector2(), enemy_ptr.ToVector2()), attackrange)) * default_Accuracy;

            double cover_val = hit_percent * (cover / 100f);
            hit_percent = hit_percent/100 - (int)(cover_val);

            return true;
        }

        public void Attack_Enemy(int hit_percent, int damage, Villain villain)
        {
            int random = RandomHelper.RandomInteger(0, 100);


            if (100 - hit_percent <= random)
            {
                SoundController.SoundChange("9mm Single");
                villain.Get_Hit(damage);

            }

            else
            {
                SoundController.SoundChange("Shooting_Miss");
            }
        }

        // range(길찾기)는 1.5배로 할것이고
        // 실제 sight는 안에 들어오는지확인
        // for문을 이안에서 돌려야될거같은데
        // enemy의 point를 넣는다
        public bool Enemy_Can_Detect_360deg_melee(Point point, int sight,SpriteEntity entity, out Heros hero)
        {
            //sight가 5일때 가는거리가 sight이하면 갈수있도록


            ReachAble(point, sight,entity);

            hero = null;
            enemy_Info.enemy_point = point;
            int val = int.MaxValue / 2 ;

            for (int i = 0; i < EntityManager.Heroes.Count; i++)
            {

                if (EntityManager.Heroes[i].Destroy) continue;

                Point tmp = point - EntityManager.Heroes[i].pos.ToPoint();

             


                if (Math.Abs(tmp.X) > this.sight || MathF.Abs(tmp.Y) > this.sight)
                {
                    continue;
                }

                //도달할수없으면 detect 안되게 일단
                // 일단 list에 가까운순서로 공격하게된다 같이 어그로가끌리면


                for (int j = 0; j < 4; j++)
                {
                    int tmp_y = this.sight - tmp.Y + h[j];
                    int tmp_x = this.sight - tmp.X + h2[j];

                    if (tmp_y < 0 || tmp_y > sight*2 || tmp_x < 0 || tmp_x > sight*2) continue;

                    if (bfsMap[tmp_y, tmp_x] <= sight)
                    {
                    
                        val = Math.Min(val, bfsMap[tmp_y, tmp_x]);         
                    }
                }

                if (val != int.MaxValue / 2)
                {
                    enemy_Info.enemy_bfs_val = val;
                    hero = EntityManager.Heroes[i];
                    return true;
                }
            }


            return false;
        }

        public Stack<Point> EnemyMeleeMove(Point point, int move,int act,out int ret,out Point fn_point)
        {
            // 상대위치까지의 거리에 -1 을 sight로 정했으니 
            // 상하좌우에 enemy_Info.enemy_bfs_val 값이 있음
            int can_go = move * act;
            fn_point = Point.Zero;
            bool flag = false;
            Stack<Point> stack = new();
            Stack<Point> stack_ret = new();

            ret = enemy_Info.enemy_bfs_val;

            Point point_adjust = enemy_Info.enemy_point - new Point(sight, sight);

            stack.Push(new Point(sight + point.X, sight + point.Y));

            while (can_go != 0 && enemy_Info.enemy_bfs_val !=0)
            {
                Point tmp = stack.Peek();

            

                Point[] pt = new Point[4];

                pt[0] = new Point(tmp.X - 1, tmp.Y);
                pt[1] = new Point(tmp.X + 1, tmp.Y);
                pt[2] = new Point(tmp.X, tmp.Y + 1);
                pt[3] = new Point(tmp.X, tmp.Y - 1);

                for (int i = 0; i < 4; i++)
                {

                    if (pt[i].X >= 0 && pt[i].X <= sight * 2 && pt[i].Y >= 0 && pt[i].Y <= sight * 2
                        && bfsMap[pt[i].Y, pt[i].X] == enemy_Info.enemy_bfs_val)
                    {

                        stack.Push(pt[i]);
                        enemy_Info.enemy_bfs_val--;
             
                        if (bfsMap[pt[i].Y, pt[i].X] <= can_go)
                        {
                            can_go--;

                            if (!flag)
                            {
                                flag = true;
                                fn_point = pt[i] + point_adjust;


                            }

                            stack_ret.Push(pt[i]+ point_adjust);
                        }

                    }

                }

            }

      

            return stack_ret;

        }


        public Stack<Point> Move(Point point, out int cost)
        {
        
            Stack<Point> stack = new();

            int val = bfsMap[sight + point.Y,sight + point.X];
            cost = val;
            stack.Push(new Point(sight + point.X, sight + point.Y));
           

            while (val!=0)
            {
                Point tmp = stack.Peek();

                Point[] pt = new Point[4];

                pt[0] = new Point(tmp.X - 1, tmp.Y);
                pt[1] = new Point(tmp.X + 1, tmp.Y);
                pt[2] = new Point(tmp.X, tmp.Y + 1);
                pt[3] = new Point(tmp.X, tmp.Y - 1);

                for (int i = 0; i < 4; i++)
                {

                    if (pt[i].X >= 0 && pt[i].X <= sight * 2 && pt[i].Y >= 0 && pt[i].Y <= sight * 2
                        && bfsMap[pt[i].Y, pt[i].X] == val-1)
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
        
            return Direction(End-Start);


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
            if(ret<0 || ret>=8)
            throw new Exception("direction err");

            return (ret + 5) % 8;
        }

        public static Vector2 RevDirection(int dir)
        {
            switch (dir)
            {
                case 5: return new Vector2(0, -1);    
                case 6: return new Vector2(1, -1);    
                case 7: return new Vector2(1, 0);     
                case 0: return new Vector2(1, 1);     
                case 1: return new Vector2(0, 1);    
                case 2: return new Vector2(-1, 1);    
                case 3: return new Vector2(-1, 0);    
                case 4: return new Vector2(-1, -1);   
                default:
                    throw new ArgumentException("Invalid direction int");
            }
        }







        #endregion


        private void GetMap(Point point, int range)
        {
           

            simpleMap = new bool[2*range+1, 2*range+1];
            bfsMap = new int[2 * range+1, 2* range+1];
            this.sight = range;
            enemy_Info.enemy_bfs_val = 0;

            for (int i = 0; i < range * 2 + 1; i++)
            {
                for (int j = 0; j < range * 2 + 1; j++)
                {
                    bfsMap[i, j] = int.MaxValue / 2;
                
                }
            
            }

            for (int i = -range; i < range + 1;i++)
            { 
                for(int j=-range;j<range + 1;j++)
                {
  
                    Point tmp = new Point(point.X + j , point.Y + i);

                    if (tmp.X < 0 || tmp.Y < 0 || tmp.X >= Coordinate.Instance.width || tmp.Y >= Coordinate.Instance.height)
                    {
                        continue;
                    }

               


                    if (Coordinate.Instance.Info[tmp.Y, tmp.X].X == (int)GameEnums.Type.None && Coordinate.Instance.TileMapping[tmp.Y, tmp.X])
                    {
                        simpleMap[i + range, j + range] = true;




                    }

                    //if (Coordinate.Instance.Info[tmp.Y, tmp.X].X == (int)GameEnums.Type.None )
                    //{
                    //    simpleMap[i + range, j + range] = true;

                    //}
                    else
                    {
                        simpleMap[i+ range, j + range] = false;

                    
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


        public int[,] Enemy_ReachAble(Point point, int range, int limit,SpriteEntity entity)
        {
            GetMap(point, range);

            Queue<Point> queue = new();
            bfsMap[range, range] = 0;

            queue.Enqueue(new Point(range, range));


            while (queue.Count != 0)
            {
                Point center = queue.Dequeue();

                int val = bfsMap[center.Y, center.X];

                if (val >= limit) continue;

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

            isReady = entity;

            return bfsMap;

        }




        public int[,] ReachAble(Point point, int range,SpriteEntity entity)
        {
       

            GetMap(point, range);

            Queue<Point> queue = new();
            bfsMap[range, range] = 0;

            queue.Enqueue(new Point(range,range));


            while (queue.Count != 0)
            {
              


                Point center = queue.Dequeue();

                int val = bfsMap[center.Y, center.X];

                if (val >= range) continue;

                Point[] pt = new Point[4];


                pt[0] = new Point(center.X - 1, center.Y);
                pt[1] = new Point(center.X + 1, center.Y);
                pt[2] = new Point(center.X , center.Y +1);
                pt[3] = new Point(center.X , center.Y -1);

                for (int i = 0; i < 4; i++)
                {

                    if (pt[i].X >= 0 && pt[i].X <= range * 2 && pt[i].Y >= 0 && pt[i].Y <= range * 2
                        && simpleMap[pt[i].Y, pt[i].X] && bfsMap[pt[i].Y, pt[i].X] == int.MaxValue/2)
                    {
                        bfsMap[pt[i].Y, pt[i].X] = val + 1;
                        queue.Enqueue(pt[i]);
                    }

                }

            }
           
            isReady = entity;

            return bfsMap;
        }


        public static BFS Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BFS();
                return _instance;
            }
        }



    }
}
