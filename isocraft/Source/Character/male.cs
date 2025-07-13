using Flat;
using Flat.Graphics;
using Flat.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static isocraft.GameEnums;




namespace isocraft
{
    public class male : Heros
    {

        public static Vector2 male_Frames = new Vector2(8, 1);
        public static Vector2 male_Dims = TileMap.Tile_Dims / 2f;
        public static int male_animation_num =5;
        public static float male_Dead_EffectTime = 0.5f;
        public static float male_Shooting_EffectTime = 0.5f;
        public static int dirNum = 8;
        public static int millisecondFrame = 200;

        public static int bullets = 1;
        public static int male_health =10;
        public static int act = 2;
        public static int Willpower = 100;
        public static int male_range = 5;
        public static int male_atkrange = 5;
        public static int male_dealing = 2;
        public static int default_accuracy = 80;

        public static string weapon_url = "Character\\Weapons\\Glock - P80 [64x48]";


        public static float Speed =1.5f;

        



      

        public male(string path, Vector2 init_pos, int dir, string name = null) :
    base( path, init_pos, male_Dims, dir, dirNum, male_range, male_Frames, male_animation_num,
    (int)(male_Frames.X * male_Frames.Y), millisecondFrame, name ?? "Idle")
        {
            cur_health = male_health;
            cur_act = act;
            cur_willpower = 0;

            AddAnimation(new Vector2(8, 4), "Character\\Animation\\Male\\male_walk", 32, millisecondFrame,"Walk", 1);
            AddAnimation(new Vector2(8, 1), "Character\\Animation\\Male\\male_fire", 8, millisecondFrame, "Fire", 2,false);
            AddAnimation(new Vector2(8, 1), "Character\\Animation\\Male\\male_hit", 8, millisecondFrame, "Hit", 3,false);
            AddAnimation(new Vector2(8, 1), "Character\\Animation\\Male\\male_dead", 8, millisecondFrame, "Dead", 4, false);

            circle = EntityManager.male_circle;
            circle.buttons[0]._ButtonClicked += ShootingCallback;
            circle.buttons[1]._ButtonClicked += DoubleAttackCallback;
            circle.reject+= CancelCallback;

            activityBar = new ActivityBar("Character\\Portrait\\male1", UIEntity.Activity_Add(), true, act, "male");
            activityBar.Clicked += Selected;

            EntityManager.AddUI(activityBar);


            statusUI = new StatusUI(false, weapon_url, male_dealing, male_health, this.cur_health, bullets, bullets, male_atkrange,male_range, "P80");

            statusUI.AddWeaponAnimation(new Vector2(1,12), "Character\\Weapons\\P80-Shoot",12,100,"fire",1,false,true);
            statusUI.AddWeaponAnimation(new Vector2(1, 6), "Character\\Weapons\\P80-Reload", 6, 100, "reload", 2, false, true);

            Vector2 pp = Coordinate.ToIsometric(pos.X, pos.Y) * Game1._Instance.getZoom();

            conditionBar = new ConditionBar(pp, true, male_health, cur_health);

            EntityManager.AddUI(conditionBar);

        }

        public override void Get_Hit(int damage)
        {
            if (Destroy) return;
            updateRequired = true;
            cur_health -= damage;

            statusUI.health = cur_health;

            conditionBar.set_hp(cur_health);

            if (damage < 0 || cur_health <= 0f )
            {

                Dying();
            }

            else
            {
                status = Hero_Status.Attacked;
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
                        

                case Hero_Status.Dead:


             
                  
                    if (Dead_timer + male_Dead_EffectTime < WorldTimer.Instance.totalTime())
                    {
                        Destroy_Sprite();
                        Game1.Attacked_Sprite = null;
                        activityBar.active = false;
                    }

                    break;

                case Hero_Status.Idle:

            


                    if (currentAnimation != 0 || Cancel)
                    {

                        Console.WriteLine("aaa");


                        ChangeCurrentAnimation(0);
                        pathway.past.X = -1;
                        pathway.next.X = -1;
                        pathway.start.X = -1;
                        PathReach = false;
                        updateRequired = false;

                        Cancel = false;

                    

                 //       Vector2 pp = Coordinate.ToIsometric(pos.X, pos.Y);

                        
                    }

                    break;

                case Hero_Status.Selected:

                    // BFS에 query 하고
                    // BFS에서 Queue에 집어넣도록
                    // 이거는 selecte에서 하고


                    //UI 띄워야됨
                    // 대사도 있으면 좋고
                    //

                    break;

                case Hero_Status.Moving:
                    //dir 변경
                    // speed 와 경로에 따라 업데이트
                    // 목적지에 도달했는지 확인하고 selected 로 변경

                    Move();
                   
   
                    break;
                    case Hero_Status.Shooting:
            
                    //dir 변경
                    // shooting timer
                    // selected 상태로 회귀
                        Attack();
                  

                    break;

                case Hero_Status.DoubleShooting:
           
                    //dir 변경
                    // shooting timer
                    // selected 상태로 회귀


                    DoubleAttack();
              

                    break;


                case Hero_Status.Attacked:

                            // 맞고나서 살아있는지 확인

                        break;

                }
            activityBar.UniqueUpdate(cur_act);
         
                base.Update();
            }

        public override void Turn_End()
        {
            cur_act = act;
        }

        protected override void Dying()
        {
            Game1.Attacked_Sprite = this;
            cur_health = 0;

            status = Hero_Status.Dead;
            Dead_timer = WorldTimer.Instance.totalTime();
            ChangeCurrentAnimation(4);
        }



        protected override void Attack()
        {
           

            switch (shooting_Sequence)
            {
                      

                case Shooting_Sequence.Not_Move :

                    pathway.start = pos.ToPoint();

                    Vector2 dirVector = new Vector2(_ShootingPos.X - pos.X, _ShootingPos.Y - pos.Y);
                    int dir = BFS.Instance.Direction(dirVector);
                    ChangeCurrentAnimation(1);
                    ChangeDirCurrentAnimaition(dir);
                    shooting_Sequence = Shooting_Sequence.Move;
                      
   
                    break;
                case Shooting_Sequence.Move:


                    if (FlatMath.Distance(pos, _ShootingPos) < 0.1f)
                    {
                        shooting_Sequence = Shooting_Sequence.Not_Shoot;
                        return;
                    }

                    else
                    {
          

                        float moveAmount = (float)Game1.GameTime * Speed;


                        Vector2 delta = new Vector2(
                            _ShootingPos.X - pos.X,
                            _ShootingPos.Y - pos.Y
                        );


                        if (delta.X != 0)
                            pos.X += MathF.Sign(delta.X) * moveAmount;

                        if (delta.Y != 0)
                            pos.Y += MathF.Sign(delta.Y) * moveAmount;
                    }
                        break;
                case Shooting_Sequence.Not_Shoot:

                    BFS.Instance.Attack_Enemy(_hit_percent, male_dealing, _villain);

                    Vector2 atkdirVector = new Vector2(_villain.pos.X - _ShootingPos.X, _villain.pos.Y - _ShootingPos.Y);
                    int atkdir = BFS.Instance.Direction(atkdirVector);
                    ChangeCurrentAnimation(2);
                    ChangeDirCurrentAnimaition(atkdir);
                    statusUI.Fire();

                    Shooting_timer = WorldTimer.Instance.totalTime();
                    shooting_Sequence = Shooting_Sequence.Shooting;

                    
              


                    break;
                case Shooting_Sequence.Shooting:

             

                    if (male_Shooting_EffectTime+Shooting_timer < WorldTimer.Instance.totalTime())
                    {
             

                        Vector2 ret_vec = new Vector2(pos.X - pathway.start.X , pos.Y - pathway.start.Y );

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
                        status = Hero_Status.Idle;
                     

                  
                    }

                    else
                    {

                        float moveAmount = (float)Game1.GameTime * Speed;


                        Vector2 delta = new Vector2(
                            pathway.start.X - _ShootingPos.X,
                            pathway.start.Y - _ShootingPos.Y
                       
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

        protected override void DoubleAttack()
        {


            switch (shooting_Sequence)
            {


                case Shooting_Sequence.Not_Move:

                    pathway.start = pos.ToPoint();

                    Vector2 dirVector = new Vector2(_ShootingPos.X - pos.X, _ShootingPos.Y - pos.Y);
                    int dir = BFS.Instance.Direction(dirVector);
                    ChangeCurrentAnimation(1);
                    ChangeDirCurrentAnimaition(dir);
                    shooting_Sequence = Shooting_Sequence.Move;


                    break;
                case Shooting_Sequence.Move:


                    if (FlatMath.Distance(pos, _ShootingPos) < 0.1f)
                    {
                        shooting_Sequence = Shooting_Sequence.Not_Shoot;
                        return;
                    }

                    else
                    {


                        float moveAmount = (float)Game1.GameTime * Speed;


                        Vector2 delta = new Vector2(
                            _ShootingPos.X - pos.X,
                            _ShootingPos.Y - pos.Y
                        );


                        if (delta.X != 0)
                            pos.X += MathF.Sign(delta.X) * moveAmount;

                        if (delta.Y != 0)
                            pos.Y += MathF.Sign(delta.Y) * moveAmount;
                    }
                    break;
                case Shooting_Sequence.Not_Shoot:

                    BFS.Instance.Attack_Enemy(_hit_percent, male_dealing, _villain);

                    Vector2 atkdirVector = new Vector2(_villain.pos.X - _ShootingPos.X, _villain.pos.Y - _ShootingPos.Y);
                    int atkdir = BFS.Instance.Direction(atkdirVector);
                    ChangeCurrentAnimation(2);
                    ChangeDirCurrentAnimaition(atkdir);

                    Shooting_timer = WorldTimer.Instance.totalTime();
                    shooting_Sequence = Shooting_Sequence.Shooting;
                 
                    break;
                case Shooting_Sequence.Shooting:



                    if (male_Shooting_EffectTime + Shooting_timer < WorldTimer.Instance.totalTime())
                    {


                        Vector2 ret_vec = new Vector2(pos.X - pathway.start.X, pos.Y - pathway.start.Y);

                        int return_dir = BFS.Instance.Direction(ret_vec);
                        ChangeCurrentAnimation(1);
                        ChangeDirCurrentAnimaition(return_dir);
                        shooting_Sequence = Shooting_Sequence.Not_return;
                        statusUI.Fire();

                    }

                    break;
                case Shooting_Sequence.Not_return:




                    if (FlatMath.Distance(pos, pathway.start.ToVector2()) < 0.1f)
                    {
                        shooting_Sequence = Shooting_Sequence.Not_Move;

                        if (!_villain.Die())
                        {
                            Attack();
                            status = Hero_Status.Shooting;

                            return;
                        }

                        else
                        {
                            status = Hero_Status.Idle;
                        }

                    }

                    else
                    {

                        float moveAmount = (float)Game1.GameTime * Speed;


                        Vector2 delta = new Vector2(
                            pathway.start.X - _ShootingPos.X,
                            pathway.start.Y - _ShootingPos.Y

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


        protected override void Move()
        {
            conditionBar.set_pos(Coordinate.ToIsometric(pos.X, pos.Y) * Game1._Instance.getZoom());

            if (Path.Count == 0 && PathReach)
            {
                pos = pathway.next.ToVector2();

                status = Hero_Status.Idle;
                ChangeCurrentAnimation(0);
                Selected();

               

                return;
            }

            if (pathway.past.X == -1 || PathReach)
            {
                pathway.past = (pathway.past.X == -1) ? pos.ToPoint() : pathway.next;

                pathway.next = Path.Pop();
                pathway.next = new Point(
                    pathway.next.X - pathway.offset.X + (int)pathway.start.X,
                    pathway.next.Y - pathway.offset.Y + (int)pathway.start.Y
                );

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

       
            if (status != Hero_Status.Idle && status!=Hero_Status.Selected) return;

   
            conditionBar.set_sel();
        
            BFS.Instance.ReachAble(pos.ToPoint(),cur_act*range,this);
            pathway.offset = new Point(cur_act * range, cur_act * range);

            status = Hero_Status.Selected;


            updateRequired = false;
            Game1._Instance.Selected(this);
        } 
       

        public override void RightClick(Point point)
        {
            //행동력 수정필요

            if (cur_act < 1) return;


            if (BFS.Instance.MousePoint_Moveable(point - pos.ToPoint()))
            {

                ChangeCurrentAnimation(1);
                status = Hero_Status.Moving;
                Path = BFS.Instance.Move(point - pos.ToPoint(), out int cost);

                cur_act -= (cost + range - 1) / (range);

                pathway.start = this.pos.ToPoint();

                updateRequired = true;

                Coordinate.Instance.Move_Unit(pos.ToPoint(), point, new Point((int)GameEnums.Type.Hero, (int)GameEnums.Hero.male));

                SoundController.SoundChange("confirmation", RandomHelper.RandomInteger(1, 10));

                return;

            }
            ShootingFlag = BFS.Instance.MousePoint_Attackable(this.pos.ToPoint(), point, male_atkrange, default_accuracy,
               out _villain, out _hit_percent, out _ShootingPos);

           


            if (_villain == null )
            {
                OrderReject();
                circle.setText("Enemy far away");

            }




            updateRequired = true;
            circle.active = true;

            //else if ()
            //{


             

        
            //}
        
        }

        public override void OrderReject()
        {
            SoundController.SoundChange("refusal", RandomHelper.RandomInteger(1, 10));
            status = Hero_Status.Idle;
            Cancel = true;
        }

        public void CancelCallback()
        {
       
            status = Hero_Status.Idle;
            Cancel = true;
            circle.active = false;
        }




        public void ShootingCallback()
        {
     


            if (Game1.selected.sprite != this) return;

            circle.active = false;

            if (statusUI.bullet_num < 1 || !ShootingFlag)
            {
                circle.setText("Low Ammo");
                OrderReject();
                return;
            }

        

            cur_act--;
            status = male.Hero_Status.Shooting;

            updateRequired = true;
            shooting_Sequence = Shooting_Sequence.Not_Move;
        }

        public void DoubleAttackCallback()
        {



            if (Game1.selected.sprite != this ) return;


            circle.active = false;

            if (statusUI.bullet_num < 2 || !ShootingFlag)
            {
                circle.setText("Low Ammo");
                OrderReject();
                return;

            }


            cur_act--;
            status = male.Hero_Status.DoubleShooting;

            updateRequired = true;
            shooting_Sequence = Shooting_Sequence.Not_Move;


        }


        public override void Reset_act()
        {
   
            if (status == Hero_Status.Dead) return;
            this.cur_act = act;
            this.status = Hero_Status.Idle;
        }


    }
}
