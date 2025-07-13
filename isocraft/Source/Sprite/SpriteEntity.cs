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
    public class SpriteEntity
    {
        //          0
        //       1     7  
        //     2        6     
        //       3    5
        //         4
        public Vector2 pos, dims;
        public bool updateRequired = false;
        public float angle;
        public Texture2D model;
        public string url;
        public bool Destroy = false;
        public bool Cancel = false;


        public SpriteEntity(string path, Vector2 init_pos, Vector2 dims, int angle)
        {
            url = path;
            model = Game1._Instance.Content.Load<Texture2D>(path);
            this.pos = init_pos;
            this.dims = dims;
            this.angle = angle * MathHelper.PiOver4;
        }

        public void angle_add(int _angle)
        { 
            this.angle += _angle * MathHelper.PiOver4;

        }

        public void dims_add(int dims)
        {
            this.dims += new Vector2(dims, dims);

        }



        public virtual void Update()
        {

        }

        //public virtual void Update(Hero hero)
        //{
        //    throw new ArgumentNullException("draw method must override ");
        //}

        //public virtual void Update(List<Mob> mob)
        //{


        //}
        //public virtual void Update(List<Mob> mob, Vector2 AdjustPos)
        //{


        //}

        
        public void UpdateModel(string path)
        {
            model = Game1._Instance.Content.Load<Texture2D>(path);
        }

        public Texture2D GetModel()
        {
            return model;
        }


        public virtual void Destroy_Sprite()
        {
            Destroy = true;
        }
  

        public virtual void Draw(Sprites sprite)
        {
            throw new ArgumentNullException("draw method must override ");
        }

        public virtual void Draw(Sprites sprite,bool flag)
        {
            throw new ArgumentNullException("draw method must override ");
        }

        //public virtual void Draw(Sprites sprite, int dir)
        //{
        //    throw new ArgumentNullException("draw method must override ");
        //}

        public virtual void Selected()
        {
            throw new ArgumentNullException("draw method must override ");
        }

        public virtual void Turn_End()
        {
            //throw new ArgumentNullException("draw method must override ");

        }


        //public virtual void LeftClicked()
        //{
        //    throw new ArgumentNullException("draw method must override ");
        //}

        //public virtual void RightClicked()
        //{
        //    throw new ArgumentNullException("draw method must override ");
        //}

    }
}
