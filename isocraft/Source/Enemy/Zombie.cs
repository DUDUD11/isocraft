using Flat;
using Flat.Graphics;
using Flat.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static isocraft.Heros;
using static isocraft.male;



namespace isocraft
{
    // patrol 안하는 좀비
    public class Zombie : Villain
    {
        public static Vector2 zombie_Frames = new Vector2(8, 1);
        public static Vector2 zombie_Dims = TileMap.Tile_Dims / 2f;
        public static int zombie_animation_num = 4;
        public static float zombie_Dead_EffectTime = 0.6f;
        public static float zombie_Attack_EffectTime = 1f;


        public static int dirNum = 8;
        public static int millisecondFrame = 200;

        public static int health = 3;
        public static int act = 2;
        public static int zombie_atkrange = 1;
        public static int zombie_range = 3;
        public static int zombie_dmg = 1;
        public static int zombie_sight = 5;

        public static float Speed = 1.5f;
        public Heros target;
        public Heros moving_target;

        public zombie_Status status = Zombie.zombie_Status.Idle;

        private double Dead_timer = 0f;
        private double Atk_timer = 0f;
        public enum zombie_Status
        {
            Idle,
            Selected,
            Moving,
            Attack,
            Attacked,
            Dead,
        }

        public Zombie(string path, Vector2 init_pos, int dir, string name = null,List<Point> patrol = null) :
    base(path, init_pos, zombie_Dims, dir, dirNum, zombie_range, zombie_Frames, zombie_animation_num,
    (int)(zombie_Frames.X * zombie_Frames.Y), millisecondFrame, name ?? "Idle",patrol)
        {
        
            cur_health = health;
            dmg = zombie_dmg;
            range = zombie_range;

            AddAnimation(new Vector2(8, 5), "Character\\Animation\\Zombie\\zombie_run", 40, millisecondFrame, "Run", 1);
            AddAnimation(new Vector2(8, 3), "Character\\Animation\\Zombie\\zombie_attack", 24, millisecondFrame*2, "Attack", 2);
            AddAnimation(new Vector2(8, 4), "Character\\Animation\\Zombie\\zombie_dead", 32, millisecondFrame, "Dead", 3,false);
        }

        public override void Get_Hit(int damage)
        {
            if (Destroy) return;

            updateRequired = true;

            cur_health -= damage;

            if (damage < 0 || health <= 0f)
            {

                health = 0;

                status = zombie_Status.Dead;
                Dead_timer = WorldTimer.Instance.totalTime();
            }

            else
            {
                status = zombie_Status.Attacked;
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
                case zombie_Status.Dead:

                    if (Dead_timer + zombie_Dead_EffectTime > WorldTimer.Instance.totalTime())
                    {
                        Destroy_Sprite();
                        //Changeanimation dead
                    }

                    break;

                case zombie_Status.Idle:

                    if (currentAnimation != 0)
                    {
                        ChangeCurrentAnimation(0);
                        pathway.past.X = -1;
                        pathway.next.X = -1;
                        target = null;
                        moving_target = null;
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

                case zombie_Status.Selected:

                    //UI 띄워야됨
                    // 대사도 있으면 좋고
                    //

                    break;

                case zombie_Status.Moving:
                    //dir 변경
                    // speed 와 경로에 따라 업데이트
                    // 목적지에 도달했는지 확인하고 selected 로 변경
  
                    if (moving_target == null)
                    {
                        status = zombie_Status.Idle;
                    }
                    Move();

                    break;
                case zombie_Status.Attack:

                    if (target == null)
                    {
                        status = zombie_Status.Idle;
                    }

                    if (cur_act == 0)
                    {
                        if (Atk_timer + zombie_Attack_EffectTime <= WorldTimer.Instance.totalTime())
                        {
                            status = zombie_Status.Idle;
                        }

                        break;
                    }

                    else
                    {
                        Attack();
                    }
                    //dir 변경
                    // shooting timer
                    // selected 상태로 회귀

                    break;
                case zombie_Status.Attacked:
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


            if (Path.Count == 0)
            {
                if (PathReach)
                {
                    pos = pathway.next.ToVector2();
                    status = zombie_Status.Idle;
                    return;
                }
            
            }


            else if (pathway.past.X == -1 || PathReach)
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
            status = zombie_Status.Selected;
        }

        // attack 하는동안은 애니메이션이 나오게 딴거못하게하고
        public override void Attack()
        {
            Atk_timer = WorldTimer.Instance.totalTime();

            cur_act = 0;

            int dir = BFS.Instance.Direction(pos,target.pos);


            ChangeCurrentAnimation(2);
            ChangeDirCurrentAnimaition(dir);

         
            target.Get_Hit(zombie_dmg);

        }

        // 원거리면 
        //일단 bfs 돌려서 있는지확인하고 raycast해서 후보지 정하고 갈수있는지를 확인하라 .. 

        public override void AI()
        {

            if (BFS.Enemy_Can_MeleeAttack(pos.ToPoint(), out Heros attack_hero))

            {
                int dir = BFS.Instance.Direction(pos - attack_hero.pos );

                Console.WriteLine(dir);

                target = attack_hero;
                status = zombie_Status.Attack;
                ChangeCurrentAnimation(2);
                ChangeDirCurrentAnimaition(dir);
                updateRequired = true;
                //atack추가
                return;
            }

            else
            {


                if (BFS.Instance.Enemy_Can_Detect_360deg_melee(pos.ToPoint(), (int)(zombie_sight * 1.5),this, out Heros detect_hero))
                {
                    


                    Path = BFS.Instance.EnemyMeleeMove(detect_hero.pos.ToPoint() - pos.ToPoint(), zombie_range, cur_act, out int cost, out Point fn_point);

                    cost = (cost + zombie_range - 1) / zombie_range;

                    // 올림용으로 zombie_range-1 
                    cur_act = Math.Max(0, cur_act-cost);
                    status = zombie_Status.Moving;
                    moving_target = detect_hero;
                    pathway.start = this.pos.ToPoint();
                    ChangeCurrentAnimation(1);


    

                    Coordinate.Instance.Move_Unit(pos.ToPoint(), fn_point, new Point((int)GameEnums.Type.Enemy, (int)GameEnums.Enemy.zombie));

                }
                // 여기서 얼만큼 움직일건지 행동력 남길건지 결정해야될듯


                else
                {
                    // 그냥 쉬는걸로
                    cur_act = 0;
                
                }


            }
            //if (BFS.Instance.Moveable(point - pos.ToPoint()))
            //{

            //    ChangeCurrentAnimation(1);
            //    status = zombie_Status.Moving;
            //    Path = BFS.Instance.Move(point - pos.ToPoint());

            //    pathway.start = this.pos.ToPoint();

            //    updateRequired = true;

            //    Coordinate.Instance.Move_Unit(pos.ToPoint(), point, new Point((int)GameEnums.Type.Hero, (int)GameEnums.Hero.zombie));

            //}




        }

        public override void Reset_act()
        {
            if (status == zombie_Status.Dead) return;
            this.cur_act = act;
            this.status = zombie_Status.Idle;
        }


    }
}
