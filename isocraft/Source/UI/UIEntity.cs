using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flat.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace isocraft
{
    public class UIEntity : SpriteAnimated
    {
        public static string Font12 = "Fonts\\Roboto_SemiCondensed-Light";
        public static string Font16 = "Fonts\\Arial16";
        public static string Font24 = "Fonts\\Roboto_Condensed-Bold";
        public static string Font32 = "Fonts\\Rubik-Black";
     


        public Vector2 frames;
        public readonly Vector2 FrameSize;
        public bool active = true;

        public UIEntity(string path, Vector2 init_pos, Vector2 dims,bool active, int dir,int dirNum, 
            Vector2 Frame, int animation_num, int millisecondFrame, string name = null) :
    base(path, init_pos, dims, dir,0, dirNum, Frame, animation_num,
    (int)(Frame.X * Frame.Y), millisecondFrame, name ?? "Idle")
        {
            this.active = active;
        
        }

        public override void Update()
        {
            if (!active) return;

            base.Update();
        }

     

        public bool Hover(Vector2 mousePosition)
        {
            if (!active) { return false; }

            

            float min_x = pos.X - dims.X / 2;
            float max_x = pos.X + dims.X / 2;
            float min_y = pos.Y - dims.Y / 2;
            float max_y = pos.Y + dims.Y / 2;

            if (mousePosition.X >= min_x && mousePosition.X <= max_x && mousePosition.Y >= min_y && mousePosition.Y <= max_y)
            {
                return true;
            }
            return false;



        }


    }
}
