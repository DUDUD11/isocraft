using Flat;
using isocraft;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace isocraft
{
    public class Coordinate
    {
        private static Coordinate _instance;

        public Point[,] Map;
        // 타일이없으면 못가게
        public bool[,] TileMapping;
        public Point[,] Info;


        private static Point screen_offset = new Point(Game1.screen_width / 2,Game1.screen_height/2);

        public int width;
        public int height;

        public bool Place_Unit(Point point, Point type)
        {
            if (Info[point.Y, point.X].X != (int)GameEnums.Type.None)
            {
                return false;
            }

            Info[point.Y, point.X] = type;

            return true;
        }

        public void Move_Unit(Point Start, Point End, Point type)
        {

            if (Info[Start.Y, Start.X].X == (int)GameEnums.Type.None)
            {
                throw new Exception("err");
            }


            Info[Start.Y, Start.X].X = (int)GameEnums.Type.None;
            Info[End.Y, End.X] = type;

        }

        public bool boundary_check(Point point)
        {
            if (point.X < 0 || point.Y < 0 || point.X >= width || point.Y >= height)
                return false;
            return true;
        
        }



        private Coordinate()
        { 
        
        }

        public static Coordinate Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Coordinate();
                return _instance;
            }
        }

        public void Init(int width, int height)
        {
            Map = new Point[height, width];
            Info = new Point[height, width];

            this.width = width;
            this.height = height;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Map[i, j] = ToIsometric(j,i); 
                }
            }
        }


        public static Point ToIsometric(int tileX, int tileY)
        {
            int screenX = (tileX - tileY) * (TileMap.Tile_Size / 2);
            int screenY = (tileX + tileY) * (TileMap.Tile_Size / 2);

            //int screenX = (tileX) * (TileMap.Tile_Size / 2);
            //int screenY = (tileY ) * (TileMap.Tile_Size / 2);

            return new Point(screenX, screenY)+ screen_offset;
        }

        public static Vector2 ToIsometric(float tileX, float tileY)
        {
            //float screenX = (tileX) * (TileMap.Tile_Size / 2);
            //float screenY = (tileY) * (TileMap.Tile_Size / 2);
            float screenX = (tileX - tileY) * (TileMap.Tile_Size / 2f);
            float screenY = (tileX + tileY) * (TileMap.Tile_Size / 2f);
            return new Vector2(screenX, screenY) + screen_offset.ToVector2();
        }



        // 스크린 좌표를 타일 좌표로 변환
        public static Point ToTile(int screenX, int screenY)
        {
            int zoom = (Game1.Display_size.X / (2 * screen_offset.X));

            screenX /= zoom;
            screenY /= zoom;

            screenX += Game1.offset.X;
            screenY += Game1.offset.Y;

            screenX -= screen_offset.X;
            screenY -= screen_offset.Y;

            float _tileX = (screenX / (TileMap.Tile_Size / 2f) + screenY / (TileMap.Tile_Size / 2f)) / 2f;
            float _tileY = (screenY / (TileMap.Tile_Size / 2f) - screenX / (TileMap.Tile_Size / 2f)) / 2f;

            int tileX = (int)MathF.Round(_tileX);
            int tileY= (int)MathF.Round(_tileY);


            //int tileX = (screenX / (TileMap.Tile_Size / 2) + screenY / (TileMap.Tile_Size / 2)) / 2;
            //int tileY = (screenY / (TileMap.Tile_Size / 2) - screenX / (TileMap.Tile_Size / 2)) / 2;

            


            return new Point(tileX, tileY);
        }

        public bool Vaild(Point point)
        {
            if (point.X < 0 || point.Y < 0) return false;

            return point.X < width && point.Y < height;
        }


    }
}


