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

        public static int Activitybar_num =0;


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

        private static Vector2 ActivityBar_Calc(int idx)
        {
            int hero_num = 2;

            //for (int i = 0; i < EntityManager.Heroes.Count; i++)
            //{
            //    hero_num++;
            //}

            Vector2 init_pos = new Vector2(Game1._Instance.getZoom()*Game1.screen_width/2-ActivityBar.ActivityBar_Dims.X*hero_num, Game1._Instance.getZoom() * Game1.screen_height - ActivityBar.ActivityBar_Dims.Y / 2);

            init_pos += new Vector2(ActivityBar.ActivityBar_Dims.X * idx,0);

            return init_pos;
        
        }

        public static Vector2 Activity_Add()
        {
            

            return ActivityBar_Calc(Activitybar_num++);


        }

        public virtual void Trun_End()
        { 
        
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
             //   Console.WriteLine(pos + " " + mousePosition);
                return true;
            }
            return false;

        }

        public bool HoverButton(Vector2 mousePosition,bool cameraused)
        {

            if (!active) { return false; }

            if (!cameraused) return Hover(mousePosition);

            mousePosition = Coordinate.ToOffset(mousePosition);

            float min_x = pos.X - dims.X / 2;
            float max_x = pos.X + dims.X / 2;
            float min_y = pos.Y - dims.Y / 2;
            float max_y = pos.Y + dims.Y / 2;



            if (mousePosition.X >= min_x && mousePosition.X <= max_x && mousePosition.Y >= min_y && mousePosition.Y <= max_y)
            {
                Console.WriteLine(pos + " " + mousePosition);
                return true;
            }
            return false;

        }


    }
}
