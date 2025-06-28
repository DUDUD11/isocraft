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

        int z = 0;

        Point center_pos;
        Rectangle draw_rect;
       

        public Tile(string path, Vector2 init_pos, Vector2 dims, int angle,int z) : 
            base(path, init_pos, dims, angle)
        {
            this.z = z;

        }

        public Tile Clone()
        {
            return new Tile(this.url, this.pos, this.dims, (int)((this.angle+0.01f) / MathHelper.PiOver4), this.z);
        
        }

        public override void Draw(Sprites sprite)
        {
           

            Vector2 _dim = TileMap.Tile_Size * dims;
            Vector2 point = Coordinate.ToIsometric(pos.X, pos.Y);

            Rectangle draw_rect = new Rectangle(point.ToPoint(), _dim.ToPoint());

         //   Console.WriteLine(_dim);
            
            sprite.Draw(model,draw_rect,Color.White,0f,new Vector2(model.Bounds.Width/2,model.Bounds.Height/2));
        }

        //public static Point Center_Pos(Vector2 pos, Point dims)
        //{ 
        //    int x = (int)(pos.X + (dims.X - 1)) * TileMap.Tile_Size;
        //    int y = (int)(pos.Y + (dims.Y - 1)) * TileMap.Tile_Size;

        //    return new Point(x, y);
        //}

     



    }
}
