/********************************************************\
*   Program: SS++
*   Engineers: Nathan VelaBorja, Christian Francisco
*   Date: March 26, 2016 WSU Hackathon 2016
\********************************************************/



using System;

namespace sspp
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}
