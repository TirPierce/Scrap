using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Box2D.XNA;
using System;
using Scrap.UserInterface;
//using Triangulator;
using Microsoft.Xna.Framework.Input;

namespace Scrap.GameWorld
{
    class TerrainAdv
    {
        //add arrays of indices of surfaces. 
        BasicEffect effect;
        VertexDeclaration vertDecl;
        VertexBuffer vertBuffer;
        IndexBuffer indexBuffer;
        int numVertices, numPrimitives;
        Vector2[] sourceVertices = new Vector2[1000];
        Rectangle[] sourceBB = new Rectangle[1000];
        Vector2[] targetVertices;
        int[] sourceIndices;
        RenderTarget2D renderTarget;
        Camera camera;
        Texture2D texture;


        Texture2D selectTex;
        Vector2? selectDraw;
        int editedPoint = -1;

        RasterizerState wireframe = new RasterizerState
        {
            CullMode = CullMode.CullClockwiseFace,
            FillMode = FillMode.WireFrame
        };
        
        Game mGame;
        Vector2 WorldLimits = new Vector2(50000, 400);//world limits should be set by the terrain max. 
        
        public TerrainAdv(Game game)
        {
            mGame = game;
            
            
        }
        public void LoadContent()
        {
            texture = mGame.Content.Load<Texture2D>("soil");
            selectTex = mGame.Content.Load<Texture2D>("rotationPointer");
            camera = (Camera)(mGame.Services.GetService(typeof(ICameraService)));
            GenerateVerts();
            TriangulateVerts();
            CreateGeometry();
            GenerateRects();
            effect = new BasicEffect(mGame.GraphicsDevice);
            effect.World = Matrix.Identity;
           
            effect.Projection = Matrix.CreateOrthographic(

    mGame.GraphicsDevice.Viewport.Width,

    mGame.GraphicsDevice.Viewport.Height,
    .1f, 10f);


            effect.TextureEnabled = true;
            effect.Texture = texture;

            renderTarget = new RenderTarget2D(mGame.GraphicsDevice, mGame.GraphicsDevice.Viewport.Width, mGame.GraphicsDevice.Viewport.Height, true, mGame.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
            
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
        //    //grounDef.userData = this;
        //    //ToDo: UserData
        //    Body groundBody= world.CreateBody(grounDef);

        //    for (int i = 0; i < sourceVertices.Length - 1; i++)
        //    {
        //        groundSegment = new PolygonShape();
        //        groundSegment.SetAsEdge(new Vector2(sourceVertices[i].X , sourceVertices[i].Y ), new Vector2(sourceVertices[i + 1].X, sourceVertices[i + 1].Y ));
        //        //groundBody = world.CreateBody(grounDef);
        //        groundFix.shape = groundSegment;
        //        groundBody.CreateFixture(groundFix);
        //    }

        //}
        
        private void GenerateVerts()
        {

            Random r = new Random();
            int vertSpace = 100;
            for (int x = 0; x < sourceVertices.Length; x++)
            {
                sourceVertices[x] = new Vector2(x * vertSpace+r.Next(-20, 20), 40 + r.Next(4));

            }
            sourceVertices[0] = new Vector2(00f, 100f);
            sourceVertices[sourceVertices.Length - 1] = new Vector2(sourceVertices.Length * vertSpace, 100f);

            sourceVertices[0] = new Vector2(-800, 40);
            sourceVertices[1] = new Vector2(0, 40 );
            sourceVertices[2] = new Vector2(40, 50);
            sourceVertices[3] = new Vector2(60, 60);
            sourceVertices[4] = new Vector2(80, 70);
            sourceVertices[5] = new Vector2(130, 70);
            sourceVertices[6] = new Vector2(230, 60);
            sourceVertices[7] = new Vector2(280, 50);
            sourceVertices[8] = new Vector2(200, 40);
            sourceVertices[9] = new Vector2(130, 40);
            sourceVertices[10] = new Vector2(120, 50);
            sourceVertices[11] = new Vector2(115, 30);
            for (int i = 12; i < 997; i++)
            {
                sourceVertices[i] = new Vector2(120 + ((i-12 ) * 2), 25 + r.Next(2));
            }

            sourceVertices[997] = new Vector2(2200, 30);
            sourceVertices[998] = new Vector2(2200
                , 500);
            sourceVertices[999] = new Vector2(-800, 500);


            Vector2[] holeVertices = new Vector2[]
			{
				new Vector2(200, 40),
				new Vector2(200, 80),
				new Vector2(350, 80),
				new Vector2(350, 40),

			};

            // cut the hole out of the source vertices
            //sourceVertices = Triangulator.Triangulator.CutHoleInShape(sourceVertices, holeVertices);





        }

        private void TriangulateVerts()
        {

            sourceIndices = new int[0];
            //Triangulator.Triangulator.Triangulate(
            //    sourceVertices,
            //    Triangulator.WindingOrder.Clockwise,
            //    out targetVertices,
            //    out sourceIndices);




        }
        private void CreateGeometry()
        {




            //numVertices = targetVertices.Length;
            //numPrimitives = sourceIndices.Length / 3;


            //VertexPositionNormalTexture[] verts = new VertexPositionNormalTexture[targetVertices.Length];
            //for (int i = 0; i < targetVertices.Length; i++)
            //    verts[i] = new VertexPositionNormalTexture(new Vector3(targetVertices[i], 0f), Vector3.Backward,
            //        new Vector2(targetVertices[i].X / 10f, targetVertices[i].Y / 10f));
            //vertBuffer = new VertexBuffer(
            //    mGame.GraphicsDevice,
            //    typeof(VertexPositionNormalTexture),
            //    verts.Length * VertexPositionNormalTexture.VertexDeclaration.VertexStride,
            //    BufferUsage.WriteOnly);
            //vertBuffer.SetData(verts);


            //if (verts.Length < 65535)
            //{
            //    short[] indices = new short[sourceIndices.Length];
            //    for (int i = 0; i < sourceIndices.Length; i++)
            //        indices[i] = (short)sourceIndices[i];
            //    indexBuffer = new IndexBuffer(
            //        mGame.GraphicsDevice,
            //        IndexElementSize.SixteenBits,
            //        indices.Length * sizeof(short),
            //        BufferUsage.WriteOnly);
            //    indexBuffer.SetData(indices);
            //}
            //else
            //{
            //    indexBuffer = new IndexBuffer(
            //        mGame.GraphicsDevice,
            //        IndexElementSize.ThirtyTwoBits,
            //        sourceIndices.Length * sizeof(int),
            //        BufferUsage.WriteOnly);
            //    indexBuffer.SetData(sourceIndices);
            //}

            //vertDecl = VertexPositionNormalTexture.VertexDeclaration;
        }

        public void GenerateTerrain()
        {
            GenerateVerts();
            TriangulateVerts();
            CreateGeometry();
            GenerateRects();
        }
        private void GenerateRects()
        {
            for (int i = 0; i < sourceVertices.Length; i++)
            {
                sourceBB[i] = new Rectangle((int)sourceVertices[i].X-2 , (int)sourceVertices[i].Y-2, 4, 4);
            }
        }
        public void RenderTerrain()
        {
            mGame.GraphicsDevice.SetRenderTarget(renderTarget);
            mGame.GraphicsDevice.Clear(Color.Transparent);



            effect.View = Matrix.CreateLookAt(new Vector3(camera.hubWorld.X, camera.hubWorld.Y, -10), new Vector3(camera.hubWorld.X, camera.hubWorld.Y, 10), Vector3.Down);
            //effect.World = Matrix.CreateScale(camera.Magnification , camera.Magnification , 0f);

            effect.Projection = Matrix.CreateOrthographic(mGame.GraphicsDevice.Viewport.Width/camera.Magnification,
                mGame.GraphicsDevice.Viewport.Height/camera.Magnification,
                .1f, 10f);

            
            //mGame.GraphicsDevice.Clear(Color.Transparent);
            mGame.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            mGame.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            mGame.GraphicsDevice.DepthStencilState = DepthStencilState.Default;


            mGame.GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;

            
            mGame.GraphicsDevice.SetVertexBuffer(vertBuffer);
            mGame.GraphicsDevice.Indices = indexBuffer;


            if ( Keyboard.GetState().IsKeyDown(Keys.W))
            {
                mGame.GraphicsDevice.RasterizerState = wireframe;
            }



            

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                //mGame.GraphicsDevice.DrawIndexedPrimitives(
                //    PrimitiveType.TriangleList,
                //    0,
                //    0,
                //    numVertices,
                //    0,
                //    numPrimitives);

            }
            //System.IO.Stream stream = System.IO.File.OpenWrite("test.jpg");


            mGame.GraphicsDevice.SetRenderTarget(null);
            //renderTarget.SaveAsJpeg(stream, mGame.GraphicsDevice.Viewport.Width, 500);
            //stream.Dispose();
            //DrawDebug();
        }
        public void Update()
        {
            editedPoint = -1;
            if (Keyboard.GetState().IsKeyDown(Keys.N)&&Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                selectDraw = Pick(new Point(Mouse.GetState().X, Mouse.GetState().Y));
                if (selectDraw != null)
                {
                    Vector3 b = mGame.GraphicsDevice.Viewport.Unproject(new Vector3(Mouse.GetState().X, Mouse.GetState().Y, mGame.GraphicsDevice.Viewport.MinDepth), effect.Projection, effect.View, effect.World);
                    sourceVertices[editedPoint] = new Vector2(b.X, b.Y);
                    TriangulateVerts();
                    CreateGeometry();
                    GenerateRects();
                }
            }
            else
            {
                selectDraw = null;
            }
        }
       

        public void Draw(SpriteBatch batch)
        {
            
           


            //batch.Draw((Texture2D)renderTarget, new Rectangle(mGame.GraphicsDevice.Viewport.Width / 2 - 5*(int)camera.Magnification, mGame.GraphicsDevice.Viewport.Height / 2 - 5*(int)camera.Magnification, 10*(int)camera.Magnification, 10*(int)camera.Magnification),
            //    new Rectangle(mGame.GraphicsDevice.Viewport.Width/2-5*(int)camera.Magnification, mGame.GraphicsDevice.Viewport.Height/2-5*(int)camera.Magnification,10*(int)camera.Magnification,10*(int)camera.Magnification), new Color(255, 255, 255, 60));
            batch.Draw((Texture2D)renderTarget, Vector2.Zero, Color.White);
            //batch.Draw((Texture2D)renderTarget, new Vector2(v.X, v.Y + 3), null, Color.White, 0, Vector2.Zero, .02f, SpriteEffects.None, 0);
            if (selectDraw != null)
            {
                Console.WriteLine("point");
                batch.Draw(selectTex,sourceBB[editedPoint], null, Color.White, 0f,Vector2.Zero, SpriteEffects.None, 0f);
            }

        }



        public Vector2? Pick(Point screenPoint)
        {
            Vector3 NearestPoint = Vector3.Zero;
            screenPoint.X = screenPoint.X + mGame.GraphicsDevice.Viewport.X;

            Vector3 nearSource = mGame.GraphicsDevice.Viewport.Unproject(new Vector3(screenPoint.X, screenPoint.Y, mGame.GraphicsDevice.Viewport.MinDepth), effect.Projection, effect.View, effect.World);

            for (int i = 0; i < 25; i++)
            {
                
                if(sourceBB[i].Contains(new Point((int)nearSource.X, (int)nearSource.Y)))
                {
                    editedPoint = i;
                    return new Vector2(sourceBB[i].Center.X, sourceBB[i].Center.Y);
                }
            }   



            return null;
        }



    }
}
