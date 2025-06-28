using Flat.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace isocraft
{
    public class Building : CoverableObject
    {
        public Building(string path, Vector2 pos, Vector2 dims, int Cover_Percent, int z, int[,] vector, int dir, int dirNum, Vector2 frames, int animation_num, int totalAnimationNum, int millisecondFrame, string name = null) 
            : base(path, pos, dims, Cover_Percent, z, vector, dir, dirNum, frames, animation_num, totalAnimationNum, millisecondFrame, name)
        {


        }

        public static Building ParseToInstance(object obj,int x,int y)
        {

            string str;

            if (obj is JsonElement element)
            {
                str = element.GetProperty("Item1").GetString();

            }

            else
            {
                str = (string)obj;
            }

            //int idx = str.Length - 1;

            //for (int i = idx; i > 0; )
            //{
            //    if (str[i-1] == '\\')
            //    {
            //        idx = i;
            //        break;
            //    }
            //    i--;
            //}

            //str = str.Substring(idx);


               
            int[,] mat = GameEnums.BuildingDictionary[str];

            int z = str.Equals(GameEnums.BuildingDirPath+"Building - size 3 - height 1 typeA - white-i") ? 1:2;

            // 일단 covering 100
           
            Building building = new Building(str, new Vector2(x, y),new Vector2(mat.GetLength(1), mat.GetLength(0)), 100, z, mat, 0, 0, new Vector2(1, 1), 1, 1, 200);


            return building;
        }

        public static Object ParseToObj(Building building)
        {
            Object obj = (Object)building.url;


            return obj;
        }

        public static Building Clone(Building building)
        {
            return new Building(building.url, building.pos, building.dims, building.Cover_Percent, building.z, building.Cover_Vector, 0, 1, new Vector2(1, 1), 1, 1, 200);
        
        }







        public override void Draw(Sprites sprite)
        {

            // animation 제거

            Vector2 _dim = TileMap.Tile_Size * dims ;
            Vector2 point = Coordinate.ToIsometric(pos.X+(dims.X)/2f, pos.Y+(dims.Y)/2f);

            //Console.WriteLine(dims);

            Rectangle draw_rect = new Rectangle(point.ToPoint(), _dim.ToPoint());
            sprite.Draw(model, draw_rect, Color.White,0f,new Vector2(model.Bounds.Width/2, model.Bounds.Height/2));
        }


    }
}
