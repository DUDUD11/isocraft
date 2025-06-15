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




namespace isocraft
{
    public class male : Heros
    {

        public static Vector2 male_Frames = new Vector2(8, 1);
        public static Vector2 male_Dims = TileMap.Tile_Dims / 2f;
        public static int male_animation_num =5;
        public static float male_Dead_EffectTime = 0.5f;
        public static int dirNum = 8;
        public static int millisecondFrame = 200;
 

        public static int health = 3;
        public static int act = 2;
        public static int Willpower = 100;
        public static int male_range = 5;

        public static float Speed =1.5f;

        public male_Status status = male.male_Status.Idle;

        private double Dead_timer = 0f;
     

        public enum male_Status
        {
            Idle,
            Selected,
            Moving,
            Shooting,
            Attacked,
            Dead,
        }

        public male(string path, Vector2 init_pos, int dir, string name = null) :
    base( path, init_pos, male_Dims, dir, dirNum, male_range, male_Frames, male_animation_num,
    (int)(male_Frames.X * male_Frames.Y), millisecondFrame, name ?? "Idle")
        {
            cur_health = health;
            cur_act = act;
            cur_willpower = 0;

            AddAnimation(new Vector2(8, 4), "Character\\Animation\\Male\\male_walk", 32, millisecondFrame,"Walk", 1);
     

        }

        public override void Get_Hit(int damage)
        {
            if (Destroy) return;    
            
            cur_health -= damage;

            if (damage < 0 || health <= 0f )
            {

                health = 0;
              
                status = male_Status.Dead;
                Dead_timer = WorldTimer.Instance.totalTime();
            }

            else
            {
                status = male_Status.Attacked;
            //    ChangeCurrentAnimation(4);
            }
        }

        public override void Update()
        {

            if (Destroy)
            {
                return;
            }

            switch (status)
            {
                case male_Status.Dead:

                    if (Dead_timer + male_Dead_EffectTime > WorldTimer.Instance.totalTime())
                    {
                        Destroy_Sprite();
                        //Changeanimation dead
                    }

                    break;

                case male_Status.Idle:

                    if (currentAnimation != 0)
                    {
                        ChangeCurrentAnimation(0);
                        path_past_pos.X = -1;
                        path_next_pos.X = -1;
                        PathReach = false;
                        updateRequired = false;
                    }

                    break;

                case male_Status.Selected:

                    // BFS에 query 하고
                    // BFS에서 Queue에 집어넣도록
                    // 이거는 selecte에서 하고


                    //UI 띄워야됨
                    // 대사도 있으면 좋고
                    //

                    break;

                case male_Status.Moving:
                    //dir 변경
                    // speed 와 경로에 따라 업데이트
                    // 목적지에 도달했는지 확인하고 selected 로 변경

                    Move();
                   
   
                    break;
                    case male_Status.Shooting:

                            //dir 변경
                            // shooting timer
                            // selected 상태로 회귀

                            break;
                    case male_Status.Attacked:

                            // 맞고나서 살아있는지 확인

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
            if (Path.Count == 0 && PathReach)
            {
                pos = path_next_pos.ToVector2();
                status = male_Status.Idle;
                return;
            }

      
            if (path_past_pos.X == -1 || PathReach)
            {
                path_past_pos = (path_past_pos.X == -1) ? pos.ToPoint() : path_next_pos;

                path_next_pos = Path.Pop();
                path_next_pos = new Point(
                    path_next_pos.X - range + (int)path_move_start_pos.X,
                    path_next_pos.Y - range + (int)path_move_start_pos.Y
                );

                Vector2 dirVector = new Vector2(path_next_pos.X - pos.X, path_next_pos.Y - pos.Y);
                int dir = BFS.Instance.Direction(dirVector);
                ChangeDirCurrentAnimaition(dir);

                PathReach = false;
                return;
            }

    
            if (FlatMath.Distance(pos, path_next_pos.ToVector2()) < 0.1f)
            {
                PathReach = true;
                return;
            }

            Vector2 delta = new Vector2(
                path_next_pos.X - path_past_pos.X,
                path_next_pos.Y - path_past_pos.Y
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

            base.Draw(sprite,dir);
        }

        public override void Selected()
        {
            BFS.Instance.ReachAble(pos.ToPoint(),range);
            status = male_Status.Selected;
        } 
       

        public override void RightClick(Point point)
        {
            //행동력 수정필요

            if (BFS.Instance.Moveable(point-pos.ToPoint()))
            {

                ChangeCurrentAnimation(1);
                status = male_Status.Moving;
                Path = BFS.Instance.Move(point - pos.ToPoint());

                path_move_start_pos = this.pos.ToPoint();

                updateRequired = true;

                Coordinate.Instance.Move_Unit(pos.ToPoint(), point, new Point((int)GameEnums.Type.Hero, (int)GameEnums.Hero.male));

            }




        }

        public override void Reset_act()
        {
            this.cur_act = act;
        }


    }
}
