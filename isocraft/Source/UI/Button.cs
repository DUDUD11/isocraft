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

        public Action<Button> ButtonRightClicked;
        public Action _ButtonRightClicked;


        public string button_text;
        public string hover_text;
        public bool reference;
        public Color ButtonTextColor { get; set; } = Color.Black;
        public Color HoverTextColor { get; set; } = Color.Black;

        public Texture2D SecondDraw;

        public bool offsetfixed = false;
        public bool cameraused = false;
        public Vector2 original_pos;

        public Button(string path,Vector2 pos,Vector2 dims,bool active,int dir,int dirNum, Vector2 Frame,int animation_num,int millisecond,string txt=null,bool toggle=false)
            : base(path, pos, dims, active, dir, dirNum, Frame, animation_num, millisecond)
        {
            toggle = false;
            reference = false;


            if (txt != null)
            {
                set_BtnText(txt, Color.Black, UIEntity.Font12);
            }

            original_pos = pos;
        }

        public Button(string path,string path2, Vector2 pos, Vector2 dims, bool active, int dir, int dirNum, Vector2 Frame, int animation_num, int millisecond, string txt = null, bool toggle = false)
     : base(path, pos, dims, active, dir, dirNum, Frame, animation_num, millisecond)
        {
            toggle = false;
            reference = false;
            SecondDraw = Game1._Instance.Content.Load<Texture2D>(path2);

            if (txt != null)
            {
                set_BtnText(txt, Color.Black, UIEntity.Font12);
            }

            original_pos = pos;
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

        public void set_Hoverext(string txt, Color color, string text_font)
        {
            hover_text = txt;
            this.HoverTextColor = color;
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

        public void set_RightAction(Action<Button> action)
        {
            ButtonRightClicked += action;
        }

        public void set_RightAction(Action action)
        {
            _ButtonRightClicked += action;
        }


        public void offset_Fix(bool flag)
        {
            offsetfixed = flag;
        }


        public override void Update()
        {
            base.Update();
            if (!active) { return; }

            Vector2 CursorPos = offsetfixed ? Game1.MouseScreenPos : Coordinate.ToOffset(Game1.MouseScreenPos);
           
            if (offsetfixed)
            {
                pos = original_pos ;
            
            }

            else
            {
                pos = original_pos + Game1.offset.ToVector2();
            }


            bool LeftClick = FlatMouse.Instance.IsLeftMouseButtonPressed();
            bool RightClick = FlatMouse.Instance.IsRightMouseButtonPressed();


            if (HoverButton(CursorPos, cameraused))
            {
                isHovered = true;

                if (LeftClick || RightClick)
                {
                    isHovered = false;
                    isPressed = true;
                }



                else if (FlatMouse.Instance.IsLeftMouseButtonReleased())
                {
                    RunBtnClick();
                }

                else if (FlatMouse.Instance.IsRightMouseButtonReleased())
                {
                    RunRightBtnClick();
                }
            }

            else
            {
                isHovered = false;
            }

            if ((!LeftClick || !RightClick) && !FlatMouse.Instance.IsLeftMouseButtonDown())
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

        public void RunRightBtnClick()
        {
            if (ButtonRightClicked == null)
            {
                _ButtonRightClicked?.Invoke();
            }

            else
            {
                ButtonRightClicked?.Invoke(this);
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

            Vector2 vec = offsetfixed ? Vector2.Zero : Game1.offset.ToVector2() ;


         

            Color tmpColor = Color.White;
            if (isPressed)
            {
                tmpColor = Color.Gray;
            }

            else if (isHovered)
            {
                tmpColor = hoverColor;

                if (hover_text != null)
                {
                    Vector2 ButtontextDims = textfont.MeasureString(hover_text);
                    sprite.DrawString(textfont, hover_text, new Vector2(original_pos.X + vec.X - ButtontextDims.X / 2, original_pos.Y + vec.Y - ButtontextDims.Y / 2), ButtonTextColor);
                    return;

                }



            }

            if (SecondDraw != null)
            {
                sprite.Draw(SecondDraw, new Rectangle((int)(original_pos.X+ vec.X), (int)(original_pos.Y+vec.Y), (int)dims.X, (int)dims.Y), tmpColor, 0f,
              new Vector2(SecondDraw.Bounds.Width / 2, SecondDraw.Bounds.Height / 2));
            }

            sprite.Draw(model, new Rectangle((int)(original_pos.X + vec.X), (int)(original_pos.Y + vec.Y), (int)dims.X, (int)dims.Y), tmpColor, 0f,
            new Vector2(model.Bounds.Width / 2, model.Bounds.Height / 2));

            if (button_text != null)
            {
                Vector2 ButtontextDims = textfont.MeasureString(button_text);
                sprite.DrawString(textfont, button_text, new Vector2(original_pos.X + vec.X - ButtontextDims.X / 2, original_pos.Y + vec.Y - ButtontextDims.Y / 2), ButtonTextColor);
            }

        }

  

    }
}
