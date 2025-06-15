using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using static System.Net.Mime.MediaTypeNames;
using Flat.Graphics;
using Flat.Input;

namespace isocraft
{
    public class TextInputBox : UIEntity
    {
        private SpriteFont font;
        private string currentInput = "";
        private string hint = "";
        private Color boxColor = Color.Gray;
        private Color textColor = Color.Black;

        private Keys pastKey = Keys.F6;
        private double repeat_cooltime = 0.5f;
        private double repeat_timer=0f;

        HashSet<Keys> key_set = new();

        public TextInputBox(string currentInput,string hint, Vector2 init_pos, Vector2 dims, bool active,string font=null) 
            : base("UI\\solid", init_pos, dims, active, 0, 0, new Vector2(1,1), 1, 0, null)
        {
            if (font != null)
            {
                this.font = Game1._Instance.Content.Load<SpriteFont>(UIEntity.Font16);
            }

            if (currentInput != null)
            {
                this.currentInput = currentInput;
            }

            if (hint != null)
            {
                this.hint = hint;
            }
        }

        public override void Update()
        {
            Vector2 mousePos = Game1.MouseScreenPos;

            if (Hover(mousePos) && FlatKeyboard.Instance.IsKeyAvailable)
            {
                if (repeat_timer + repeat_cooltime < WorldTimer.Instance.totalTime())
                {
                    repeat_timer = WorldTimer.Instance.totalTime();
                    key_set.Clear();
                }

                foreach (var key in FlatKeyboard.Instance.GetPressedKeys())
                {
                    if (!key_set.Contains(key))
                    {
                        key_set.Add(key);

                        if (key == Keys.Back && currentInput.Length > 0)
                        {
                            currentInput = currentInput.Substring(0, currentInput.Length - 1);
                        }

                        if (key >= Keys.A && key <= Keys.Z) // 알파벳 처리 (대소문자)
                        {
                            currentInput += key.ToString(); // 입력된 문자를 추가
                        }
                        else if (key >= Keys.D0 && key <= Keys.D9) // 숫자 키 처리
                        {
                            currentInput += (key - Keys.D0).ToString();
                        }
                        else if (key == Keys.Space) // 공백 처리
                        {
                            currentInput += " ";
                        }

                        else if (key == Keys.OemMinus)
                        {
                            currentInput += "-";
                        }

                        else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                        {
                            currentInput += (key - Keys.NumPad0).ToString();
                        }

                    }
                }

            }


        }

        public override void Draw(Sprites sprite)
        {

            base.Draw(sprite);

       
            if (currentInput != null && !currentInput.Equals(""))
            {
                Vector2 TextInputDims = font.MeasureString(currentInput);
                sprite.DrawString(font, currentInput, new Vector2(pos.X - TextInputDims.X / 2, pos.Y - TextInputDims.Y / 2), Color.Black);

            }

            else if (hint != null && !hint.Equals(""))
            {
                Vector2 HintDims = font.MeasureString(hint);
                sprite.DrawString(font, hint, new Vector2(pos.X - HintDims.X / 2, pos.Y - HintDims.Y / 2), Color.Red);
            }

        }



        public string GetInput()
        {
            return currentInput;
        }

        public void SetInput(string str)
        {
            this.currentInput = str;
        }


    }
}
