using Flat.Graphics;
using Flat.Input;
using isocraft;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static isocraft.Game1;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace isocraft
{
    public class GameReadyUI : UIEntity
    {
        public static int hero_num = 2;
        public static int upgradeable_object_num = 4;
        public static int upgradeable_steps = 3;
 
        public static bool[,] Upgrade_Mat = new bool[4,3];
        public static string UpgradeUrl = "Upgrade";

        public Hero_Type hero_Type = Hero_Type.male;

        public Texture2D UI_cell;
        private UpgradeData upgradeData;

        public static int Upgrade_Num = 5;

        //stats
        //hp, damage, range, speed

        //Chapter X
        //~~~~

        // Start
        // Back


        //save하고, 능력치 불러오고, 하는등의 작업필요

        public enum Hero_Type
        { 
            male,
            mafia,
        }

        public enum Upgrade
        { 
            gundamage,
            gunrange,
            hp,
            moveSpeed,
        
        }


        public Vector2 IconSize;
        public Button[,] Upgrade_Button;
        public Button[] Unit_typebuttons; //클릭하면 herotype을 알수있게

        private string[] upgrade_texture = { "UI\\1upgrade", "UI\\2upgrade", "UI\\3upgrade" };
        private string[] upgradedone_texture = { "UI\\1upgrade-done", "UI\\2upgrade-done", "UI\\3upgrade-done" };

        private string[] textures = { "UI\\gundamage", "UI\\gunrange" ,"UI\\HP" , "UI\\moveSpeed" };
        private string[] hover_text = { "Gun Damage", "Effective Range", "Health", "moveSpeed" };


        private string Unit_Btn_string;

        private SpriteFont font;
        private SpriteFont font_big;


        private Vector2 space_y = new Vector2(0, 10);
        private Vector2 space_both = new Vector2(10, 10);


        Vector2 up_left_pos;
        Vector2 up_left_dims;
        Vector2 up_right_pos;
        Vector2 up_right_dims;
        Vector2 down_right_pos;
        Vector2 down_right_dims;

        Texture2D Window04;
        Texture2D Window03;
        Texture2D Window02;

        private string up_left_title = "Stats";
        private string up_right_title = "OBjective";

        Vector2 up_left_vec;
        Vector2 up_right_vec;

        private float dimsY_font;



        public GameReadyUI(bool active)
     : base("UI\\Panel1_NoOpacity592x975px", new Vector2(Game1.screen_width*Game1._Instance.getZoom() / 3,Game1.screen_height * Game1._Instance.getZoom() / 3) , new Vector2(Game1.screen_width*Game1._Instance.getZoom()/1.5f,Game1.screen_height * Game1._Instance.getZoom()/1.5f), active, 0, 0, new Vector2(1, 1), 1, 0, null)
        {
            Window02 = Game1._Instance.Content.Load<Texture2D>("UI\\Window02");
            Window03 = Game1._Instance.Content.Load<Texture2D>("UI\\Window03");
            Window04 = Game1._Instance.Content.Load<Texture2D>("UI\\Window04");

            Vector2 screen = new Vector2(Game1.screen_width * Game1._Instance.getZoom(), Game1.screen_height * Game1._Instance.getZoom());


            up_left_dims = new Vector2(dims.X, screen.Y - dims.Y) - space_y;
            up_left_pos = new Vector2(dims.X/2, screen.Y - up_left_dims.Y/2 - space_y.Y/2);

        


            up_right_dims = new Vector2(screen.X - dims.X, screen.Y / 1.2f) - space_both;
            up_right_pos = new Vector2(screen.X - up_right_dims.X/2, screen.Y - up_right_dims.Y / 2 - space_both.Y / 2);

            down_right_dims = new Vector2(up_right_dims.X, screen.Y - up_right_dims.Y) - space_y;
            down_right_pos = new Vector2(up_right_pos.X, down_right_dims.Y/2);


            UI_cell = Game1._Instance.Content.Load<Texture2D>("UI\\CellBig_02");
            Unit_Btn_string = "UI\\Btn_V15";
            

            IconSize = new Vector2(Game1.screen_width / 10, Game1.screen_height / 10);

            Upgrade_Button = new Button[4,4];

            font = Game1._Instance.Content.Load<SpriteFont>(Font16);
            font_big = Game1._Instance.Content.Load<SpriteFont>(Font32);

            up_left_vec = font_big.MeasureString(up_left_title);
            up_right_vec = font_big.MeasureString(up_right_title);

            for (int i = 0; i < 4; i++)
            {
               
                for (int j = 0; j < 4; j++)
                {
                    if (j == 0)
                    {
                        Upgrade_Button[i, j] = new Button(textures[i], new Vector2(Game1.screen_width / 6 * (i+1), Game1.screen_height / 5 * (j+2)), IconSize, true, 0, 0, new Vector2(1, 1), 1, 0, null);
                        Upgrade_Button[i, j].set_Hoverext(hover_text[i], Color.White, Font24);
                    }

                    else
                    {
                        Upgrade_Button[i, j] = new Button(upgrade_texture[j-1], new Vector2(Game1.screen_width / 6 * (i+1), Game1.screen_height / 5 * (j+2)), IconSize, true, 0, 0, new Vector2(1, 1), 1, 0, null);
                        Upgrade_Button[i, j].set_Action(Upgrade_Callback);
                        Upgrade_Button[i, j].set_RightAction(Abort_Callback);
                    }

                    Upgrade_Button[i, j].offset_Fix(true);
                }
            }

            Unit_typebuttons = new Button[Enum.GetValues(typeof(Hero_Type)).Length];

            for (int i = 0; i < Enum.GetValues(typeof(Hero_Type)).Length; i++)
            {
                Unit_typebuttons[i] = new Button(Unit_Btn_string,new Vector2(Game1.screen_width / 6 * (i+1), Game1.screen_height / 5),IconSize, true, 0, 0, new Vector2(1, 1), 1, 0, null);

                Unit_typebuttons[i].offset_Fix(true);
            }

            this.active = active;

            upgradeData = Save.Instance.LoadUpgradeData(UpgradeUrl);

            if (upgradeData != null)
            {
                male_Clicked();
                Upgrade_Num = upgradeData.remain_perk;
            }

            else
            {
                upgradeData = new UpgradeData();
                upgradeData.Init(hero_num, upgradeable_object_num, upgradeable_steps);
            }



        }

        public void male_Clicked()
        {
            hero_Type = Hero_Type.male;

            Update_Mat(0);
            Update_Button();

        //파일 upgrade.json 만들고 읽어와서 male 부분을 upgrade_mat에 대응시키고
        //이미지도 대응
        }

        public void Update_Mat(int idx)
        {
            for (int i = 0; i < upgradeData.upgrade_objects; i++)
            {
                for (int j = 0; j < upgradeData.upgrade_steps; j++)
                {
                    Upgrade_Mat[i, j] = upgradeData.UpgradeList[idx][i][j];
                }
            
            }

        }

        public void Update_Button()
        {
            for (int i = 0; i < upgradeable_object_num; i++)
            {
                for (int j = 0; j < upgradeable_steps; j++)
                {
                    if (Upgrade_Mat[i, j])
                    {
                        Upgrade_Button[i, j + 1].UpdateModel(upgradedone_texture[j]);
                    }
                }
            }
        
        }


        public void Upgrade_Callback(Button button)
        {
            if (Upgrade_Num == 0) return;

            int idx_i = -1;
            int idx_j = -1;

            for (int i = 0; i < 4; i++)
            {

                for (int j = 1; j < 4; j++)
                {
                    if (Upgrade_Button[i, j] == button)
                    {
                        idx_i = i;
                        idx_j = j;
                    }
                }
            }

            if (idx_i == -1 || idx_j == -1) return;

            for (int j = idx_j-1; j >= 1; j--)
            {
                if (!Upgrade_Mat[idx_i, j-1]) return;
            }
            Upgrade_Mat[idx_i, idx_j-1] = true;
            Upgrade_Button[idx_i, idx_j].UpdateModel(upgradedone_texture[idx_j-1]);
            Upgrade_Num--;

            switch (hero_Type)
            {
                case Hero_Type.male:
                    upgradeData.Map_Update(0,idx_i,idx_j - 1,true);
                    break;
                case Hero_Type.mafia:
                    upgradeData.Map_Update(0, idx_i, idx_j - 1, true);
                    break;
                default:
                    throw new Exception("hero_type_Error");
            }

            SoundController.SoundChange("Modern8");
        }


        public void Abort_Callback(Button button)
        {
            

            int idx_i = -1;
            int idx_j = -1;

            for (int i = 0; i < 4; i++)
            {

                for (int j = 1; j < 4; j++)
                {
                    if (Upgrade_Button[i, j] == button)
                    {
                        idx_i = i;
                        idx_j = j;
                    }
                }
            }

            if (idx_i == -1 || idx_j == -1) return;

            if (!Upgrade_Mat[idx_i, idx_j - 1]) return;

            for (int j = idx_j + 1; j < 4; j++)
            {
                if (Upgrade_Mat[idx_i, j - 1]) return;
            }


            Upgrade_Mat[idx_i, idx_j - 1] = false;
            Upgrade_Button[idx_i, idx_j].UpdateModel(upgrade_texture[idx_j - 1]);
            Upgrade_Num++;

            switch (hero_Type)
            {
                case Hero_Type.male:
                    upgradeData.Map_Update(0, idx_i, idx_j - 1, false);
                    break;
                case Hero_Type.mafia:
                    upgradeData.Map_Update(0, idx_i, idx_j - 1, false);
                    break;
                default:
                    throw new Exception("hero_type_Error");
            }

            SoundController.SoundChange("Modern15");
        }



        public override void Update()
        {
            if (!active) return;

            Vector2 mousePos = Coordinate.ToOffset(Game1.MouseScreenPos);
        

            for (int i = 0; i < 4; i++)
            {

                for (int j = 0; j < 4; j++)
                {

                    Upgrade_Button[i, j].Update();
                }
            }

            for (int i = 0; i < Enum.GetValues(typeof(Hero_Type)).Length; i++)
            {
                Unit_typebuttons[i].Update();
            }


            if (FlatKeyboard.Instance.IsKeyDown(Keys.P))
            {
                upgradeData.remain_perk = Upgrade_Num;
                Save.Instance.SaveUpgradeData(upgradeData, UpgradeUrl);

            }


            //Unit_typebuttons

        }


        public override void Draw(Sprites sprite)
        {
            if (!active) return;

            Vector2 stringsz = font.MeasureString("" + Upgrade_Num);

            sprite.Draw(model, new Rectangle((int)(pos.X), (int)(pos.Y), (int)dims.X, (int)dims.Y), Color.White, 0f,
     new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));


            sprite.Draw(Window02, new Rectangle((int)(up_left_pos.X), (int)(up_left_pos.Y), (int)up_left_dims.X, (int)up_left_dims.Y), Color.White, 0f,
     new Vector2(Window02.Bounds.Width / 2, Window02.Bounds.Height / 2));

         


            sprite.Draw(Window04, new Rectangle((int)(up_right_pos.X), (int)(up_right_pos.Y), (int)up_right_dims.X, (int)up_right_dims.Y), Color.White, 0f,
     new Vector2(Window04.Bounds.Width / 2, Window04.Bounds.Height / 2));

            sprite.Draw(Window03, new Rectangle((int)(down_right_pos.X), (int)(down_right_pos.Y), (int)down_right_dims.X, (int)down_right_dims.Y), Color.White, 0f,
new Vector2(Window03.Bounds.Width / 2, Window03.Bounds.Height / 2));





            sprite.Draw(UI_cell, new Rectangle((int)(pos.X+dims.X/3), (int)(pos.Y+dims.Y/3), (int)IconSize.X, (int)IconSize.Y), Color.White, 0f,
new Vector2(UI_cell.Bounds.Width / 2, UI_cell.Bounds.Height / 2));


            sprite.DrawString(font, "" + Upgrade_Num, new Vector2(-stringsz.X / 2 + pos.X + dims.X / 3, -stringsz.Y / 2 + pos.Y + dims.Y / 3), Color.Red);


            for (int i = 0; i < 4; i++)
            {

                for (int j = 0; j < 4; j++)
                {
                    Upgrade_Button[i, j].Draw(sprite);
                }
            }


            for (int i = 0; i < Enum.GetValues(typeof(Hero_Type)).Length; i++)
            {
                Unit_typebuttons[i].Draw(sprite);
            }


            Vector2 up_left_title_pos = up_left_pos - up_left_vec / 2 + new Vector2(0, up_left_dims.Y / 3.2f);
            Vector2 up_right_title_pos = up_right_pos - up_right_vec / 2 + new Vector2(0, up_right_dims.Y / 2.2f);


            sprite.DrawString(font_big, up_left_title, up_left_title_pos, Color.White);

            string up_left_content ="Character : " + hero_Type.ToString() + "\n";

            switch (hero_Type)
            {
                case Hero_Type.male:

                    up_left_content += "HP : " + male.male_health + "\n" +
                                    "Damage : " + male.male_dealing + "\n" +
                                    "Effective Range : " + male.male_atkrange + "\n" +
                                    "Speed : " + male.Speed + "\n" +
                                    "Default Accuracty : " + male.default_accuracy;
                                  
                    break;
                case Hero_Type.mafia:


                    break;
            
            
            }


            dimsY_font = font.MeasureString(up_left_content).Y;

            sprite.DrawString(font, up_left_content, new Vector2(up_left_title_pos.X, up_left_title_pos.Y - dimsY_font), Color.White);
                 
            sprite.DrawString(font_big, up_right_title, up_right_title_pos, Color.White);


            string up_right_content = "They came without warning. The city fell in hours.\n" +
                "Once a thriving metropolis, the city is now a feeding ground for the relentless undead. These zombies\n" +
                "are terrifying in their focus. Once they spot a target, they never stop, never stray, never forget.\n" +
                "You lead a last ditch rescue unit, scouring the ruins for anyone still alive. In the shadows of a ruined\n" +
                "safehouse, you find him: a lone mafia enforcer. Tough, calm under fire, and deadly efficient.\n" +
                "Unlike ordinary survivors, this man does not run. He fights. With unmatched precision, he can strike\n" +
                "twice before the enemy even reacts. In a world where every second counts, his double attack could\n" +
                "mean the difference between survival and death.\n" +
                "Now, in this crumbling city, your only ally is a man born of crime, yet perhaps forged for survival.\n" +
                "And remember: once a zombie sees you, it never stops.";

            dimsY_font = font.MeasureString(up_right_content).Y;


            sprite.DrawString(font, up_right_content, new Vector2(up_right_title_pos.X, up_left_title_pos.Y - dimsY_font), Color.White);







        }



    }
}







   

