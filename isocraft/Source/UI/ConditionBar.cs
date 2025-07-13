using Flat.Graphics;
using Flat.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
namespace isocraft
{
    public class ConditionBar : UIEntity
    {
        public static Vector2 ConditionBar_Dims = new Vector2((int)(TileMap.Tile_Size * 1.5), (int)(TileMap.Tile_Size * 0.85));
        public static Vector2 hpbar_dims = ConditionBar_Dims * 0.7f;
        public Vector2 hpbar_relative_offset = new Vector2(0, (int)(TileMap.Tile_Size * 0.4));

        public static string bkg = "UI\\Panel1_Opacity592x975px";
        public static string hpbar = "UI\\HpBar";
        public static string emptyhpbar = "UI\\emptyHpBar";
        public static string chk = "UI\\Checked_01";
        public static string Notchk = "UI\\Checked_02";

        private Texture2D hpbartexture;
        private Texture2D emptyhpbartexture;
        private Texture2D seltexture;
        private Texture2D notseltexture;

        public int cur_hp;
        public int max_hp;

        private Vector2 pos_helper;
        private float size_x;
        private bool sel;

        private float space;

        public ConditionBar(Vector2 init_pos, bool active, int max_hp, int cur_hp)
    : base(bkg, init_pos+ new Vector2(0, (int)(TileMap.Tile_Size * 1.5)/Game1._Instance.getZoom()), ConditionBar_Dims, active, 0, 0, new Vector2(1, 1), 1, 0, null)
        {

            hpbar_relative_offset = new Vector2(0, (int)(TileMap.Tile_Size * 1.5)/ Game1._Instance.getZoom());

            hpbartexture = Game1._Instance.Content.Load<Texture2D>(hpbar);
            emptyhpbartexture = Game1._Instance.Content.Load<Texture2D>(emptyhpbar);
            seltexture = Game1._Instance.Content.Load<Texture2D>(chk);
            notseltexture = Game1._Instance.Content.Load<Texture2D>(Notchk);

            this.max_hp = max_hp;
            this.cur_hp = cur_hp;

  
            this.active = active;
            

            this.size_x = hpbar_dims.X / max_hp;

            this.space = (ConditionBar_Dims.X - hpbar_dims.X) ;


            //portrait_dims = new Vector2(ActivityBar_Dims.X, ActivityBar.ActivityBar_Dims.Y / 2) - space;
            //portrait_pos = new Vector2(pos.X, pos.Y + portrait_dims.Y / 2);

            //name_dims = new Vector2(ActivityBar_Dims.X, ActivityBar.ActivityBar_Dims.Y / 4) - space;
            //name_pos = new Vector2(pos.X, pos.Y - name_dims.Y / 2);
            //bar_dims = new Vector2(ActivityBar_Dims.X / 4, ActivityBar.ActivityBar_Dims.Y / 4) - space;
            //first_bar_pos = new Vector2(pos.X - ActivityBar_Dims.X / 4, pos.Y - ActivityBar_Dims.Y / 3);
            //second_bar_pos = new Vector2(pos.X + ActivityBar_Dims.X / 4, pos.Y - ActivityBar_Dims.Y / 3);


            //string_size = string_texture.MeasureString(name);

        }

        public void set_hp(int val)
        {
            this.cur_hp = Math.Clamp(val, 0, max_hp);
        }


        public void set_pos(Vector2 pos)
        {
            this.pos = pos + hpbar_relative_offset;
        }

        public void set_sel()
        {
            this.sel = true;
        }

        public override void Update()
        {
                
    

        }

        public override void Turn_End()
        {
            this.sel = false;
        }

        public override void Draw(Sprites sprite)
        {
            if (!active) return;

            pos_helper = new Vector2(pos.X - ConditionBar_Dims.X / 2 + size_x/2, pos.Y);


            Vector2 offset = Game1.offset.ToVector2() * Game1._Instance.getZoom();

            sprite.Draw(model, new Rectangle((int)(pos.X- offset.X), (int)(pos.Y- offset.Y), (int)dims.X, (int)dims.Y), Color.White, 0f,
           new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));

            for (int i = 0; i < cur_hp; i++)
            {
                sprite.Draw(hpbartexture, new Rectangle((int)(pos_helper.X - offset.X), (int)(pos_helper.Y - offset.Y), (int)size_x, (int)hpbar_dims.Y), Color.White, 0f,
                     new Vector2(hpbartexture.Bounds.Width / 2, hpbartexture.Bounds.Height / 2));
                pos_helper += new Vector2(size_x, 0);
            }

            for (int i = cur_hp; i < max_hp; i++)
            {
                sprite.Draw(emptyhpbartexture, new Rectangle((int)(pos_helper.X - offset.X), (int)(pos_helper.Y - offset.Y), (int)size_x, (int)hpbar_dims.Y), Color.White, 0f,
                    new Vector2(emptyhpbartexture.Bounds.Width / 2, emptyhpbartexture.Bounds.Height / 2));
                pos_helper += new Vector2(size_x, 0);
            }

            pos_helper=new Vector2(pos.X+dims.X/2 - space/2,pos_helper.Y);

            if (sel)
            {
                sprite.Draw(seltexture, new Rectangle((int)(pos_helper.X - offset.X), (int)(pos_helper.Y - offset.Y), (int)space, (int)hpbar_dims.Y), Color.White, 0f,
                 new Vector2(seltexture.Bounds.Width / 2, seltexture.Bounds.Height / 2));
            }

            else
            {
                sprite.Draw(notseltexture, new Rectangle((int)(pos_helper.X - offset.X ), (int)(pos_helper.Y - offset.Y), (int)space , (int)hpbar_dims.Y), Color.White, 0f,
            new Vector2(notseltexture.Bounds.Width / 2, notseltexture.Bounds.Height / 2));
            }


        }

    }
}


/*

namespace isocraft
{
    public class ActivityBar : UIEntity
    {
        public static Vector2 ActivityBar_Dims = new Vector2(TileMap.Tile_Size*4,TileMap.Tile_Size*4);
   
        public static string emptyAct = "UI\\emptystamina";
        public static string fillAct = "UI\\stamina";
        public static string name_bkg = "UI\\Btn_V14";

        public int activity;
        public string name;
        private Texture2D portrait;
        private Texture2D emptyBar;
        private Texture2D fillBar;
        private Texture2D namebkg;
        private SpriteFont string_texture;

        private Vector2 space = new Vector2(10, 10);

        private Vector2 string_size;

        public Action Clicked;


        private Vector2 portrait_dims;
        private Vector2 portrait_pos;
        private Vector2 name_dims;
        private Vector2 name_pos;
        private Vector2 bar_dims;
        private Vector2 first_bar_pos;
        private Vector2 second_bar_pos;


   
        public  void UniqueUpdate(int activity)
        {
            if (!active) return;

            this.activity = activity;

           

        }

        public override void Update()
        {

            if (Hover(Game1.MouseScreenPos))
            {
                if (FlatMouse.Instance.IsLeftMouseButtonPressed())
                {
                    if (Clicked != null)
                    {

                        Clicked.Invoke();
                      
                    }
                }
            }

        }

        public override void Turn_End()
        {
            if(Game1.Player_turn)
            this.activity = 2;
        }

        public override void Draw(Sprites sprite)
        {
            if (!active) return;



            sprite.Draw(model, new Rectangle((int)(pos.X ), (int)(pos.Y), (int)dims.X, (int)dims.Y), Color.White, 0f,
           new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));

            sprite.Draw(portrait, new Rectangle((int)(portrait_pos.X), (int)(portrait_pos.Y), (int)portrait_dims.X, (int)portrait_dims.Y), Color.White, 0f,
       new Vector2(portrait.Bounds.Width / 2, portrait.Bounds.Height / 2));

            sprite.Draw(namebkg, new Rectangle((int)(name_pos.X), (int)(name_pos.Y), (int)name_dims.X, (int)name_dims.Y), Color.White, 0f,
new Vector2(namebkg.Bounds.Width / 2, namebkg.Bounds.Height / 2));

            sprite.DrawString(string_texture, name, new Vector2(name_pos.X  - string_size.X/2, name_pos.Y  - string_size.Y/2 ), Color.Black);



            if (activity == 2)
            {
                sprite.Draw(fillBar, new Rectangle((int)(first_bar_pos.X), (int)(first_bar_pos.Y), (int)bar_dims.X, (int)bar_dims.Y), Color.White, 0f,
new Vector2(fillBar.Bounds.Width / 2, fillBar.Bounds.Height / 2));
            }

            else
            {
                sprite.Draw(emptyBar, new Rectangle((int)(first_bar_pos.X), (int)(first_bar_pos.Y), (int)bar_dims.X, (int)bar_dims.Y), Color.White, 0f,
new Vector2(emptyBar.Bounds.Width / 2, emptyBar.Bounds.Height / 2));
            }

            if (activity >= 1)
            {
                sprite.Draw(fillBar, new Rectangle((int)(second_bar_pos.X), (int)(second_bar_pos.Y), (int)bar_dims.X, (int)bar_dims.Y), Color.White, 0f,
new Vector2(fillBar.Bounds.Width / 2, fillBar.Bounds.Height / 2));
            }
            else
            {
                sprite.Draw(emptyBar, new Rectangle((int)(second_bar_pos.X), (int)(second_bar_pos.Y), (int)bar_dims.X, (int)bar_dims.Y), Color.White, 0f,
new Vector2(emptyBar.Bounds.Width / 2, emptyBar.Bounds.Height / 2));
            }

        }


    }
}



 */