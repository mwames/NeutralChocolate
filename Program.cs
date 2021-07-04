using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NeutralChocolate
{
    public static class Program
    {
        public static Game Game;
        [STAThread]
        static void Main(string[] args)
        {                
            using (var game = (Game)new DebugZone.DebugZone())
            {
                Game = game;
                game.Run();
            }
        }
    }
}
