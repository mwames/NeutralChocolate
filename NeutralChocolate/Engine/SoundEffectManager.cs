using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace NeutralChocolate
{
    public enum SoundEffectName
    {
        None,
        Blip,
    }
        

    public class SoundEffectManager
    {
        public Dictionary<SoundEffectName, SoundEffect> soundEffects = new Dictionary<SoundEffectName, SoundEffect>();

        public void Add(SoundEffectName name, SoundEffect soundEffect)
        {
            soundEffects.Add(name, soundEffect);
        }

        public SoundEffect Get(SoundEffectName name)
        {
            return soundEffects[name];
        }
    }
}
