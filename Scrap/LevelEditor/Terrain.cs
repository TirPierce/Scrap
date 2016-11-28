using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using System;
using System.IO;
using Triangulator;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;
using Newtonsoft.Json;

namespace LevelEditor
{
    class MapFile
    {
        public Vector2[] terrainVertices;// = new Vector2[14];
    }
    class Terrain
    {

        BasicEffect effect;
        VertexDeclaration vertDecl;
        DynamicVertexBuffer vertBuffer;
        DynamicIndexBuffer indexBuffer;
        int numVertices, numPrimitives;
        MapFile mapfile = new MapFile();

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

        LevelEditor game;
        Vector2 WorldLimits = new Vector2(50000, 400);//world limits should be set by the terrain max. 

        public Terrain(LevelEditor game)
        {
            this.game = game;
        }
        public void LoadContent()
        {
            texture = game.Content.Load<Texture2D>("soil");
            this.camera = ((LevelEditor)game).camera;
            
            effect = new BasicEffect(game.GraphicsDevice);
            effect.TextureEnabled = true;
            effect.Texture = texture;
            renderTarget = new RenderTarget2D(game.GraphicsDevice, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height, false, SurfaceFormat.RgbPvrtc4Bpp, DepthFormat.Depth24Stencil8); //game.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);

        }
        public void CreateGround(World world)
        {
            GenerateVerts();
            TriangulateVerts();
            CreateGeometry();
            List<Vertices> vertList = FarseerPhysics.Common.Decomposition.Triangulate.ConvexPartition(new Vertices(mapfile.terrainVertices), FarseerPhysics.Common.Decomposition.TriangulationAlgorithm.Bayazit);
            Body groundBody = BodyFactory.CreateCompoundPolygon(world, vertList, 1f, this);
            groundBody.BodyType = BodyType.Static;
            groundBody.IgnoreGravity = true;
            groundBody.Restitution = .5f;
            groundBody.Friction = .9f;
        }

        public void Save()
        {
            string imagesDirectory = System.IO.Path.Combine(Environment.CurrentDirectory,"");
            string json = JsonConvert.SerializeObject(mapfile.terrainVertices, Formatting.Indented);
            using (StreamWriter file = File.CreateText(imagesDirectory+"/maps.json"))
                {
                   JsonSerializer serializer = new JsonSerializer();
                   serializer.Serialize(file, mapfile);
                }
            Console.WriteLine(json);
        }
        public void Load()
        {
            string imagesDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, "");
            string json = System.IO.File.ReadAllText(imagesDirectory + "/maps.json");
            mapfile = JsonConvert.DeserializeObject<MapFile>(json);
        }
        private void GenerateVerts()
        {
            Load();

            //mapfile.terrainVertices[0] = new Vector2(0f, 50f);
            //mapfile.terrainVertices[1] = new Vector2(0f, 100f);
            //mapfile.terrainVertices[2] = new Vector2(500f, 100f);
            //mapfile.terrainVertices[3] = new Vector2(500f, 00f);
            //mapfile.terrainVertices[4] = new Vector2(250f, 00f);
            //mapfile.terrainVertices[5] = new Vector2(250f, 20f);
            //mapfile.terrainVertices[6] = new Vector2(420f, 30f);
            //mapfile.terrainVertices[7] = new Vector2(420f, 60f);
            //mapfile.terrainVertices[8] = new Vector2(200f, 60f);

            //Save();
        }

        private void TriangulateVerts()
        {

            sourceIndices = new int[0];
            Triangulator.Triangulator.Triangulate(
                mapfile.terrainVertices,
                Triangulator.WindingOrder.CounterClockwise,
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
                    new Vector2(targetVertices[i].X / 10f, targetVertices[i].Y / 10f));
            vertBuffer = new DynamicVertexBuffer(
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
                indexBuffer = new DynamicIndexBuffer(
                    game.GraphicsDevice,
                    IndexElementSize.SixteenBits,
                    indices.Length * sizeof(short),
                    BufferUsage.WriteOnly);
                indexBuffer.SetData(indices);
            }
            else
            {
                indexBuffer = new DynamicIndexBuffer(
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
            effect.View = camera.Transformation;
            effect.World = Matrix.Identity;
            effect.Projection = camera.Projection;

            game.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
            game.GraphicsDevice.BlendState = BlendState.Opaque;
            

            game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
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
