using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace isocraft
{
    public static class Shader
    {


        public static Effect AntiAliasEffect;
        public static Effect defaulteffect;
        public static Effect ThrobEffect;
        public static Effect ShrinkCircleEffect;

        public static void init()
        {
            defaulteffect = Game1._Instance.Content.Load<Effect>("Shader\\base");
            AntiAliasEffect = Game1._Instance.Content.Load<Effect>("Shader\\antialiasing");
            ThrobEffect = Game1._Instance.Content.Load<Effect>("Shader\\Throb");
            ShrinkCircleEffect = Game1._Instance.Content.Load<Effect>("Shader\\shrinkCircle");

        }

        public static void AntiAliasingShader(Texture2D texture, Vector2 dims, int Mirror = 0)
        {
            AntiAliasEffect.Parameters["xSize"].SetValue((float)texture.Bounds.Width);
            AntiAliasEffect.Parameters["ySize"].SetValue((float)texture.Bounds.Height);
            AntiAliasEffect.Parameters["xDraw"].SetValue((float)(int)dims.X);
            AntiAliasEffect.Parameters["yDraw"].SetValue((float)(int)dims.Y);
            AntiAliasEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            AntiAliasEffect.Parameters["Mirror"].SetValue(Mirror);
            AntiAliasEffect.CurrentTechnique.Passes[0].Apply();
        }

        public static void AntiAliasingShader(Texture2D texture, Vector2 dims, Vector2 frameSize, int Mirror = 0)
        {
            AntiAliasEffect.Parameters["xSize"].SetValue(frameSize.X);
            AntiAliasEffect.Parameters["ySize"].SetValue(frameSize.Y);
            AntiAliasEffect.Parameters["xDraw"].SetValue((float)(int)dims.X);
            AntiAliasEffect.Parameters["yDraw"].SetValue((float)(int)dims.Y);
            AntiAliasEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            AntiAliasEffect.Parameters["Mirror"].SetValue(Mirror);
            AntiAliasEffect.CurrentTechnique.Passes[0].Apply();
        }

        public static void NoAntiAliasingShader(Color color)
        {
            defaulteffect.CurrentTechnique.Passes[0].Apply();
        }

        public static void ThrobShader(float sin, Color color, int Mirror = 0)
        {
            ThrobEffect.Parameters["SINLOC"].SetValue(sin);
            ThrobEffect.Parameters["filterColor"].SetValue(color.ToVector4());
            ThrobEffect.Parameters["Mirror"].SetValue(Mirror);
            ThrobEffect.CurrentTechnique.Passes[0].Apply();
        }

        public static void ShrinkCircleShader(float value, Color color, Vector2 Framesize, Vector2 origin)
        {
            ShrinkCircleEffect.Parameters["param1"].SetValue(1.0f - value);
            ShrinkCircleEffect.Parameters["filterColor"].SetValue(color.ToVector4());
            ShrinkCircleEffect.Parameters["framesize"].SetValue(Framesize);
            ShrinkCircleEffect.Parameters["origin"].SetValue(origin);
            ShrinkCircleEffect.CurrentTechnique.Passes[0].Apply();

        }

    }

}
