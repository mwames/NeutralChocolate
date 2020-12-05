using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

namespace NeutralChocolate
{
    public enum SongName
    {
        None,
        Overworld,
    }

    public class SongManager
    {
        public SongName selected;
        public Dictionary<SongName, Song> songs = new Dictionary<SongName, Song>();

        public void Add(SongName name, Song song)
        {
            songs.Add(name, song);
        }

        public void Play(SongName requested)
        {
            if (selected != requested)
            {
                selected = requested;
                MediaPlayer.Play(songs[requested]);
                MediaPlayer.IsRepeating = true;
            }
        }

        public void Pause()
        {
            MediaPlayer.Pause();
        }

        public void Stop()
        {
            MediaPlayer.Stop();
        }
    }
}
