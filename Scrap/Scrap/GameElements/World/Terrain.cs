
//using System;

//using Triangulator;
//using Microsoft.Xna.Framework.Input;
//using Scrap;
//using FarseerPhysics.Dynamics;
//using FarseerPhysics.Collision.Shapes;
//using FarseerPhysics.Factories;
//using FarseerPhysics.Common;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework;
//using System.Collections.Generic;

//namespace Scrap.GameElements.Level
//{
//    class Level
//    {
//        //add arrays of indices of surfaces. 

//        Vector2[] sourceVertices = new Vector2[1000];




//        Game game;
//        Vector2 WorldLimits = new Vector2(50000, 400);//world limits should be set by the terrain max. 

//        public Level(Game game)
//        {
//            this.game = game;


//        }
//        public void LoadContent()
//        {

//        }
//        public void CreateGround(World world)
//        {

//            GenerateVerts();
//            List<Vertices> vertList = new List<Vertices>();
//            vertList.Add(new Vertices(sourceVertices));
//            Body groundBody = BodyFactory.CreateCompoundPolygon(world, vertList, 1f);
//            //Body groundBody = BodyFactory.CreateEdge(world, new Vector2(-200, 20), new Vector2(200,20));
//            groundBody.BodyType = BodyType.Static;
//            groundBody.IgnoreGravity = true;


//        }
//        private void GenerateVerts()
//        {

//            Random r = new Random();
//            int vertSpace = 100;
//            for (int x = 0; x < sourceVertices.Length; x++)
//            {
//                sourceVertices[x] = new Vector2(x * vertSpace + r.Next(-20, 20), 40 + r.Next(4));

//            }
//            sourceVertices[0] = new Vector2(00f, 100f);
//            sourceVertices[sourceVertices.Length - 1] = new Vector2(sourceVertices.Length * vertSpace, 100f);

//            sourceVertices[0] = new Vector2(-800, 40);
//            sourceVertices[1] = new Vector2(0, 40);
//            sourceVertices[2] = new Vector2(40, 50);
//            sourceVertices[3] = new Vector2(60, 60);
//            sourceVertices[4] = new Vector2(80, 70);
//            sourceVertices[5] = new Vector2(130, 70);
//            sourceVertices[6] = new Vector2(230, 60);
//            sourceVertices[7] = new Vector2(280, 50);
//            sourceVertices[8] = new Vector2(200, 40);
//            sourceVertices[9] = new Vector2(130, 40);
//            sourceVertices[10] = new Vector2(120, 50);
//            sourceVertices[11] = new Vector2(115, 30);
//            for (int i = 12; i < 997; i++)
//            {
//                sourceVertices[i] = new Vector2(120 + ((i - 12) * 2), 25 + r.Next(2));
//            }

//            sourceVertices[997] = new Vector2(2200, 30);
//            sourceVertices[998] = new Vector2(2200
//                , 500);
//            sourceVertices[999] = new Vector2(-800, 500);




//        }





//    }
//}



using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using System;

using Triangulator;
using Microsoft.Xna.Framework.Input;
using Scrap;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;

namespace GameElements.GameWorld
{
    class Terrain
    {

        BasicEffect effect;
        VertexDeclaration vertDecl;
        VertexBuffer vertBuffer;
        IndexBuffer indexBuffer;
        int numVertices, numPrimitives;
        Vector2[] sourceVertices = new Vector2[6];

        Vector2[] targetVertices;
        int[] sourceIndices;
        RenderTarget2D renderTarget;
        Camera camera;
        Texture2D texture;




        RasterizerState wireframe = new RasterizerState
        {
            CullMode = CullMode.None,
            FillMode = FillMode.WireFrame
        };

        ScrapGame game;
        Vector2 WorldLimits = new Vector2(50000, 400);//world limits should be set by the terrain max. 

        public Terrain(ScrapGame game)
        {
            this.game = game;
        }
        public void LoadContent()
        {
            texture = game.Content.Load<Texture2D>("soil");
            this.camera = ((ScrapGame)game).camera;
            
            effect = new BasicEffect(game.GraphicsDevice);
            effect.TextureEnabled = true;
            effect.Texture = texture;
            renderTarget = new RenderTarget2D(game.GraphicsDevice, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height, true, game.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);

        }
        public void CreateGround(World world)
        {
            GenerateVerts();
            TriangulateVerts();
            CreateGeometry();
            List<Vertices> vertList = FarseerPhysics.Common.Decomposition.Triangulate.ConvexPartition(new Vertices(sourceVertices), FarseerPhysics.Common.Decomposition.TriangulationAlgorithm.Bayazit);
            Body groundBody = BodyFactory.CreateCompoundPolygon(world, vertList, 1f, null);
            groundBody.BodyType = BodyType.Static;
            groundBody.IgnoreGravity = true;

        }


        private void GenerateVerts()
        {


            sourceVertices[0] = new Vector2(0f, 50f);
            sourceVertices[1] = new Vector2(0f, 100f);
            sourceVertices[2] = new Vector2(500f, 100f);
            sourceVertices[3] = new Vector2(500f, 50f);
            sourceVertices[4] = new Vector2(400f, 20f);
            sourceVertices[5] = new Vector2(200f, 70f);






        }

        private void TriangulateVerts()
        {

            sourceIndices = new int[0];
            Triangulator.Triangulator.Triangulate(
                sourceVertices,
                Triangulator.WindingOrder.Clockwise,
                out targetVertices,
                out sourceIndices);




        }
        private void CreateGeometry()
        {




            numVertices = targetVertices.Length;
            numPrimitives = sourceIndices.Length / 3;


            VertexPositionNormalTexture[] verts = new VertexPositionNormalTexture[targetVertices.Length];
            for (int i = 0; i < targetVertices.Length; i++)
                verts[i] = new VertexPositionNormalTexture(new Vector3(targetVertices[i], 0f), Vector3.Backward,
                    new Vector2(targetVertices[i].X / 100f, targetVertices[i].Y / 100f));
            vertBuffer = new VertexBuffer(
                game.GraphicsDevice,
                typeof(VertexPositionNormalTexture),
                verts.Length * VertexPositionNormalTexture.VertexDeclaration.VertexStride,
                BufferUsage.WriteOnly);
            vertBuffer.SetData(verts);


            if (verts.Length < 65535)
            {
                short[] indices = new short[sourceIndices.Length];
                for (int i = 0; i < sourceIndices.Length; i++)
                    indices[i] = (short)sourceIndices[i];
                indexBuffer = new IndexBuffer(
                    game.GraphicsDevice,
                    IndexElementSize.SixteenBits,
                    indices.Length * sizeof(short),
                    BufferUsage.WriteOnly);
                indexBuffer.SetData(indices);
            }
            else
            {
                indexBuffer = new IndexBuffer(
                    game.GraphicsDevice,
                    IndexElementSize.ThirtyTwoBits,
                    sourceIndices.Length * sizeof(int),
                    BufferUsage.WriteOnly);
                indexBuffer.SetData(sourceIndices);
            }

            vertDecl = VertexPositionNormalTexture.VertexDeclaration;
        }



        public void RenderTerrain()
        {
            game.GraphicsDevice.SetRenderTarget(renderTarget);
            game.GraphicsDevice.Clear(Color.Transparent);
            effect.View = Matrix.Identity;//camera.Transformation;
            effect.World = Matrix.Identity;
            effect.Projection = camera.Projection;

            game.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            game.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            game.GraphicsDevice.SetVertexBuffer(vertBuffer);
            game.GraphicsDevice.Indices = indexBuffer;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                game.GraphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    0,
                    0,
                    numVertices,
                    0,
                    numPrimitives);

            }
            game.GraphicsDevice.SetRenderTarget(null);

        }



        public void Draw(SpriteBatch batch)
        {
            batch.Draw((Texture2D)renderTarget, Vector2.Zero, Color.White);
        }

    }
}
