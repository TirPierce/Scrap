using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Box2D.XNA;
using System;
using Scrap.UserInterface;

namespace Scrap.GameWorld
{
    class Terrain
    {
        Game mGame;
        List<Vector2> mTerrainPoints;
        List<Texture2D> mTextures;
        Texture2D mDirtTexture;
        Vector2 WorldLimits = new Vector2(50000, 400);//world limits should be set by the terrain max. 
        Point terrainSpriteSize = new Point(100, 100);


        public Terrain(Game game)
        {
            mGame = game;

            mTerrainPoints = new List<Vector2>();
            mTextures = new List<Texture2D>();

            Random r = new Random();
            for (int x = 0; x < WorldLimits.X; x += 100)
            {
                mTerrainPoints.Add(new Vector2(x, r.Next(10, 80)));
            }

        }
        public void Update(Camera camera)
        {
            lowX = (int)camera.Position.X-15;
            highX =(int) camera.Position.X + 15;

        }
        int lowX = 0;
        int highX = 0;
        Vector2 v = Vector2.Zero;
        public void Draw(SpriteBatch batch)
        {

            for (int i = lowX < 0? 0: lowX; (i < highX && i < mTextures.Count) ; i++)
            {
                v = new Vector2(terrainSpriteSize.X * i * .01f, 20);
                batch.Draw(mTextures[i], v, null, Color.White, 0, Vector2.Zero, .01f, SpriteEffects.None, 0);
                batch.Draw(mDirtTexture, new Vector2(v.X, v.Y +1), null, Color.White, 0, Vector2.Zero, .02f, SpriteEffects.None, 0);
                batch.Draw(mDirtTexture, new Vector2(v.X, v.Y + 3), null, Color.White, 0, Vector2.Zero, .02f, SpriteEffects.None, 0);
                batch.Draw(mDirtTexture, new Vector2(v.X, v.Y + 5), null, Color.White, 0, Vector2.Zero, .02f, SpriteEffects.None, 0);
            }
        }
        private float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public bool TriangleInPoint(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
        {//No terrain class would be complete without some barryCentric wizardry... or bad spelling. 

            // Compute vectors        
            Vector2 v0 = C - A;
            Vector2 v1 = B - A;
            Vector2 v2 = P - A;

            // Compute dot products

            float dot00 = Vector2.Dot(v0, v0);
            float dot01 = Vector2.Dot(v0, v1);
            float dot02 = Vector2.Dot(v0, v2);
            float dot11 = Vector2.Dot(v1, v1);
            float dot12 = Vector2.Dot(v1, v2);

            // Compute barycentric coordinates
            float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
            float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            // Check if point is in triangle
            return (u > 0) && (v > 0) && (u + v < 1);

        }
        public void LoadContent()
        {
            mDirtTexture = mGame.Content.Load<Texture2D>("dirt");
            SetSpriteListAlpha(mDirtTexture, mTextures, mTerrainPoints, false);
        }

        //public void CreateGround(World world)
        //{
        //    var grounDef = new BodyDef();
        //    var groundFix = new FixtureDef();
        //    grounDef.type = BodyType.Static;
        //    groundFix.restitution = .1f;
        //    groundFix.friction = 0.7f;
        //    groundFix.density = 0.0f;
        //    PolygonShape groundSegment;
        //    Body groundBody;
        //    for (int i = 0; i < mTerrainPoints.Count - 1; i++)
        //    {
        //        groundSegment = new PolygonShape();
        //        groundSegment.SetAsEdge(new Vector2(mTerrainPoints[i].X * .01f, mTerrainPoints[i].Y * .01f + 20), new Vector2(mTerrainPoints[i + 1].X * .01f, mTerrainPoints[i + 1].Y * .01f + 20));
        //        groundBody = world.CreateBody(grounDef);
        //        groundFix.shape = groundSegment;
        //        groundBody.CreateFixture(groundFix);
        //    }
        //}
        private int HeightAtPoint(Vector2 first, Vector2 second, int xIncrement)
        { //y - y1 = m(x - x1)
            if (xIncrement < first.X || xIncrement > second.X)
            {
                return 0;
            }


            return (int)(((second.Y - first.Y)
                / (second.X - first.X))
                * ((float)xIncrement - first.X)
                + first.Y);
        }

        private void SetSpriteListAlpha(Texture2D texture, List<Texture2D> textureList, List<Vector2> pointList, bool above)
        {
            int terrainX = 0;
            int textureIndex = 0;
            int pointIndex = 0;

            while (terrainX < WorldLimits.X - terrainSpriteSize.X && pointIndex < pointList.Count - 1)
            {
                textureList.Add(ProcessSprite(texture, pointList, ref terrainX, ref pointIndex, above));
                textureIndex++;

            }

        }
        Color[,] workingTexture;
        private Texture2D ProcessSprite(Texture2D texture, List<Vector2> pointList, ref int terrainX, ref int pointIndex, bool above)
        {
            workingTexture = TextureTo2DArray(texture);

            int range = terrainX + terrainSpriteSize.X;
            for (int x = 0; terrainX < range && terrainX < WorldLimits.X; x++, terrainX++)//once for each sprite
            {
                // if (pointList.Count == pointIndex + 1)
                //  return mDirtTexture;
                if (pointList[pointIndex + 1].X <= terrainX)
                {
                    pointIndex++;
                    x = 0;
                }
                if (pointList.Count == pointIndex + 1)
                    return mDirtTexture;
                for (int y = HeightAtPoint(pointList[pointIndex], pointList[pointIndex + 1], terrainX); y >= 0; y--)
                {//each column of pixels gets clear
                    workingTexture[x, y] = Color.Transparent;
                }
            }


            return ArrayToTexture(workingTexture);
        }
        Color[] colors1D;
        Color[,] colors2D;//declared outside the loop. Not sure just how much .NET will do for me. :P
        private Color[,] TextureTo2DArray(Texture2D texture)
        {
            colors1D = new Color[terrainSpriteSize.X * terrainSpriteSize.Y];
            
            texture.GetData(colors1D);
            colors2D = new Color[terrainSpriteSize.X, terrainSpriteSize.Y];
            for (int x = 0; x < terrainSpriteSize.X; x++)
                for (int y = 0; y < terrainSpriteSize.Y; y++)
                    colors2D[x, y] = colors1D[x + y * terrainSpriteSize.X];
            return colors2D;
        }
        Color[] colorLine;
        Texture2D texture;
        private Texture2D ArrayToTexture(Color[,] colorArray)
        {
            texture = new Texture2D(mGame.GraphicsDevice, terrainSpriteSize.X, terrainSpriteSize.Y);
            colorLine = new Color[terrainSpriteSize.X * terrainSpriteSize.Y];



            for (int x = 0; x < terrainSpriteSize.X; x++)
                for (int y = 0; y < terrainSpriteSize.Y; y++)
                    colorLine[x + y * terrainSpriteSize.Y] = colorArray[x, y];



            texture.SetData(colorLine);
            return texture;
        }
    }
}
