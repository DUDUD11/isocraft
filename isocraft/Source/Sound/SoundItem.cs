using Flat;
using isocraft;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isocraft
{
    public class SoundItem
    {
        public float volume;
        public string name;
        public SoundEffect sound;
        public SoundEffectInstance instance;


        public SoundItem(string Name, string soundpath, float volume)
        {
           
            name = Name;
            this.volume = FlatUtil.Clamp(volume, 0f, 1f);
            sound = Game1._Instance.Content.Load<SoundEffect>(soundpath);
            CreateInstance();

        }

        public SoundItem(SoundItem soundItem)
        {
            name = soundItem.name;
            this.volume = FlatUtil.Clamp(soundItem.volume, 0f, 1f);
            sound = soundItem.sound;
            CreateInstance();
        }

        public void CreateInstance()
        {
            instance = sound.CreateInstance();
        }

    }
  
}

