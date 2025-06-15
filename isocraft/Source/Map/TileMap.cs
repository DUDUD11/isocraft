using Flat;
using Flat.Graphics;

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;


namespace isocraft
{
    public static class TileMap
    {
        public static int Tile_Size = 48;
        public static Vector2 Tile_Dims = new Vector2(Tile_Size, Tile_Size);

        public static List<Tile> FirstFloorTiles = new();
        public static List<Tile> SecondFloorTiles = new();

        //public static List<StaticTile> StaticTiles = new List<StaticTile>();
        //public static List<DynamicTile> DynamicTiles = new List<DynamicTile>();
        //public static List<FlatBody> SpecialTileFlatBody = new List<FlatBody>();

        public static void Add_Tile(Tile tile, int z)
        {
            if (z == 1)
            {
                FirstFloorTiles.Add(tile);
            }

            else if (z == 2)
            {
                SecondFloorTiles.Add(tile);
            }
        
        
        }


        public static void Clear()
        {

            FirstFloorTiles.Clear();
            SecondFloorTiles.Clear();
            //StaticTiles.Clear();
            //DynamicTiles.Clear();
            //SpecialTileFlatBody.Clear();
        }

        //public static void Interact(FlatBody tilebody, SpriteEntity spriteEntity)
        //{
        //    for (int i = 0; i < DynamicTiles.Count; i++)
        //    {
        //        if (DynamicTiles[i].flatBody == tilebody)
        //        {
        //            DynamicTiles[i].Interact(spriteEntity);
        //        }
        //    }
        //}


        //public static void Update(Hero hero)
        //{
        //    for (int i = 0; i < SpecialTileFlatBody.Count; i++)
        //    {
        //        FlatBody tmp = SpecialTileFlatBody[i];

        //        if (hero.FlatBody.LinearVelocity.Y > 0 || hero.FlatBody.Position.Y < tmp.Position.Y - tmp.height / 2)
        //        {
        //            tmp.active = false;
        //            사실 heroactive 하면 좋을듯 아니면 특정 world type에만

        //        }

        //        else
        //        {
        //            tmp.active = true;
        //        }

        //    }

        //    for (int i = 0; i < TileMap.DynamicTiles.Count; i++)
        //    {
        //        TileMap.DynamicTiles[i].Update();
        //    }
        //}

        public static void Draw(Sprites sprites, Vector2 o)
        {
            for (int i = 0; i < FirstFloorTiles.Count; i++)
            {
                FirstFloorTiles[i].Draw(sprites);
            }

            for (int i = 0; i < SecondFloorTiles.Count; i++)
            {
                SecondFloorTiles[i].Draw(sprites);
            }

        }


        }
}
