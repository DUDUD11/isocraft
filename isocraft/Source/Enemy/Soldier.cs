using Flat;
using Flat.Graphics;
using Flat.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static isocraft.Heros;
using static isocraft.male;
using static isocraft.Zombie;




namespace isocraft
{
    public class Solidier : Villain
    {
        public enum Solidier_Status
        {
            Idle,
            Selected,
            Moving,
            Attack,
            Attacked,
            Dead,
        }

        public struct Shooting_metaData
        {
            public Heros target;
            public Vector2 ShootingPos;
            public int hitrate;
            public Point walking_end_point;
            public int walking_cost;
        }



        public static Vector2 Solidier_Frames = new Vector2(8, 1);
        public static Vector2 Solidier_Dims = TileMap.Tile_Dims / 2f;
        public static int Solidier_animation_num = 5;
        public static float Solidier_Dead_EffectTime = 0.6f;
        public static float Solidier_Attack_EffectTime = 1f;
        public static float Solidier_Shooting_EffectTime = 0.5f;


        public static int dirNum = 8;
        public static int millisecondFrame = 200;

        public static int health = 3;
        public static int act = 2;
        public static int Solidier_atkrange = 5;
        public static int Solidier_range = 5;
        public static int Solidier_dealing = 1;
        public static int Solidier_sight = 5;
        public static int default_accuracy = 50;

        public static float Speed = 1.5f;

        public Solidier_Status status = Solidier_Status.Idle;
        public Strategy strategy = Strategy.Hold;


        public Shooting_metaData metadata;
        public Shooting_Sequence shooting_Sequence;
      

        private double Dead_timer = 0f;
        private double Atk_timer = 0f;


        public Solidier(string path, Vector2 init_pos, int dir, string name = null,List<Point> patrol=null) :
        base(path, init_pos, Solidier_Dims, dir, dirNum, Solidier_range, Solidier_Frames, Solidier_animation_num,
        (int)(Solidier_Frames.X * Solidier_Frames.Y), millisecondFrame, name ?? "Idle",patrol)
            {

                cur_health = health;
                dmg = Solidier_dealing;
                range = Solidier_range;

            AddAnimation(new Vector2(8, 4), "Character\\Animation\\MafiaEnemy\\mafiaE_walk", 32, millisecondFrame, "walk", 1);
            AddAnimation(new Vector2(8, 2), "Character\\Animation\\MafiaEnemy\\mafiaE_shoot", 16, millisecondFrame , "Attack", 2, false);
            AddAnimation(new Vector2(8, 2), "Character\\Animation\\MafiaEnemy\\mafiaE_dead", 16, millisecondFrame, "Dead", 3, false);
        }

        protected override void Dying()
        {
    
            Game1.Attacked_Sprite = this;
            cur_health = 0;

            status = Solidier_Status.Dead;
            Dead_timer = WorldTimer.Instance.totalTime();
            ChangeCurrentAnimation(3);
        
        }

        public override void Get_Hit(int damage)
            {

            updateRequired = true;

            if (Destroy) return;

                cur_health -= damage;

                if (cur_health < 0 || health <= 0f)
                {

                    Dying();
                
                }

                else
                {
                    status = Solidier_Status.Attacked;
                    //    ChangeCurrentAnimation(4);
                }
            }

            public override void Update()
            {
       
           

            if (Destroy || !updateRequired)
                {
                    return;
                }

                switch (status)
                {
                    case Solidier_Status.Dead:

                        if (Dead_timer + Solidier_Dead_EffectTime < WorldTimer.Instance.totalTime())
                        {
                            Destroy_Sprite();
                            Game1.Attacked_Sprite = null;
                        //Changeanimation dead
                        }

                        break;

                    case Solidier_Status.Idle:

                        if (currentAnimation != 0)
                        {
                            ChangeCurrentAnimation(0);
                            pathway.past.X = -1;
                            pathway.next.X = -1;
                            pathway.start.X = -1;
                            metadata = default;
                            PathReach = false;
                        }

                        if (cur_act != 0)
                        {
                            AI();
                        }

                        else
                        {
                            updateRequired = false;
                       
                        }

                        break;

                    case Solidier_Status.Selected:

                        //UI 띄워야됨
                        // 대사도 있으면 좋고
                        //

                        break;

                    case Solidier_Status.Moving:
                        //dir 변경
                        // speed 와 경로에 따라 업데이트
                        // 목적지에 도달했는지 확인하고 selected 로 변경

                        if (metadata.target == null)
                        {
                            status = Solidier_Status.Idle;
                        }
                        Move();

                        break;
                    case Solidier_Status.Attack:

                        if (metadata.target == null)
                        {
                            status = Solidier_Status.Idle;
                        }

                        Attack();
                        
                        //dir 변경
                        // shooting timer
                        // selected 상태로 회귀

                        break;
                    case Solidier_Status.Attacked:
                        // 상대턴이라 쓰일지는 모르겠음
                        // Draw에 그려야될듯


                        break;

                }
                base.Update();
            }

            public override void Turn_End()
            {
                cur_act = act;
            }


            private void Move()
            {
                // 옮기고 range 만큼 됐는데 가능하면 attack 로 옮겨야됨

                //range만큼만 움직이게끔해야함


                if (Path.Count == 0 && PathReach)
                {
                    pos = pathway.next.ToVector2();

                    if (strategy == Strategy.Move)
                    {
                        status = Solidier_Status.Idle;
                    }

                    else if (strategy == Strategy.Move_and_Shoot)
                    {
                        status = Solidier_Status.Attack;
                    }
                    return;
                }
       

            else  if (pathway.past.X == -1 || PathReach)
            {
                pathway.past = (pathway.past.X == -1) ? pos.ToPoint() : pathway.next;

                pathway.next = Path.Pop();


                Vector2 dirVector = new Vector2(pathway.next.X - pos.X, pathway.next.Y - pos.Y);
                int dir = BFS.Instance.Direction(dirVector);
                ChangeDirCurrentAnimaition(dir);

                PathReach = false;
                return;
            }


                if (FlatMath.Distance(pos, pathway.next.ToVector2()) < 0.1f)
                {
                    PathReach = true;
                    return;
                }

                Vector2 delta = new Vector2(
                    pathway.next.X - pathway.past.X,
                    pathway.next.Y - pathway.past.Y
                );

                float moveAmount = (float)Game1.GameTime * Speed;

                if (delta.X != 0)
                    pos.X += MathF.Sign(delta.X) * moveAmount;

                if (delta.Y != 0)
                    pos.Y += MathF.Sign(delta.Y) * moveAmount;
            }

            public override void Draw(Sprites sprite)
            {
                //      Game1.AntiAliasingShader(model, dims);
                //    sprite.Draw(model, new Rectangle((int)(pos.X+o.X), (int)(pos.Y+o.Y), (int)dims.X, (int)dims.Y), Color.White,flatBody.Angle,
                //       new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));

                base.Draw(sprite);
            }

            public override void Selected()
            {
                //  BFS.Instance.ReachAble(pos.ToPoint(), range);
                status = Solidier_Status.Selected;
            }

        // attack 하는동안은 애니메이션이 나오게 딴거못하게하고



        public override void Attack()
        {

        
            switch (shooting_Sequence)
            {
                

                case Shooting_Sequence.Not_Move:

                    pathway.start = pos.ToPoint();

                    Vector2 dirVector = new Vector2(metadata.ShootingPos.X - pos.X, metadata.ShootingPos.Y - pos.Y);

                    int dir;

                    if (FlatMath.LengthSquared(dirVector.X, dirVector.Y) < 0.1f)
                    {
                        dir = BFS.Instance.Direction(metadata.ShootingPos);
                    }
                    else
                    {
                        dir = BFS.Instance.Direction(dirVector);
                    }

                       
                    ChangeCurrentAnimation(1);
                    ChangeDirCurrentAnimaition(dir);
                    shooting_Sequence = Shooting_Sequence.Move;


                    break;
                case Shooting_Sequence.Move:


                    if (FlatMath.Distance(pos, metadata.ShootingPos) < 0.1f)
                    {
                        shooting_Sequence = Shooting_Sequence.Not_Shoot;
                        return;
                    }

                    else
                    {


                        float moveAmount = (float)Game1.GameTime * Speed;


                        Vector2 delta = new Vector2(
                            metadata.ShootingPos.X - pos.X,
                            metadata.ShootingPos.Y - pos.Y
                        );


                        if (delta.X != 0)
                            pos.X += MathF.Sign(delta.X) * moveAmount;

                        if (delta.Y != 0)
                            pos.Y += MathF.Sign(delta.Y) * moveAmount;
                    }
                    break;
                case Shooting_Sequence.Not_Shoot:

                    BFS.Instance.Attack_Hero(metadata.hitrate, male_dealing, metadata.target);

                    Vector2 atkdirVector = new Vector2(metadata.target.pos.X - pos.X, metadata.target.pos.Y - pos.Y);
                    int atkdir = BFS.Instance.Direction(atkdirVector);
                    ChangeCurrentAnimation(2);
                    ChangeDirCurrentAnimaition(atkdir);

                    Atk_timer = WorldTimer.Instance.totalTime();
                    shooting_Sequence = Shooting_Sequence.Shooting;
                    break;
                case Shooting_Sequence.Shooting:



                    if (male_Shooting_EffectTime + Atk_timer < WorldTimer.Instance.totalTime())
                    {


                        Vector2 ret_vec = new Vector2(pos.X - pathway.start.X, pos.Y - pathway.start.Y);

                        int return_dir = BFS.Instance.Direction(ret_vec);
                        ChangeCurrentAnimation(1);
                        ChangeDirCurrentAnimaition(return_dir);
                        shooting_Sequence = Shooting_Sequence.Not_return;

                    }

                    break;
                case Shooting_Sequence.Not_return:


                    if (FlatMath.Distance(pos, pathway.start.ToVector2()) < 0.1f)
                    {
                        shooting_Sequence = Shooting_Sequence.Not_Move;
                        status = Solidier_Status.Idle;
                    }

                    else
                    {

                        float moveAmount = (float)Game1.GameTime * Speed;


                        Vector2 delta = new Vector2(
                            pathway.start.X - metadata.ShootingPos.X,
                            pathway.start.Y - metadata.ShootingPos.Y

                        );


                        if (delta.X != 0)
                            pos.X += MathF.Sign(delta.X) * moveAmount;

                        if (delta.Y != 0)
                            pos.Y += MathF.Sign(delta.Y) * moveAmount;
                    }

                    break;
                default:
                    throw new Exception("Logical Error");
            }


        }

            // 원거리면 
            //일단 bfs 돌려서 있는지확인하고 raycast해서 후보지 정하고 갈수있는지를 확인하라 .. 

            public override void AI()
            {

           


            if (BFS.Instance.Enemy_Can_Detect_90deg_ImPenetration_Range(pos.ToPoint(), BFS.RevDirection(GetDirCurrentAnimaition()), this, Solidier_sight, out Heros _))
            {

           


                Path = BFS.Instance.EnemyRangeMove(pos.ToPoint(), Solidier_range, cur_act, Solidier_atkrange, default_accuracy, Villain.Shooting_accuracy,
                    out metadata.walking_cost, out metadata.hitrate, out metadata.walking_end_point, out metadata.target,
                    out metadata.ShootingPos, out strategy);


                if (strategy == Strategy.Hold)
                {
                    status = Solidier_Status.Idle;
                    cur_act = 0;


                }

                else if (strategy == Strategy.Move || strategy == Strategy.Move_and_Shoot)
                {
                    // move

                    metadata.walking_cost = (metadata.walking_cost + Solidier_range - 1) / Solidier_range;

                    cur_act -= metadata.walking_cost;
                    status = Solidier_Status.Moving;
                    pathway.start = this.pos.ToPoint();
                    ChangeCurrentAnimation(1);
                    Coordinate.Instance.Move_Unit(pos.ToPoint(), metadata.walking_end_point, new Point((int)GameEnums.Type.Enemy, (int)GameEnums.Enemy.solidier));

                }
                else if (strategy == Strategy.Shoot)
                {
                    //shoot

                    cur_act--;
                    status = Solidier_Status.Attack;


                    shooting_Sequence = Shooting_Sequence.Not_Move;

                    //ChangeCurrentAnimation(2);
                    //ChangeDirCurrentAnimaition(dir);

                }


            }

            else
            {
                cur_act = 0;

               
            }
            }

            public override void Reset_act()
            {
            if (status == Solidier_Status.Dead) return;
            this.cur_act = act;
            this.status = Solidier_Status.Idle;
        }


        

    }
}





