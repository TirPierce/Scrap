
using System;

using Triangulator;
using Microsoft.Xna.Framework.Input;
using Scrap;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Scrap.GameElements.Level
{
    class Level
    {
        //add arrays of indices of surfaces. 

        Vector2[] sourceVertices = new Vector2[10];




        Game game;
        Vector2 WorldLimits = new Vector2(50000, 400);//world limits should be set by the terrain max. 

        public Level(Game game)
        {
            this.game = game;
            
            
        }
        public void LoadContent()
        {
            
        }
        public void CreateGround(World world)
        {

            GenerateVerts();
            List<Vertices> vertList = new List<Vertices>();
            vertList.Add(new Vertices(sourceVertices));
            //Body groundBody = BodyFactory.CreateCompoundPolygon(world, vertList, 1f);
            Body groundBody = BodyFactory.CreateEdge(world, new Vector2(-200, 20), new Vector2(200,20));


        }
        
        private void GenerateVerts()
        {
            sourceVertices[0] = new Vector2(-30, 100);
            sourceVertices[1] = new Vector2(0, 40 );
            sourceVertices[2] = new Vector2(40, 50);
            sourceVertices[3] = new Vector2(60, 60);
            sourceVertices[4] = new Vector2(80, 70);
            sourceVertices[5] = new Vector2(130, 70);
            sourceVertices[6] = new Vector2(230, 60);
            sourceVertices[7] = new Vector2(280, 50);
            sourceVertices[8] = new Vector2(200, 40);
            sourceVertices[9] = new Vector2(130, 100);





        }



        
    }
}
