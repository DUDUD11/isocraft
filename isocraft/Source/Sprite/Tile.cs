using Flat;
using Flat.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;


namespace isocraft
{
    public class Tile : SpriteEntity
    {
        //          0
        //       1     7  
        //     2        6     
        //       3    5
        //         4

        int z = 1;

        Point center_pos;
        Rectangle draw_rect;
        float angle;

        public Tile(string path, Vector2 init_pos, Vector2 dims, int dir,int z) : 
            base(path, init_pos, dims, dir)
        {
            this.z = z;

            // 1일때 그대로
            // 2일때 0.5 * tilesize
            //3일때 1.0

            dims = TileMap.Tile_Size * dims / FlatMath.InvSqrt2;
              

            center_pos.X = (int)(init_pos.X + (dims.X - 1)) * TileMap.Tile_Size;
            center_pos.Y = (int)(init_pos.Y + (dims.Y - 1)) * TileMap.Tile_Size;
            draw_rect = new Rectangle(center_pos, dims.ToPoint());
            angle = dir * MathHelper.PiOver4;
        }

        public override void Draw(Sprites sprite)
        {
            sprite.Draw(model,draw_rect,Color.White, rotation: angle,Vector2.Zero);
        }

        public static Point Center_Pos(Vector2 pos, Point dims)
        { 
            int x = (int)(pos.X + (dims.X - 1)) * TileMap.Tile_Size;
            int y = (int)(pos.Y + (dims.Y - 1)) * TileMap.Tile_Size;

            return new Point(x, y);
        }

        public void Change_Dims(Point Add)
        {
            if ((Add.X < 0 && (dims.X <= TileMap.Tile_Size / FlatMath.InvSqrt2 + 0.1f)) || (Add.Y < 0 && (dims.Y <= TileMap.Tile_Size / FlatMath.InvSqrt2 + 0.1f)))
                return;


            dims += TileMap.Tile_Size * dims / FlatMath.InvSqrt2;

            center_pos.X = (int)(pos.X + (dims.X - 1)) * TileMap.Tile_Size;
            center_pos.Y = (int)(pos.Y + (dims.Y - 1)) * TileMap.Tile_Size;
            draw_rect = new Rectangle(center_pos, dims.ToPoint());
        }

        public void Change_Dir(int Add)
        {
            dir += Add;
            angle = dir * MathHelper.PiOver4;
        }


    }
}
