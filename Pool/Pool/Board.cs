using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Pool
{
    class Board
    {
        Player[] players;
        List<Ball> balls;
        //Physics physics;
        GUI gui;
        List<Zone> zones;

        double friction;
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;//creating content manager
        public Texture2D ball;//ball sprite holder

        public Board(int numPlayers, IServiceProvider _serviceProvider)
        {
            content = new ContentManager(_serviceProvider, "Content");//initializing the content manager
            ball = this.content.Load<Texture2D>("ball");// loading the ball sprite
            players = new Player[numPlayers];
            balls = new List<Ball>();
            //physics = new Physics();
            friction = 0;
            gui = new GUI();
            zones = new List<Zone>();
            for (int i=0; i<2;i++)
            {
                balls.Add(new Ball(ball));
                
            }//initializes the balls
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Ball b in balls)
                b.Draw(spriteBatch);
        }
        
    }
}
