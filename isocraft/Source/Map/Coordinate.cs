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

        public Point[,] isoMap;
        // 타일이없으면 못가게
        public bool[,] TileMapping;
        public Point[,] Info;


        private static Point screen_offset = new Point(Game1.screen_width / 2,Game1.screen_height/2);

        public int width;
        public int height;

        public bool Place_Unit(Point point, Point type)
        {
            return Place_Unit(point, type, new Point(1, 1));
        }

        // 크기좀 다른 hero나, enemy등은 사이즈 1로 고정시키고 true반환
        private bool Place_Unit(Point point, Point type,Point dims)
        {

         


            if (type.X == (int)GameEnums.Type.Hero || type.X == (int)GameEnums.Type.Enemy)
            {
                if (Info[point.Y , point.X].X != (int)GameEnums.Type.None)
                {
                    return false;
                }

                Info[point.Y , point.X] = type;
                return true;
            }

            if (dims.X > 3)
            {
                throw new Exception("");
            }


            for (int i = 0; i < dims.Y; i++)
            {
                for (int j = 0; j < dims.X; j++)
                {

                    if (Info[point.Y+i, point.X+j].X != (int)GameEnums.Type.None)
                    {
                        return false;
                    }

                    Info[point.Y+i, point.X+j] = type;


                }

            }

            return true;
        }


        
        public void Move_Unit(Point Start, Point End, Point type, Point dims)
        {

            if (Info[Start.Y, Start.X].X == (int)GameEnums.Type.None)
            {
                throw new Exception("err");
            }




            for (int i = 0; i < dims.Y; i++)
            {
                for (int j = 0; j < dims.X; j++)
                {
                    Info[Start.Y + i, Start.X + j].X = (int)GameEnums.Type.None;
                    Info[End.Y + i, End.X + j] = type;
                }
            }


        }








        public void Move_Unit(Point Start, Point End, Point type)
        {
           Move_Unit(Start, End, type, new Point(1, 1));

        

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
            isoMap = new Point[height, width];
            Info = new Point[height, width];
            TileMapping = new bool[height, width];
            this.width = width;
            this.height = height;
   
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    isoMap[i, j] = ToIsometric(j,i); 
                }
            }

            // 일단 타일의 dims는 1로고정

            for (int i = 0; i < TileMap.FirstFloorTiles.Count; i++)
            {
                Tile tile = TileMap.FirstFloorTiles[i];
                TileMapping[(int)tile.pos.Y,(int)tile.pos.X] = true;
             

            }

            ////// 일단 타일의 dims는 1로고정
            //for (int i = 0; i < TileMap.SecondFloorTiles.Count; i++)
            //{
            //    Tile tile = TileMap.SecondFloorTiles[i];
            //    TileMapping[(int)tile.pos.Y, (int)tile.pos.X] = true;

            //}

            for (int i = 0; i < EntityManager.AllEntities.Count; i++)
            {
                SpriteEntity sprite = EntityManager.AllEntities[i];

                Point pos = sprite.pos.ToPoint();

                Place_Unit(pos, GameEnums.Type_ret(sprite), sprite.dims.ToPoint());

            //    Info[pos.Y, pos.X] = GameEnums.Type_ret(sprite);
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

        //오프셋이 적용된 위치
        public static Vector2 ToOffset(Vector2 screen)
        {
            int zoom = (Game1.Display_size.X / (2 * screen_offset.X));

            screen /= zoom;
          

            screen.X += Game1.offset.X;
            screen.Y+= Game1.offset.Y;

            //int tileX = (screenX / (TileMap.Tile_Size / 2) + screenY / (TileMap.Tile_Size / 2)) / 2;
            //int tileY = (screenY / (TileMap.Tile_Size / 2) - screenX / (TileMap.Tile_Size / 2)) / 2;

            return screen;
        }



        public bool Vaild(Point point)
        {
            if (point.X < 0 || point.Y < 0) return false;

            return point.X < width && point.Y < height;
        }


    }
}


