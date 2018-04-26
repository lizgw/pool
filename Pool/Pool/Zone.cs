using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Pool
{
    class Zone
    {
        Rectangle bounds;
        Player player;
        bool display_zones;
        int timer;
        ContentManager content;
        Texture2D texture;
        Color color;
        public Zone(Rectangle aBounds, Player aPlayerIndex, IServiceProvider _serviceProvider)
        {
            content = new ContentManager(_serviceProvider, "Content");//initializing the content manager
            bounds = aBounds;
            player = aPlayerIndex;
            display_zones = true;
            texture = this.content.Load<Texture2D>("zone");
            color = player.color;
            
        }

        public void Update(GameTime gameTime)
        {
            if (display_zones)
            {
                timer = (timer + 1) % 301;
                if (timer>=300)
                {
                    display_zones = false;
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (display_zones)
                spriteBatch.Draw(texture,bounds,color);
        }
        public void update_bounds(Rectangle rect)
        {
            bounds = rect;
        }
        public void displayZones()
        {
            display_zones = true;
        }

    }
}
