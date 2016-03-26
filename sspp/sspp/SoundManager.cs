using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace sspp
{
    class SoundManager
    {
        #region Members

        private SoundEffect ball1, ball2, ball3, ball4, ball5, goal, goalPost, whistle;                         // Game env sound effects
        private SoundEffect buttonHover, buttonClick, countLow, countHigh, gameover, menu, pause, splash;       // Menu / Env sound effects
        private SoundEffect P1G1, P1G2, P1G3, P1G4, P1G5, P1J1, P1J2, P1J3, P1T1, P1T2;                         // Player one sound effects
        private SoundEffect P2G1, P2G2, P2G3, P2G4, P2G5, P2J1, P2J2, P2J3, P2T1, P2T2;                         // Player two sound effects
        private List<SoundEffectInstance> soundList = new List<SoundEffectInstance>();                          // List of currently running sounds
        private List<SoundEffect> ballSounds;                                                                   // Lists of sounds for randomizing effects
        private List<SoundEffect> Player1Grunts;
        private List<SoundEffect> Player1Jumps;
        private List<SoundEffect> Player1Taunts;
        private List<SoundEffect> Player2Grunts;
        private List<SoundEffect> Player2Jumps;
        private List<SoundEffect> Player2Taunts;
        private SoundEffectInstance currentInstance;                                                            // Most recent loop ran
        private Random rng;                                                                                     // Random number generator used by all lists

        #endregion

        #region Constructors

        public SoundManager()
        {
            #region Setup Random Sound Lists

            ballSounds = new List<SoundEffect>();
            Player1Grunts = new List<SoundEffect>();
            Player1Jumps = new List<SoundEffect>();
            Player1Taunts = new List<SoundEffect>();
            Player2Grunts = new List<SoundEffect>();
            Player2Jumps = new List<SoundEffect>();
            Player2Taunts = new List<SoundEffect>();

            // Balls sounds
            ballSounds.Add(ball1);
            ballSounds.Add(ball2);
            ballSounds.Add(ball3);
            ballSounds.Add(ball4);
            ballSounds.Add(ball5);

            // Player 1 Grunts
            Player1Grunts.Add(P1G1);
            Player1Grunts.Add(P1G2);
            Player1Grunts.Add(P1G3);
            Player1Grunts.Add(P1G4);
            Player1Grunts.Add(P1G5);

            // Player 1 Jumps
            Player1Jumps.Add(P1J1);
            Player1Jumps.Add(P1J2);
            Player1Jumps.Add(P1J3);

            // Player 1 Taunts
            Player1Taunts.Add(P1T1);
            Player1Taunts.Add(P1T2);

            // Player 2 Grunts
            Player2Grunts.Add(P2G1);
            Player2Grunts.Add(P2G2);
            Player2Grunts.Add(P2G3);
            Player2Grunts.Add(P2G4);
            Player2Grunts.Add(P2G5);

            // Player 2 Jumps
            Player2Jumps.Add(P2J1);
            Player2Jumps.Add(P2J2);
            Player2Jumps.Add(P2J3);

            // Player 2 Taunts
            Player2Taunts.Add(P2T1);
            Player2Taunts.Add(P2T2);

            #endregion
        }

        #endregion

        #region Properties

        #endregion

        #region MonoGame Functions

        public void LoadContent(ContentManager Content)
        {
            #region Sound File Loading

            // Load Ball sounds
            ball1 = Content.Load<SoundEffect>("sspp_BallNoise1");
            ball2 = Content.Load<SoundEffect>("sspp_BallNoise2");
            ball3 = Content.Load<SoundEffect>("sspp_BallNoise3");
            ball4 = Content.Load<SoundEffect>("sspp_BallNoise4");
            ball5 = Content.Load<SoundEffect>("sspp_BallNoise5");

            // Load In-game sounds
            goal = Content.Load<SoundEffect>("sspp_Goal");
            goalPost = Content.Load<SoundEffect>("sspp_GoalPost");
            whistle = Content.Load<SoundEffect>("sspp_whistle");

            // Load Menu / Env sounds
            buttonHover = Content.Load<SoundEffect>("sspp_buttonHover");
            buttonClick = Content.Load<SoundEffect>("sspp_buttonClick");
            countLow = Content.Load<SoundEffect>("sspp_countDownLow");
            countHigh = Content.Load<SoundEffect>("sspp_countDownHigh");
            gameover = Content.Load<SoundEffect>("sspp_gameover2");                                          //  or 3 after filename for different options
            menu = Content.Load<SoundEffect>("sspp_menu");
            pause = Content.Load<SoundEffect>("sspp_pause");
            splash = Content.Load<SoundEffect>("sspp_splash");

            // Load Player 1 sounds
            P1G1 = Content.Load<SoundEffect>("sspp_characterSoundsP1G1");
            P1G2 = Content.Load<SoundEffect>("sspp_characterSoundsP1G2");
            P1G3 = Content.Load<SoundEffect>("sspp_characterSoundsP1G3");
            P1G4 = Content.Load<SoundEffect>("sspp_characterSoundsP1G4");
            P1G5 = Content.Load<SoundEffect>("sspp_characterSoundsP1G5");   
            P1J1 = Content.Load<SoundEffect>("sspp_characterSoundsP1J1");
            P1J2 = Content.Load<SoundEffect>("sspp_characterSoundsP1J2");
            P1J3 = Content.Load<SoundEffect>("sspp_characterSoundsP1J3");
            P1T1 = Content.Load<SoundEffect>("sspp_characterSoundsP1T1");
            P1T2 = Content.Load<SoundEffect>("sspp_characterSoundsP1T2");

            // Load Player 2 sounds
            P2G1 = Content.Load<SoundEffect>("sspp_characterSoundsP2G1");
            P2G2 = Content.Load<SoundEffect>("sspp_characterSoundsP2G2");
            P2G3 = Content.Load<SoundEffect>("sspp_characterSoundsP2G3");
            P2G4 = Content.Load<SoundEffect>("sspp_characterSoundsP2G4");
            P2G5 = Content.Load<SoundEffect>("sspp_characterSoundsP2G5");
            P2J1 = Content.Load<SoundEffect>("sspp_characterSoundsP2J1");
            P2J2 = Content.Load<SoundEffect>("sspp_characterSoundsP2J2");
            P2J3 = Content.Load<SoundEffect>("sspp_characterSoundsP2J3");
            P2T1 = Content.Load<SoundEffect>("sspp_characterSoundsP2T1");
            P2T2 = Content.Load<SoundEffect>("sspp_characterSoundsP2T2");

            #endregion
        }

        #endregion

        #region Plays

        public void playBall()                                                                              // Randomizes a ball noise and plays it
        {
            int ri = rng.Next(1, 5);
            SoundEffectInstance instance = ballSounds[ri - 1].CreateInstance();
            instance.IsLooped = false;
            instance.Play();
        }

        public void playGoal()                                                                              // Randomizes a ball noise and plays it
        {
            SoundEffectInstance instance = goal.CreateInstance();
            instance.IsLooped = false;
            instance.Play();
        }

        public void playGoalPost()                                                                              // Randomizes a ball noise and plays it
        {
            SoundEffectInstance instance = goalPost.CreateInstance();
            instance.IsLooped = false;
            instance.Play();
        }

        public void playWhistle()                                                                              // Randomizes a ball noise and plays it
        {
            SoundEffectInstance instance = whistle.CreateInstance();
            instance.IsLooped = false;
            instance.Play();
        }

        public void playCountLow()                                                                              // Randomizes a ball noise and plays it
        {
            SoundEffectInstance instance = countLow.CreateInstance();
            instance.IsLooped = false;
            instance.Play();
        }

        public void playCountHigh()                                                                              // Randomizes a ball noise and plays it
        {
            SoundEffectInstance instance = countHigh.CreateInstance();
            instance.IsLooped = false;
            instance.Play();
        }

        public void playSplash()                                                                              // Randomizes a ball noise and plays it
        {
            SoundEffectInstance instance = splash.CreateInstance();
            instance.IsLooped = false;
            instance.Play();
        }

        public void playButtonHover()                                                                              // Randomizes a ball noise and plays it
        {
            SoundEffectInstance instance = buttonHover.CreateInstance();
            instance.IsLooped = false;
            instance.Play();
        }

        public void playButtonClick()                                                                              // Randomizes a ball noise and plays it
        {
            SoundEffectInstance instance = buttonClick.CreateInstance();
            instance.IsLooped = false;
            instance.Play();
        }

        public void playJump(int playerNum)                                                                 // Plays random jump sound from specified player (1 or 2)
        {
            int ri = rng.Next(1, 3);
            SoundEffectInstance instance;

            if (playerNum == 1)
            {
                instance = Player1Jumps[ri - 1].CreateInstance();
                instance.IsLooped = false;
                instance.Play();
                return;
            }
            else if (playerNum == 2)
            {
                instance = Player2Jumps[ri - 1].CreateInstance();
                instance.IsLooped = false;
                instance.Play();
            }
        }

        public void playGrunt(int playerNum)                                                                // Plays random grunt sound from specified player (1 or 2)
        {
            int ri = rng.Next(1, 5);
            SoundEffectInstance instance;

            if (playerNum == 1)
            {
                instance = Player1Grunts[ri - 1].CreateInstance();
                instance.IsLooped = false;
                instance.Play();
                return;
            }
            else if (playerNum == 2)
            {
                instance = Player2Grunts[ri - 1].CreateInstance();
                instance.IsLooped = false;
                instance.Play();
            }
        }

        public void playTaunt(int playerNum)                                                                // Plays random grunt sound from specified player (1 or 2)
        {
            int ri = rng.Next(1, 2);
            SoundEffectInstance instance;

            if (playerNum == 1)
            {
                instance = Player1Grunts[ri - 1].CreateInstance();
                instance.IsLooped = false;
                instance.Play();
                return;
            }
            else if (playerNum == 2)
            {
                instance = Player2Grunts[ri - 1].CreateInstance();
                instance.IsLooped = false;
                instance.Play();
            }
        }

        #endregion

        #region Loops

        public void LoopMenu()
        {
            SoundEffectInstance instance;
            instance = menu.CreateInstance();
            instance.IsLooped = true;
            instance.Play();
            currentInstance = instance;
        }

        public void LoopGameOver()
        {
            SoundEffectInstance instance;
            instance = gameover.CreateInstance();
            instance.IsLooped = true;
            instance.Play();
            currentInstance = instance;
        }

        public void LoopPause()
        {
            SoundEffectInstance instance;
            instance = pause.CreateInstance();
            instance.IsLooped = true;
            instance.Play();
            currentInstance = instance;
        }

        #endregion

        #region Helper Functions

        public void Stop()                                                                                      // Stops current loop
        {
            currentInstance.Stop();
        }

        #endregion

    }
}
