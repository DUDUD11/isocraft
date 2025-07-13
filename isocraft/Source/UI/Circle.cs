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

namespace isocraft
{
    public class Circle : UIEntity
    {
        public static int button_num = 4;
        private int button_active = 0;
        public Button[] buttons;
        public Action reject;


        private SpriteFont font;

        private Texture2D explaintexture;

        private Vector2 explain_size;

        private string explain_text;
        public Vector2 exp_textsz;


        public Circle(Vector2 init_pos, Vector2 dims, bool active)
            : base("UI\\Circle", init_pos, dims, active, 0, 0, new Vector2(1, 1), 1, 0, null)
        {
            buttons = new Button[button_num];
            this.active = active;

            explaintexture = Game1._Instance.Content.Load<Texture2D>("UI\\explain");

            explain_size = dims * 0.2f;

            font = Game1._Instance.Content.Load<SpriteFont>(Font16);

        }

        public void setText(string str)
        {
            this.explain_text = str;
            exp_textsz = font.MeasureString(str);
        }

        public void AddButton(Button button)
        {
       
            buttons[button_active] = button;
            buttons[button_active++].offset_Fix(true);
        }

        public override void Update()
        {
            if (!active) return;

            Vector2 mousePos = Coordinate.ToOffset(Game1.MouseScreenPos);
            bool hoverflag = false;

            for (int i = 0; i < button_active; i++)
            {
                buttons[i].Update();
                if (buttons[i].isHovered || buttons[i].isPressed) hoverflag = true;
            }

            if (FlatMouse.Instance.IsLeftMouseButtonPressed() && !hoverflag)
            {
                reject?.Invoke();
            }


        }

        public override void Draw(Sprites sprite)
        {
            if (!active) return;

            sprite.Draw(model, new Rectangle((int)(pos.X), (int)(pos.Y), (int)dims.X, (int)dims.Y), Color.White, 0f,
     new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));


            sprite.Draw(explaintexture, new Rectangle((int)(pos.X), (int)(pos.Y), (int)explain_size.X, (int)explain_size.Y), Color.White, 0f,
new Vector2(explaintexture.Bounds.Width / 2, explaintexture.Bounds.Height / 2));

            if (explain_text != null && !explain_text.Equals(""))
            {
                sprite.DrawString(font, explain_text, new Vector2(pos.X - exp_textsz.X / 2, pos.Y - exp_textsz.Y / 2), Color.White);
            }

            for (int i = 0; i < button_active; i++)
            {
                buttons[i].Draw(sprite);

            }

      

        }





    }
}
