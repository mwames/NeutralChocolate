using System;

namespace NeutralChocolate
{
    public static class Program
    {
        public static NeutralChocolate Game;
        [STAThread]
        static void Main()
        {
            using (var game = new NeutralChocolate())
            {
                Game = game;
                game.Run();
            }
        }
    }
}
