using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace DebugZone
{
    public static class UI
    {
        private static List<IScreenEntity> Widgets { get; set; } = new List<IScreenEntity>();
        private static List<IScreenEntity> toDelete { get; set; } = new List<IScreenEntity>();
        public static void Add(IScreenEntity widget)
        {
            Widgets.Add(widget);
        }
        public static void Delete(IScreenEntity widget)
        {
            toDelete.Add(widget);
        }

        public static void Update()
        {
            // Remove Deleted
            foreach (IScreenEntity widget in toDelete)
            {
                Widgets.Remove(widget);
            }

            // Update Remaining
            foreach (IScreenEntity widget in Widgets)
            {
                widget.Update();
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (IScreenEntity widget in Widgets)
            {
                widget.Draw(spriteBatch);
            }
        }
    }
}
