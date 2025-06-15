using Flat;
using Flat.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace isocraft
{

    public struct ani_set
    {
        public Vector2 Frames;
        public Vector2 FrameSize;
        public string path;
        public int dir;

        public ani_set(Vector2 frame, Vector2 size,int dir, string path)
        {
            this.Frames = frame;
            this.FrameSize = size;
            this.dir = dir;
            this.path = path;
        }
    }

    public class SpriteAnimated : SpriteEntity
    {
        public readonly int AnimationNum;
        public int AnimationDirNum;

        public List<ani_set> Animation_Set;
        private List<FrameAnimation> FrameAnimationList;
        public bool AnimationFlag;
        public int currentAnimation;


        public SpriteAnimated(string path, Vector2 init_pos, Vector2 dims, int dir,int dirNum, Vector2 frames, int animationNum, int totalframe, int millitimePerFrame, string name)
        : base(path, init_pos, dims,dir)
        {
            this.AnimationNum = animationNum;
            this.Animation_Set = new List<ani_set>(animationNum);
            this.AnimationDirNum = dirNum;

            FrameAnimationList = new List<FrameAnimation>(animationNum);
            AnimationFlag = true;
            currentAnimation = 0;
            AddAnimation(frames, path, totalframe, millitimePerFrame, name, currentAnimation);
            ChangeDirCurrentAnimaition(dir);
        }

        public void Set_repeat(int idx, bool flag)
        {
            FrameAnimationList[idx].repeat = flag;
        }

        public void AddAnimation(Vector2 frames, string path, int totalframes, int millitimePerFrame, string NAME, int idx,bool repeat = true)
        {
            // model bound와 height는 변하지 않는다고 가정할때
            if (idx == 0)
            {
                Animation_Set.Add(new ani_set(frames, new Vector2((int)(model.Bounds.Width / frames.X), (int)(model.Bounds.Height / frames.Y)), AnimationDirNum, path));
            }

            else
            {
                Animation_Set.Add(new ani_set(frames, Animation_Set[0].FrameSize, AnimationDirNum, path));
            }
            FrameAnimationList.Add(new FrameAnimation(FlatMath.VectorZero, Animation_Set[idx].Frames, (int)Animation_Set[idx].Frames.X, Vector2.Zero, totalframes, AnimationDirNum, millitimePerFrame, NAME));

            if (!repeat)
            {
                Set_repeat(idx,false);
            }
        
        }

        public bool DeleteAnimation(string animationanme)
        {
            for (int i = 0; i < FrameAnimationList.Count; i++)
            {
                if (FrameAnimationList[i].name == animationanme)
                {
                    FrameAnimationList.RemoveAt(i);
                    Animation_Set.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public void ChangeCurrentAnimation(int animationNum)
        {
            FrameAnimationList[currentAnimation].Reset();
            this.currentAnimation = animationNum;
            base.UpdateModel(Animation_Set[currentAnimation].path);
        }

        public void ChangeDirCurrentAnimaition(int dir)
        {
            FrameAnimationList[currentAnimation].Change_Dir(dir);
        }



        public void SetAnimationFlag(bool flag)
        {
            AnimationFlag = flag;
        }

        public Rectangle Src_Rectangle()
        {
            return FrameAnimationList[currentAnimation].Source_rectangle;
        }

        public Vector2 Get_Sheet()
        {
            return FrameAnimationList[currentAnimation].sheet;
        }

        public Vector2 Get_SheetFrame()
        {
            return FrameAnimationList[currentAnimation].sheetFrame;
        }


        public override void Update()
        {
            if (AnimationFlag && FrameAnimationList.Count > 0)
            {
                FrameAnimationList[currentAnimation].Update();
            }

        }

        public string GetCurrentAnimationModelPath()
        {
            return Animation_Set[currentAnimation].path;
        }


        public virtual int GetAnimationFromName(string ANIMATIONNAME)
        {
            for (int i = 0; i < FrameAnimationList.Count; i++)
            {
                if (FrameAnimationList[i].name.Equals(ANIMATIONNAME))
                {
                    return i;
                }
            }

            return -1;
        }

        public void SetAnimationByName(string NAME)
        {
            int tempAnimation = GetAnimationFromName(NAME);

            if (tempAnimation == -1)
            {
                throw new Exception("NO Animation Found");
            }

            if (tempAnimation != currentAnimation)
            {
                FrameAnimationList[tempAnimation].Reset();
            }

            currentAnimation = tempAnimation;
        }

        public override void Draw(Sprites sprite, int dir)
        {
            //hero 등에서는 이미 바꾸기때문
            //     Game1.AntiAliasingShader(model, dims, Animation_Set[currentAnimation].FrameSize);

            // 반에 걸쳐있는경우 계산해야함

            Vector2 point = Coordinate.ToIsometric(pos.X, pos.Y);


            if (AnimationFlag && FrameAnimationList.Count != 0 && FrameAnimationList[currentAnimation].Frames > 0)
            {
                FrameAnimationList[currentAnimation].Draw(sprite, Animation_Set[currentAnimation].FrameSize, model, new Rectangle((int)(point.X ), (int)(point.Y ), (int)dims.X, (int)dims.Y), 0f, Color.White);
            }
            else
            {

                Rectangle Source_rectangle = new Rectangle(0, 0, (int)Animation_Set[currentAnimation].FrameSize.X, (int)Animation_Set[currentAnimation].FrameSize.Y);
                sprite.Draw(model, new Rectangle((int)(point.X ), (int)(point.Y ), (int)dims.X, (int)dims.Y), Source_rectangle, Color.White, 0f,
                    new Vector2((int)(model.Bounds.Width / (2 * Animation_Set[currentAnimation].Frames.X)), (int)(model.Bounds.Height / (2 * Animation_Set[currentAnimation].Frames.Y))));

            }
        }






    }
}
