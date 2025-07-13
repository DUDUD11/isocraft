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
    public class StatusUI : UIEntity
    {
        SpriteAnimated Gun;
        public int speed;
        public readonly int damage;
        public int health { get; set; }
        public readonly int maxhealth;
        public int bullet_num { get; set; }
        public readonly int bullet_reload;
        public int gun_effectiverange;
        public string gun_name;
        

        public static Vector2 StatusUI_Dims = new Vector2(TileMap.Tile_Size * 6, TileMap.Tile_Size * 6);
  
        public static string UI_bkg = "UI\\Window03";
        public static Vector2 fixed_pos = StatusUI_Dims / 2;
        private SpriteFont string_texture;
        private SpriteFont string_texture_big;
        private Texture2D gun_texture;
        private Vector2 space = new Vector2(10, 10);

        public Vector2 gunTexture_size;
        private Vector2 weaponname_size;

        private Button reload_btn;




        public void AddWeaponAnimation(Vector2 frames, string path, int totalframes, int millitimePerFrame, string NAME, int idx, bool repeat,bool backtodefault)
        {
            Gun.AddAnimation(frames,path,totalframes,millitimePerFrame,NAME,idx,repeat,backtodefault);
        }


        public StatusUI(bool active, string weapontexture,int damage, int maxhealth, int curhealth,int bullet_num,int bullet_reload,int gun_effectiverange,int speed, string gun_name)
        : base(UI_bkg, fixed_pos, StatusUI_Dims, active, 0, 0, new Vector2(1, 1), 1, 0, null)
        {
            this.gun_texture = Game1._Instance.Content.Load<Texture2D>(weapontexture);
            string_texture = Game1._Instance.Content.Load<SpriteFont>(UIEntity.Font16);
            string_texture_big = Game1._Instance.Content.Load<SpriteFont>(UIEntity.Font32);

            this.damage = damage;
            this.maxhealth = maxhealth;
            this.health = curhealth;
            this.bullet_num = bullet_num;
            this.bullet_reload = bullet_reload;
            this.gun_effectiverange = gun_effectiverange;
            this.gun_name = gun_name;
            this.speed = speed;
            weaponname_size = string_texture_big.MeasureString(gun_name);

            gunTexture_size = new Vector2(StatusUI_Dims.X/4, StatusUI_Dims.Y / 4) - space;

            Vector2 gun_texture_pos = new Vector2(pos.X - weaponname_size.X , pos.Y - weaponname_size.Y + dims.Y / 4);

            Gun = new SpriteAnimated(weapontexture, gun_texture_pos, gunTexture_size, 0, 0, 1, new Vector2(1, 1), 3, 1, 100, "defulat");

            // reload = new Button("UI\\Btn_V10", new Vector2(pos.X - hp_size.X / 2, pos.Y - 5f * hp_size.Y),)

            reload_btn = new Button("UI\\Btn_V10", new Vector2(pos.X+dims.X/2, pos.Y +dims.Y/2) - 2 * space, TileMap.Tile_Dims, true, 0, 0, new Vector2(1, 1), 1, 100,"reload");
            reload_btn.offset_Fix(true);
            reload_btn.set_Action(Reload);
            //     string_size = string_texture.MeasureString(name);

            //portrait_dims = new Vector2(ActivityBar_Dims.X, ActivityBar.ActivityBar_Dims.Y / 2) - space;
            //portrait_pos = new Vector2(pos.X, pos.Y + portrait_dims.Y / 2);

            //name_dims = new Vector2(ActivityBar_Dims.X, ActivityBar.ActivityBar_Dims.Y / 4) - space;
            //name_pos = new Vector2(pos.X, pos.Y - name_dims.Y / 2);
            //bar_dims = new Vector2(ActivityBar_Dims.X / 4, ActivityBar.ActivityBar_Dims.Y / 4) - space;
            //first_bar_pos = new Vector2(pos.X - ActivityBar_Dims.X / 4, pos.Y - ActivityBar_Dims.Y / 3);
            //second_bar_pos = new Vector2(pos.X + ActivityBar_Dims.X / 4, pos.Y - ActivityBar_Dims.Y / 3);

        }

        public void Fire()
        {
            Gun.SetAnimationByName("fire");
            bullet_num--;
        }

        public void Reload()
        {

            if (!this.active) return;


            if (Game1.selected.sprite is Heros _h)
            {
                if (_h.cur_act < 1) return;
                _h.cur_act -= 1;
                _h.activityBar.activity -= 1;

                Gun.SetAnimationByName("reload");
                bullet_num = bullet_reload;
                

                SoundController.SoundChange("9mm Reload");
            }
        }


        public override void Draw(Sprites sprite)
        {
            if (!active) return;


            sprite.Draw(model, new Rectangle((int)(pos.X), (int)(pos.Y), (int)dims.X, (int)dims.Y), Color.White, 0f,
 new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));



            string hp = "HP : " + health + " / " + maxhealth;
            Vector2 weaponname_size = string_texture_big.MeasureString(gun_name);
            Vector2 hp_size = string_texture.MeasureString(hp);

            sprite.DrawString(string_texture_big, gun_name, new Vector2(pos.X - weaponname_size.X / 2, pos.Y - weaponname_size.Y + dims.Y / 6), Color.Black);


            sprite.DrawString(string_texture, hp, new Vector2(pos.X - hp_size.X / 2, pos.Y - hp_size.Y), Color.White);

            sprite.DrawString(string_texture, "Speed : " + speed, new Vector2(pos.X - hp_size.X / 2, pos.Y - 2f * hp_size.Y), Color.White);

            sprite.DrawString(string_texture, "damage : " + damage, new Vector2(pos.X - hp_size.X / 2, pos.Y - 3f * hp_size.Y), Color.White);

            sprite.DrawString(string_texture, "bullet : " + bullet_num + " / " + bullet_reload, new Vector2(pos.X - hp_size.X / 2, pos.Y - 4f * hp_size.Y), Color.White);

            sprite.DrawString(string_texture, "range : " + gun_effectiverange, new Vector2(pos.X - hp_size.X / 2, pos.Y - 5f * hp_size.Y), Color.White);

            Gun.Draw(sprite, true);

            reload_btn.Draw(sprite);

        }

        public override void Update()
        {
            if (!active) return;

            Gun.Update();

            reload_btn.Update();


            //if (Game1.selected.sprite == null || !Game1.Player_turn)
            //{
            //    return;
            //}

            //if (Game1.selected.sprite is Heros hero)
            //{ 


            //}

            ////if (Hover(Game1.MouseScreenPos))
            ////{
            ////    if (FlatMouse.Instance.IsLeftMouseButtonPressed())
            ////    {
            ////        if (Clicked != null)
            ////        {

            ////            Clicked.Invoke();

            ////        }
            ////    }
            ////}

        }




    }
}



   

 





