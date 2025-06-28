using Flat.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace isocraft
{
    public class Heros : SpriteAnimated
    {
        public struct st_pathway
        {
            public Point past;
            public Point next;
            public Point start;
            public Point offset;
        }

        public enum Hero_Status
        {
            Idle,
            Selected,
            Moving,
            Shooting,
            Attacked,
            Dead,
        }

        public enum Shooting_Sequence
        { 
            Not_Move,
            Move,
            Not_Shoot,
            Shooting,
            Not_return
        }


        public int cur_health;
        public int cur_act;
        public int cur_willpower;
        public int range;
        public st_pathway pathway;
        public Hero_Status status = male.Hero_Status.Idle;

        public Stack<Point> Path = new();
        protected Vector2 _ShootingPos;
        protected Villain _villain;
        protected int _hit_percent;
        protected Shooting_Sequence shooting_Sequence;
        protected bool PathReach = false;

        protected double Dead_timer = 0f;
        protected double Shooting_timer = 0f;

        public Heros(string path, Vector2 init_pos,Vector2 dims, int dir,int dirNum, int range, Vector2 frames, int animation_num,int totalAnimationNum,int millisecondFrame, string name = null) :
    base(path, init_pos, dims, dir,0, dirNum, frames, animation_num,
    totalAnimationNum, millisecondFrame, name ?? "Idle")
        {
        
            this.range = range;
            pathway.past = new Point(-1, -1);
            pathway.next = new Point(-1, -1);
            pathway.start = new Point(-1, -1);
        }

        public virtual void Get_Hit(int damage)
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

     

        public virtual void RightClick(Point point)
        { 
            
        
        }

        protected virtual void Attack()
        { 
        }
        protected virtual void Move()
        { 
        
        }


        public virtual void Reset_act()
        {
            throw new Exception("No arg");
        
        }


    }
}
