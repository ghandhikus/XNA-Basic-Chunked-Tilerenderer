using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileRendering
{
    struct Chunk
    {
        bool visible;
        bool rendered;
        public bool isVisible() { return visible; }
        int fbo;
        Vector2 position;
        static int tileSize = MapManager.getInstance().getTileSize();
        static int chunkSize = MapManager.getInstance().getChunkSize();

        static SpriteBatch batch = new SpriteBatch(Game1.graphics.GraphicsDevice);

        public Chunk(Vector2 tilePosition)
        {
            position = tilePosition;
            visible = false;
            rendered = false;
            fbo = -1;
        }

        public void Draw(SpriteBatch batch, Vector2 offset)
        {
            batch.Draw(MapManager.getInstance().getFBO(fbo), new Vector2((int)Math.Round(position.X * tileSize - offset.X), (int)Math.Round(position.Y * tileSize - offset.Y)), null);
        }

        public void Blur()
        {
            visible = false;
            rendered = false;
            if (fbo != -1)
                MapManager.getInstance().setFBOUsed(fbo, false);
            fbo = -1;
        }

        public void Focus()
        {
            visible = true;
            PreCheck();
        }

        public void PreCheck()
        {
            if (rendered == false && visible == true)
            {
                rendered = true;
                batch.End();
                fbo = MapManager.getInstance().getFreeFBO();
                MapManager.getInstance().setFBOUsed(fbo, true);

                Game1.graphics.GraphicsDevice.SetRenderTarget(MapManager.getInstance().getFBO(fbo));
                batch.Begin();

                for (int x = 0; x < chunkSize / tileSize; x++)
                    for (int y = 0; y < chunkSize / tileSize; y++)
                    {
                        int tile = MapManager.getInstance().tilemap[(int)(x + position.X), (int)(y + position.Y)];
                        switch (tile)
                        {
                            case 1:
                                batch.Draw(Game1.tile, new Vector2(x * tileSize, y * tileSize), Color.DarkGray);
                                break;
                            default:
                                batch.Draw(Game1.tile, new Vector2(x * tileSize, y * tileSize), Color.White);
                                break;
                        }
                    }

                batch.End();
                Game1.graphics.GraphicsDevice.SetRenderTarget(null);
                batch.Begin();
            }
        }
    }
}
