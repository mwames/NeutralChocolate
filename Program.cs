using System;

namespace NeutralChocolate
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new NeutralChocolate())
                game.Run();
        }
    }
}
