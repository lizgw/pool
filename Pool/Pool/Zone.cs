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

        public Zone(IServiceProvider _serviceProvider, Rectangle aBounds, Player aPlayerIndex)
        {
            content = new ContentManager(_serviceProvider, "Content");//initializing the content manager
            bounds = aBounds;
            player = aPlayerIndex;
            display_zones = true;
            texture = content.Load<Texture2D>("zone");
            color = player.GetColor();            
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
                spriteBatch.Draw(texture, bounds, color * 0.65f);
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
