using Flat.Graphics;
using Flat.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isocraft
{
    public class Button : UIEntity
    {
        public bool isPressed, isHovered;
        public bool toggle;

        public static string Button_path = "UI\\SimpleBtn";
        public SpriteFont textfont = Game1._Instance.Content.Load<SpriteFont>(Font12);   
        public Color hoverColor = Color.AliceBlue;

        public Action<Button> ButtonClicked;
        public Action _ButtonClicked;

        public string button_text;
        public bool reference;
        public Color ButtonTextColor { get; set; } = Color.Black;

        public Button(string path,Vector2 pos,Vector2 dims,bool active,int dir,int dirNum, Vector2 Frame,int animation_num,int millisecond,string txt=null,bool toggle=false)
            : base(path, pos, dims, active, dir, dirNum, Frame, animation_num, millisecond)
        {
            toggle = false;
            reference = false;


            if (txt != null)
            {
                set_BtnText(txt, Color.Black, UIEntity.Font12);
            }
        }
        public void set_BtnText(string txt,Color color,string text_font)
        {
            button_text = txt;
            this.ButtonTextColor = color;
            if (textfont != null)
            {
                textfont = Game1._Instance.Content.Load<SpriteFont>(text_font);
            }
        }

        public void set_Action(Action<Button> action)
        {
            ButtonClicked += action;
        }

        public void set_Action(Action action)
        {
            _ButtonClicked += action;
        }




        public override void Update()
        {
            base.Update();
            if (!active) { return; }

            Vector2 CursorPos = Game1.MouseScreenPos;
            bool LeftClick = FlatMouse.Instance.IsLeftMouseButtonPressed();

            if (Hover(CursorPos))
            {
                isHovered = true;

                if (LeftClick)
                {
                    isHovered = false;
                    isPressed = true;
                }

                else if (FlatMouse.Instance.IsLeftMouseButtonReleased())
                {
                    RunBtnClick();
                }
            }

            else
            {
                isHovered = false;
            }

            if (!LeftClick && !FlatMouse.Instance.IsLeftMouseButtonDown())
            {
                isPressed = false;
            }
        }

        public void Reset()
        {
            isPressed = false;
            isHovered = false;
        }

        public void RunBtnClick()
        {
            if (ButtonClicked == null)
            {
                _ButtonClicked?.Invoke();
            }

            else
            {
                ButtonClicked?.Invoke(this);
            }


            Reset();

            if (toggle)
            {
                active = !active;
            }

        }

        public override void Draw(Sprites sprite)
        {
            if (!active) { return; }

            Color tmpColor = Color.White;
            if (isPressed)
            {
                tmpColor = Color.Gray;
            }

            else if (isHovered)
            {
                tmpColor = hoverColor;
            }


            sprite.Draw(model, new Rectangle((int)(pos.X), (int)(pos.Y), (int)dims.X, (int)dims.Y), tmpColor, 0f,
            new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));

            if (button_text != null)
            {
                Vector2 ButtontextDims = textfont.MeasureString(button_text);
                sprite.DrawString(textfont, button_text, new Vector2(pos.X - ButtontextDims.X / 2, pos.Y - ButtontextDims.Y / 2), ButtonTextColor);
            }

        }

      
    }
}
