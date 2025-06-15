using Flat.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isocraft
{
    public class Villain : SpriteAnimated
    {
        //빌런이 다른점이 뭘까?
        // 클릭이 아닌 스스로 생각하는 AI에 의해서 행동한다
        
        public int cur_health;
        public int cur_act;
        public int range;
        public int dmg;

        public Point path_past_pos;
        public Point path_next_pos;
        public Point path_move_start_pos;
        public Stack<Point> Path = new();
       


        protected bool PathReach = false;

        public Villain(string path, Vector2 init_pos, Vector2 dims, int dir, int dirNum, int range, 
            Vector2 frames, int animation_num, int totalAnimationNum, int millisecondFrame, string name = null) :
        base(path, init_pos, dims, dir, dirNum, frames, animation_num, totalAnimationNum, millisecondFrame, name ?? "Idle")
        {
            cur_act = 0;
            this.range = range;
            path_past_pos = new Point(-1, -1);
            path_next_pos = new Point(-1, -1);
            path_move_start_pos = new Point(-1, -1);
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
            base.Draw(sprite, dir);
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
