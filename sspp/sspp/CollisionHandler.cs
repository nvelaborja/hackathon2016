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
    class CollisionHandler
    {
        Player player1;
        Player player2;
        Ball ball;
        private bool playerCollide;
        private bool ballCollide;
        SoundManager sounds;

        #region Constructors

        public CollisionHandler(ref Player player1, ref Player player2, ref Ball ball)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.ball = ball;

            Initialize();
        }

        #endregion

        #region MonoGame Functions

        protected void Initialize()
        {
            playerCollide = false;
            ballCollide = false;
            sounds = new SoundManager();
            
        }

        public void LoadContent(ContentManager Content)
        {
            sounds.LoadContent(Content);
        }

        public void Update(GameTime gameTime)
        {
            if (playerCollide)
            {
                if (DistanceFormulaVector2(player1.Center, player2.Center) > player1.Radius + player2.Radius)
                {
                    playerCollide = false;
                }
            }
            else if (DistanceFormulaVector2(player1.Center, player2.Center) < player1.Radius + player2.Radius)
            {
                playerCollide = true;
                if (player1.Physics.VelocityX > player2.Physics.VelocityX)
                    sounds.playGrunt(2);
                else sounds.playGrunt(1);
            }

            if (ballCollide)
            {
                if ((DistanceFormulaVector2(player1.Center, ball.Center) > player1.Radius + ball.Radius) && (DistanceFormulaVector2(player2.Center, ball.Center) > player2.Radius + ball.Radius))
                {
                    ballCollide = false;
                }
            }
            else if ((DistanceFormulaVector2(player1.Center, ball.Center) < player1.Radius + ball.Radius) || (DistanceFormulaVector2(player2.Center, ball.Center) < player2.Radius + ball.Radius))
            {
                ballCollide = true;
                sounds.playBall();
                if (DistanceFormulaVector2(player1.Center, ball.Center) < player1.Radius + ball.Radius)
                    BallHit(player1);
                else BallHit(player2);
            }
        }

        #endregion

        #region Gameplay Functions

        private void BallHit(Player hitter)
        {
            ball.AcceptForce(hitter.physics.ApplyForce());
        }

        #endregion

        #region Helper Functions

        private int DistanceFormulaVector2(Vector2 point1, Vector2 point2)
        {
            int distance = 0;

            distance = (int) Math.Sqrt(((Math.Pow(MathHelper.Distance(point2.X, point1.X), 2) + Math.Pow(MathHelper.Distance(point2.Y, point1.Y), 2))));

            return distance;
        }

        #endregion
    }
}
