using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;


namespace NeutralChocolate
{
    public static class Sound
    {
        public static Song Overworld{ get; private set; }
        public static SoundEffect Blip { get; private set;}

         public static void Load(ContentManager content)
        {
        Overworld = content.Load<Song>("Sounds/nature");
        Blip = content.Load<SoundEffect>("Sounds/Blip");
        }
    }
}