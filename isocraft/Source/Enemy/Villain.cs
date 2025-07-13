using Flat.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using static isocraft.Heros;
using static isocraft.Villain;

namespace isocraft
{
    public class Villain : SpriteAnimated
    {
        //빌런이 다른점이 뭘까?
        // 클릭이 아닌 스스로 생각하는 AI에 의해서 행동한다

        public enum Strategy
        { 
            Hold, // 행동력과 관계없이 대기
            Move, // 행동력 1일떄 움직임 (쏠수있는 곳으로)
            Move_and_Shoot, // 행동력 2일때 움직이고 쏨
            Shoot, // 행동력과 관계없이 쏠수있으면 쏨 
        }

        public struct Villain_st
        {
            public string path;
            public int dir;
            public List<Point> Patrol;
        
        }

        public static int Shooting_accuracy = 30;
        public int cur_health;
        public int cur_act;
        public int range;
        public int dmg;

        public st_pathway pathway;


    
        public Stack<Point> Path = new();
        public List<Point> Patrol = new();


        protected bool PathReach = false;

        public Villain(string path, Vector2 init_pos, Vector2 dims, int dir, int dirNum, int range, 
            Vector2 frames, int animation_num, int totalAnimationNum, int millisecondFrame, string name = null, List<Point> Patrol = null) :
        base(path, init_pos, dims, dir,0, dirNum, frames, animation_num, totalAnimationNum, millisecondFrame, name ?? "Idle")
        {
            cur_act = 0;
            this.range = range;
            pathway.past = new Point(-1, -1);
            pathway.next = new Point(-1, -1);
            pathway.start = new Point(-1, -1);
            this.Patrol = Patrol;
        }

       public static Villain ParseToInstance(object obj, int x, int y, int type)
        {

            Villain_st villain_St;

            if (obj is JsonElement jsonElement)
            {
                // JsonElement → Villain_st
                villain_St.path = jsonElement.GetProperty("Item1").GetProperty("path").GetString();
                villain_St.dir = jsonElement.GetProperty("Item1").GetProperty("dir").GetInt32();
                villain_St.Patrol = null;
              //  public List<Point> Patrol;


      
            }

            else
            {
                villain_St= (Villain_st)obj;
            }

         
       
            switch(type)
            {
                case (int)GameEnums.Enemy.zombie:

                    Villain zombie = new Zombie(villain_St.path, new Vector2(x, y), villain_St.dir,patrol:villain_St.Patrol);
                    return zombie;
                   
                case (int)GameEnums.Enemy.solidier:
                    Villain solidier = new Solidier(villain_St.path, new Vector2(x, y), villain_St.dir, patrol: villain_St.Patrol);
                    return solidier;

                   
                default:
                    throw new Exception("ERR");


            }

       
        }

        protected virtual void Dying()
        { }

        public static Object ParseToObj(Villain villain)
        {
            Villain_st tmpvillain;

            tmpvillain.path = villain.url;
            tmpvillain.dir = villain.GetDirCurrentAnimaition();
            tmpvillain.Patrol = villain.Patrol;


            Object obj = (Object)tmpvillain;


            return obj;
        }

        public static Villain Clone(Villain villain, int type)
        {

            switch (type)
            {
                case (int)GameEnums.Enemy.zombie:
                    Villain zombie = new Zombie(villain.url, villain.pos, villain.GetDirCurrentAnimaition(), patrol: villain.Patrol);
                    return zombie;

                case (int)GameEnums.Enemy.solidier:
                    Villain solidier = new Solidier(villain.url, villain.pos, villain.GetDirCurrentAnimaition(), patrol: villain.Patrol);
                    return solidier;
                default:
                    throw new Exception("ERR");
            }

        }

        public bool Die()
        {
            return this.cur_health <= 0;
        }


        public virtual void Get_Hit(int damage)
        {

        }

        public virtual void Attack()
        { 
        
        }

        public override void Update()
        {
            base.Update();
        }
        public override void Draw(Sprites sprite)
        {
            base.Draw(sprite);
        }

        public virtual void LeftClick(Point point)
        {
            // 정보 UI
        }

        public virtual void AI()
        { 
            // 
        }

        public virtual void Reset_act()
        {
            throw new Exception("No arg");

        }


    }
}
