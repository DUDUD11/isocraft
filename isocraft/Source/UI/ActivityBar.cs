using Flat.Graphics;
using Flat.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
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

        public ActivityBar(string portrait, Vector2 init_pos, bool active,int activity, string name)
            : base("UI\\PanelWindow", init_pos, ActivityBar_Dims, active, 0, 0, new Vector2(1, 1), 1, 0, null)
        {
            this.portrait = Game1._Instance.Content.Load<Texture2D>(portrait);
            emptyBar = Game1._Instance.Content.Load<Texture2D>(emptyAct);
            fillBar = Game1._Instance.Content.Load<Texture2D>(fillAct);
            namebkg = Game1._Instance.Content.Load<Texture2D>(name_bkg);
            string_texture = Game1._Instance.Content.Load<SpriteFont>(UIEntity.Font24);
            this.active = active;
            this.pos = init_pos;
            this.activity = activity;
            this.name = name;

     
    
            

            portrait_dims = new Vector2(ActivityBar_Dims.X, ActivityBar.ActivityBar_Dims.Y / 2) - space;
            portrait_pos = new Vector2(pos.X, pos.Y + portrait_dims.Y / 2);

            name_dims = new Vector2(ActivityBar_Dims.X, ActivityBar.ActivityBar_Dims.Y / 4) - space;
            name_pos = new Vector2(pos.X, pos.Y - name_dims.Y / 2);
            bar_dims = new Vector2(ActivityBar_Dims.X / 4, ActivityBar.ActivityBar_Dims.Y / 4) - space;
            first_bar_pos = new Vector2(pos.X - ActivityBar_Dims.X / 4, pos.Y - ActivityBar_Dims.Y / 3);
            second_bar_pos = new Vector2(pos.X + ActivityBar_Dims.X / 4, pos.Y - ActivityBar_Dims.Y / 3);


            string_size = string_texture.MeasureString(name);

        }
   
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


