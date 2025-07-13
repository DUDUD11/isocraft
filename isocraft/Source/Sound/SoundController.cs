using Flat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isocraft
{
    public static class SoundController
    {
        public static SoundItem BkgSound;
        public static SoundItem EffectSound;
        public static float EffectSound_Volume = 0.5f;
        public static List<SoundItem> Sounds = new List<SoundItem>();

        public static void Init()
        {
            SoundAdd(new SoundItem("Modern8", "sound\\Modern8", EffectSound_Volume));
            SoundAdd(new SoundItem("Modern15", "sound\\Modern15", EffectSound_Volume));

            SoundAdd(new SoundItem("9mm Single", "sound\\9mm Single",EffectSound_Volume));
            SoundAdd(new SoundItem("Shooting_Miss", "sound\\Shooting_Miss", EffectSound_Volume));
            SoundAdd(new SoundItem("9mm Reload", "sound\\9mm Reload", EffectSound_Volume));

            SoundAdd(new SoundItem("confirmation", "sound\\Male\\confirmation_1_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("confirmation", "sound\\Male\\confirmation_2_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("confirmation", "sound\\Male\\confirmation_3_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("confirmation", "sound\\Male\\confirmation_4_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("confirmation", "sound\\Male\\confirmation_5_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("confirmation", "sound\\Male\\confirmation_6_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("confirmation", "sound\\Male\\confirmation_7_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("confirmation", "sound\\Male\\confirmation_8_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("confirmation", "sound\\Male\\confirmation_9_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("confirmation", "sound\\Male\\confirmation_10_alex", EffectSound_Volume));


            SoundAdd(new SoundItem("refusal", "sound\\Male\\refusal_1_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("refusal", "sound\\Male\\refusal_2_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("refusal", "sound\\Male\\refusal_3_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("refusal", "sound\\Male\\refusal_4_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("refusal", "sound\\Male\\refusal_5_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("refusal", "sound\\Male\\refusal_6_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("refusal", "sound\\Male\\refusal_7_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("refusal", "sound\\Male\\refusal_8_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("refusal", "sound\\Male\\refusal_9_alex", EffectSound_Volume));
            SoundAdd(new SoundItem("refusal", "sound\\Male\\refusal_10_alex", EffectSound_Volume));



        }


        private static void PlayBackgroundMusic()
        {
            if (BkgSound != null)
            {
                BkgSound.instance.Volume = BkgSound.volume;
                BkgSound.instance.IsLooped = true;
                BkgSound.instance.Play();
            }
            else
            {
                throw new Exception("bkgmusic not set");
            }
        }

        //public static void OptionMenuSoundVolume()
        //{
        //    if (param is obj_tofloat obj)
        //    {
        //        ChagneBackgroundVolume(obj.first);
        //        ChangeEffectSoundVolume(obj.second);
        //    }

        //}

        public static void ChagneBackgroundVolume(float val)
        {
            val = FlatUtil.Clamp(val, 0f, 1f);
            if (BkgSound != null)
            {
                BkgSound.volume = val;
                BkgSound.instance.Volume = val;
            }
        }

        public static void ChangeEffectSoundVolume(float val)
        {
            val = FlatUtil.Clamp(val, 0f, 1f);
            EffectSound_Volume = val;
            if (EffectSound != null)
            {
                //      EffectSound.volume = val ;
                EffectSound.instance.Volume = val * EffectSound.volume;
            }

        }


        private static void PlayEffectMusic()
        {
            if (EffectSound != null)
            {
                EffectSound.instance.Volume = EffectSound_Volume;
                EffectSound.instance.Play();
            }
            else
            {
                throw new Exception("EffectSound not set");
            }
        }

        public static void BkgMusicChange( string name, string path, float volume)
        {
            BkgSound = new SoundItem( name, path, volume);
            PlayBackgroundMusic();
        }

        private static void SoundChange(SoundItem item)
        {
            EffectSound = new SoundItem(item);
            PlayEffectMusic();
        }

        public static void SoundAdd(SoundItem item)
        {
            Sounds.Add(item);
        }

        public static bool SoundChange(string name)
        {
            for (int i = 0; i < Sounds.Count; i++)
            {
                if (Sounds[i].name.Equals(name))
                {
                    SoundChange(Sounds[i]);
                    return true;
                }
            }
            return false;
        }

        public static bool SoundChange(string name,int idx)
        {
     

            for (int i = 0; i < Sounds.Count; i++)
            {
                if (Sounds[i].name.Equals(name))
                {
                    idx--;

                    if (idx == 0)
                    {

                        SoundChange(Sounds[i]);
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
