using Flat.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace isocraft
{
    public class TextZone
    {

        public int maxWidth, lineHeight;
        string str;
        public Vector2 pos, dims;
        public Color color;
        public SpriteFont font;

        public List<string> lines = new List<string>();
        public TextZone(Vector2 pos, string str, int maxwidth, int lineheight, SpriteFont font, Color fontcolor)
        {

            this.pos = pos;
            this.str = str;
            this.maxWidth = maxwidth;
            this.lineHeight = lineheight;
            this.color = fontcolor;
            this.font = font;


            if (str != null && !str.Equals(""))
            {
                ParseLines();
            }


        }




        #region Properties
        public string Str
        {
            get { return str; }
            set
            {
                str = value;
                ParseLines();
            }

        }
        #endregion
        public void ParseLines()
        {
            lines.Clear();
            List<string> wordList = new List<string>();
            string tempString = "";

            int largeswidth = 0, currentwidth = 0;

            if (str != null && str != "")
            {
                wordList = str.Split(' ').ToList<string>();

                for (int i = 0; i < wordList.Count; i++)
                {
                    if (tempString != "")
                    {
                        tempString += " ";
                    }

                    currentwidth = (int)(font.MeasureString(tempString + wordList[i]).X);

                    if (currentwidth > largeswidth && currentwidth <= maxWidth)
                    {
                        largeswidth = currentwidth;
                    }
                    if (currentwidth <= maxWidth)
                    {
                        tempString += wordList[i];
                    }

                    else
                    {
                        lines.Add(tempString);
                        tempString = wordList[i];
                    }


                }


                if (tempString != "")
                {
                    lines.Add(tempString);
                }

                SetDims(largeswidth);


            }



        }

        public void SetDims(int Largeswidth)
        {
            dims = new Vector2(Largeswidth, lineHeight * lines.Count);
        }

        public void Draw(Sprites sprites)
        {
            for (int i = 0; i < lines.Count; i++)
            {
               
                sprites.DrawString(font, lines[i], new Vector2((int)pos.X, (int)pos.Y + (lineHeight * i)), color);

            }
        }
    }
}


    }
}

