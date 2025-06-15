using Flat.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isocraft
{
    public class Heros : SpriteAnimated
    {
    
        public int cur_health;
        public int cur_act;
        public int cur_willpower;
        public int range;
        public Point path_past_pos;
        public Point path_next_pos;
        public Point path_move_start_pos;


        public Stack<Point> Path = new();

        protected bool PathReach = false;

        public Heros(string path, Vector2 init_pos,Vector2 dims, int dir,int dirNum, int range, Vector2 frames, int animation_num,int totalAnimationNum,int millisecondFrame, string name = null) :
    base(path, init_pos, dims, dir, dirNum, frames, animation_num,
    totalAnimationNum, millisecondFrame, name ?? "Idle")
        {
        
            this.range = range;
            path_past_pos = new Point(-1, -1);
            path_next_pos = new Point(-1, -1);
            path_move_start_pos = new Point(-1, -1);
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
            base.Draw(sprite, dir);
        }

     

        public virtual void RightClick(Point point)
        { 
            
        
        }

        public virtual void Reset_act()
        {
            throw new Exception("No arg");
        
        }


    }
}
