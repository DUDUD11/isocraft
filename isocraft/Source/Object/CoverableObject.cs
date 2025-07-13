using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flat.Graphics;

namespace isocraft
{
    public class CoverableObject : SpriteAnimated
    {
        public int Cover_Percent;
        public int z;
        public int[,] Cover_Vector;


        public CoverableObject(string path,Vector2 pos, Vector2 dims,int Cover_Percent,int z,int[,] vector,int dir, int dirNum,Vector2 frames,int animation_num, 
            int totalAnimationNum, int millisecondFrame, string name = null) : 
            base(path, pos, dims, dir, 0, dirNum, frames, animation_num,
    totalAnimationNum, millisecondFrame, name ?? "Idle")
        {
            // cover_vector ...

            this.Cover_Percent = Cover_Percent;
            this.z = z;
            this.Cover_Vector = vector;
        }
// 빌딩인경우에 
// 2x2 이런걸 설정하게 하고 (버튼으로) 클래스는 그렇게만들면될듯


        public virtual void Interact()
        { 
        
        }

        public bool Mapping(Point coord)
        {
            if (coord.X >= (int)pos.X && coord.X <= (int)(pos.X + dims.X) && coord.Y >= (int)pos.Y && coord.Y <= (int)(pos.Y + dims.Y)) return true;

            return false;

        }

        public override void Draw(Sprites sprite)
        {
            base.Draw(sprite);
        }

      

        public float Covering_Vec(Vector2 val)
        {

        

            bool left = false;
            bool right = false;
            bool up = false;
            bool down = false;

            int coverX = (int)val.X - (int)this.pos.X;
            int coverY = (int)val.Y - (int)this.pos.Y;

            if (coverX == 0) left = true;
            if (coverX == (int)dims.X)
            {
                right = true;
                coverX -= 1;
            }

            if (coverY == 0) down = true;
            if (coverY == (int)dims.Y)
            {
                up = true;
                coverY -= 1;
            }

            int coverage = Cover_Vector[coverY,coverX];

            if (coverage == 100)
            {
                return (50 + 25 * z) * Cover_Percent;
            }

            float leftX = val.X - (int)val.X;
            float leftY = val.Y - (int)val.Y;

            if ((!left && !right) || (!up && !down))
            {
                return (coverage/2 + 25 * z) * Cover_Percent;
            }

            if (left && down)
            {
                if (100 * leftX <= 100 - coverage || 100 * leftY <= 100 - coverage)
                {
                    return 0;
                }

                else
                {
                    return (coverage / 2 + 25 * z) * Cover_Percent;
                }
            }

            else if (left && up)
            {
                if (100 * leftX <= 100 - coverage || 100 * leftY >=  coverage)
                {
                    return 0;
                }

                else
                {
                    return (coverage / 2 + 25 * z) * Cover_Percent;
                }

            }


            else if (right && down)
            {
                if (100 * leftX >= coverage || 100 * leftY <= 100 - coverage)
                {
                    return 0;
                }

                else
                {
                    return (coverage / 2 + 25 * z) * Cover_Percent;
                }
            }

            else if (right && up)
            {
                if (100 * leftX >= coverage || 100 * leftY >= coverage)
                {
                    return 0;
                }

                else
                {
                    return (coverage / 2 + 25 * z) * Cover_Percent;
                }

            }

            throw new Exception("Logically can here");


        }

        //public static CoverableObject ParseToInstance(Object obj, int x, int y)
        //{
        //    throw new ArgumentNullException("");    
        
        //}

        //public static Object ParseToObject(CoverableObject obj)
        //{
        //    throw new ArgumentNullException("");

        //}

    }
}
