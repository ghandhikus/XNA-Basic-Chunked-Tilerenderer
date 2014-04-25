using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileRendering
{
    class MapManager
    {
        public byte[,] tilemap;

        RenderTarget2D[] fbo;

        bool[] fboUsed;

        Chunk[,] chunks;
        int screenWidth, screenHeight;

        int fboCount;
        int tileSize = 16;
        int chunkSize = 256;

        public int getTileSize() { return tileSize; }
        public int getChunkSize() { return chunkSize; }

        static MapManager instance;

        public static MapManager getInstance() { return instance; }

        public void setFBOUsed(int fbo, bool used) { fboUsed[fbo] = used; }

        public MapManager(int width, int height, int screenWidth, int screenHeight)
        {
            instance = this;

            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            tilemap = new byte[width * chunkSize, height * chunkSize];
            Random rnd = new Random();
            for (int x = 0; x < width * chunkSize; x++)
            {
                for (int y = 0; y < height * chunkSize; y++)
                {
                    if (((int)rnd.Next(1, 5)) == 1) tilemap[x, y] = 1;
                    else tilemap[x, y] = 0;
                }
            }

            chunks = new Chunk[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    chunks[x, y] = new Chunk(new Vector2(x * chunkSize / tileSize, y * chunkSize / tileSize));
                }
            }


            fboCount = (int)(Math.Ceiling((double)((screenHeight / chunkSize) + 2) * ((screenWidth / chunkSize) + 2)));
            Console.WriteLine("FBO count : " + fboCount);

            
            fbo = new RenderTarget2D[fboCount];
            fboUsed = new bool[fboCount];
            for (int i = 0; i < fboCount; i++)
            {
                fbo[i] = new RenderTarget2D(Game1.graphics.GraphicsDevice, chunkSize, chunkSize);

                fboUsed[i] = false;
            }
        }

        public void Draw(SpriteBatch batch, Vector2 position)
        {
            batch.Begin();
            for (int x = (int)Math.Floor(position.X / chunkSize); x < (int)Math.Ceiling((position.X + screenWidth) / chunkSize); x++)
            {
                for (int y = (int)Math.Floor(position.Y / chunkSize); y < (int)Math.Ceiling((position.Y + screenHeight) / chunkSize); y++)
                {
                    if (x < 0 || y < 0) continue;
                    if (x > chunks.GetLength(0) - 1 || y > chunks.GetLength(1) - 1) continue;

                    chunks[x, y].Draw(batch, position);
                }
            }
            batch.End();
        }

        public void Update(Vector2 position)
        {
            for (int x = 0; x < chunks.GetLength(0); x++)
            {
                for (int y = 0; y < chunks.GetLength(1); y++)
                {
                    if (x >= (int)Math.Floor(position.X / chunkSize) && x <= (int)Math.Ceiling((position.X + screenWidth) / chunkSize))
                        if (y >= (int)Math.Floor(position.Y / chunkSize) && y <= (int)Math.Ceiling((position.Y + screenHeight) / chunkSize))
                    {
                        continue;
                    }

                    chunks[x, y].Blur();
                }
            }


            for (int x = (int)Math.Floor(position.X / chunkSize); x < (int)Math.Ceiling((position.X + screenWidth) / chunkSize); x++)
            {
                for (int y = (int)Math.Floor(position.Y / chunkSize); y < (int)Math.Ceiling((position.Y + screenHeight) / chunkSize); y++)
                {
                    if (x < 0 || y < 0) continue;
                    if (x > chunks.GetLength(0) - 1 || y > chunks.GetLength(1) - 1) continue;

                    chunks[x, y].Focus();
                }
            }

        }

        public int getFreeFBO()
        {
            for (int i = 0; i < fboCount; i++)
            {
                if (fboUsed[i] == false) return i;
            }

            // If not found then recreate fbo array and add new fbo
            RenderTarget2D[] meow = new RenderTarget2D[fboCount + 1];
            meow[fboCount] = new RenderTarget2D(Game1.graphics.GraphicsDevice, chunkSize, chunkSize);
            for (int i = 0; i < fboCount; i++)
            {
                meow[i] = fbo[i];
            }
            fbo = meow;

            bool[] nyaa = new bool[fboCount + 1];
            for (int i = 0; i < fboCount; i++)
            {
                nyaa[i] = fboUsed[i];
            }
            nyaa[fboCount] = false;
            fboUsed = nyaa;

            fboCount = fboCount + 1;
            Console.WriteLine("New FBO count : " + fboCount);

            return fboCount - 1;
        }

        public RenderTarget2D getFBO(int fbo) { return this.fbo[fbo]; }

        public void Dispose()
        {
            for (int i = 0; i < fboCount; i++)
            {
                fbo[i].Dispose();
            }
        }
    }
}
