using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scrap.Entities;
using Microsoft.Xna.Framework;
using Scrap.UserInterface;
//using Box2D.XNA;
//using Scrap.Physics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Scrap.GameState
{

    public class Line
    {
        public Entity start;
        public Entity finish;

        public Line(Entity start, Entity finish)
        {
            this.start = start;
            this.finish = finish;
        }
    }
    public class Invention
    {
        //public Body sensors;
        //ToDo: implement variable grid size. Ideally size from center rather than max size, ensuring hub will always be centered. 
        //public int maxGridWidth = 5;
        //public int maxGridHEight = 5;
        Game mGame;
        IGUI gui;
        //IPhysicsService physics;
        Texture2D electricEffect;
        Texture2D greenSenTex;
        Texture2D redSenTex;
        public List<Line> lineList;
        public List<Entity> entityList;
        public List<Entity> worldEntityList;
        public int[,] sensorBlocked = new int[5,5];
        public Entity[,] grid = new Entity[5, 5];

        private bool[,] alreadyChecked = new bool[5, 5];

        private bool CheckConnections(int x, int y)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    alreadyChecked[i, j] = false;
                }
            }
            return CheckSurround(x, y);
        }
        private bool CheckSurround(int x,int y)
        {

            alreadyChecked[x, y] = true;
            if (x > 0 && grid[x-1, y] != null)
            {
                if (grid[x - 1, y].GetType() == typeof(Hub))
                {
                    return true;
                }
                if (!alreadyChecked[x - 1, y])
                {
                    return CheckSurround(x - 1, y);
                }

            }
            if (x < 4 && grid[x + 1, y] != null)
            {
                if (grid[x + 1, y].GetType() == typeof(Hub))
                {
                    return true;
                }
                if (!alreadyChecked[x + 1, y])
                {
                    return CheckSurround(x + 1, y);
                }

            }

            if (y > 0 && grid[x , y-1] != null)
            {
                if (grid[x , y-1].GetType() == typeof(Hub))
                {
                    return true;
                }
                if (!alreadyChecked[x , y-1])
                {
                    return CheckSurround(x, y - 1);
                }
            }
            if (y < 4 && grid[x , y+1] != null)
            {
                if (grid[x , y+1].GetType() == typeof(Hub))
                {
                    return true;
                }
                if (!alreadyChecked[x , y+1])
                {
                    return CheckSurround(x, y + 1);
                }
            }

            return false; 
        }
        public Invention(Game game)
        {

            mGame = game;
            entityList = new List<Entity>();

            gui =  (IGUI)game.Services.GetService(typeof(IGUI));
            //physics = (IPhysicsService)mGame.Services.GetService(typeof(IPhysicsService));
            lineList = new List<Line>();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    sensorBlocked[i , j] = 0;
                }
            }
            
        }
        public void AttachBody(Entity movedEntity, Vector2 worldPos, float rotation)
        {//this should be called once the item has been dragged and rotated
         
 

            Point newGridPos = Point.Zero;
            //for (Fixture fix = sensors.GetFixtureList(); fix != null; fix = fix.GetNext())
            //{

            //    if (fix.TestPoint(worldPos))
            //    {//position object in space correctly
            //        newGridPos = new Point((int)((PolygonShape)fix.GetShape())._centroid.X+2, (int)((PolygonShape)fix.GetShape())._centroid.Y+2);//grid position starts top left, centroid is always centred on the hub
            //        if (CheckConnections(newGridPos.X, newGridPos.Y))
            //        {
            //            movedEntity.physicsBody.SetTransform(sensors.GetWorldCenter() + Vector2.Transform(((PolygonShape)fix.GetShape())._centroid, Matrix.CreateFromAxisAngle(Vector3.Backward, sensors.Rotation)), fix.GetBody().Rotation + rotation);
            //            if (entityList.Contains(movedEntity))
            //            {
            //                grid[movedEntity.gridPosition.X, movedEntity.gridPosition.Y] = null;
            //            }

            //            grid[newGridPos.X, newGridPos.Y] = movedEntity;
            //            movedEntity.gridPosition = newGridPos;
            //        }
            //        break;
            //    }
            //}
            Launch();

        }



        public void Launch()
        {
             
            foreach (Entity item in entityList)
            {
                //physics.UnRegisterJoint(item);
            }
            entityList.Clear();
            lineList.Clear();

            
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    if (grid[x,y] != null)
                    {
                        //grid[x, y].physicsBody.SetAngularVelocity(0);
                        //grid[x, y].physicsBody.SetLinearVelocity(Vector2.Zero);

                        entityList.Add(grid[x, y]);

                        if (y-1 >= 0 && 
                            grid[x,y-1] != null)
                        {//above
                            //physics.RegisterJoint(grid[x, y], grid[x, y - 1], Vector2.Zero); 
                            lineList.Add(new Line(grid[x, y],  grid[x, y - 1]));
                            
                        }
                        if (x - 1 >= 0 && 
                            grid[x- 1, y ] != null)
                        {//left
                            //physics.RegisterJoint(grid[x, y], grid[x- 1, y ], Vector2.Zero);
                            lineList.Add(new Line( grid[x, y],  grid[x- 1, y ]));
                            
                        }
                    }

                }
            }
            
            //physics.Resume();

        }
        public void Initialize()
        {
            //physics.RegisterHubSenors(out sensors);
        }
        public void LoadContent(ContentManager Content)
        {
            electricEffect = Content.Load<Texture2D>("electricity");
            greenSenTex = Content.Load<Texture2D>("GUI/GreenSensor");
            redSenTex = Content.Load<Texture2D>("GUI/RedSensor");

            
        }
        int targetElectricCount = 0;
        public void Update()
        {
            foreach (Entity item in entityList)
            {

                if (item.GetType() == typeof(Hub))
                {
                    //sensors.SetTransform(item.physicsBody.GetPosition(), item.physicsBody.GetAngle());
                }
            }
        }
        //public bool ContainsBody(Body body)
        //{
        //    foreach (Entity ent in entityList)
        //    {
        //        if (ent.physicsBody == body)
        //            return true;
        //    }
        //    return false;
        //}
        public bool PlaceablePoint(Vector2 position)
        {

            //for (Fixture fix = sensors.GetFixtureList(); fix != null; fix = fix.GetNext())
            //{
                
            //    if (fix.TestPoint( position))
            //    {
            //        //int c = sensorBlocked[(int)((PolygonShape)fix.GetShape())._centroid.X + 2, (int)((PolygonShape)fix.GetShape())._centroid.Y + 2]; 
            //        if (sensorBlocked[(int)((PolygonShape)fix.GetShape())._centroid.X + 2, (int)((PolygonShape)fix.GetShape())._centroid.Y + 2] == 0)
            //        {
            //            return true;
            //        }
            //    }
            //}
            return false;
        }
        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            Rectangle target = new Rectangle(targetElectricCount, 0, 100, electricEffect.Height);// electricEffect.Width, electricEffect.Height);
            Vector2 difference;

            //for (Fixture fix = sensors.GetFixtureList(); fix!=null; fix = fix.GetNext()) 
            //{
            //    int gridX = (int)((PolygonShape)fix.GetShape())._centroid.X + 2;
            //    int gridY = (int)((PolygonShape)fix.GetShape())._centroid.Y + 2;
            //    if (sensorBlocked[gridX, gridY]!=0 || grid[gridX, gridY] != null)
            //    {
            //        batch.Draw(redSenTex, sensors.GetWorldCenter() + Vector2.Transform(((PolygonShape)fix.GetShape())._centroid, Matrix.CreateFromAxisAngle(Vector3.Backward, sensors.Rotation)), null, Color.White, sensors.Rotation, new Vector2(greenSenTex.Width / 2f, greenSenTex.Height / 2f), .01f * (100f / (float)greenSenTex.Width), SpriteEffects.None, 0);

            //    }
            //    else
            //    {
            //        batch.Draw(greenSenTex, sensors.GetWorldCenter() + Vector2.Transform(((PolygonShape)fix.GetShape())._centroid, Matrix.CreateFromAxisAngle(Vector3.Backward, sensors.Rotation)), null, Color.White, sensors.Rotation, new Vector2(greenSenTex.Width / 2f, greenSenTex.Height / 2f), .01f * (100f / (float)greenSenTex.Width), SpriteEffects.None, 0);
            //    }
            //}
            //foreach (Line item in lineList)
            //{
            //    difference = item.finish.physicsBody.GetWorldCenter() - item.start.physicsBody.GetWorldCenter();
                
            //    float angle = (float)Math.Atan2(difference.Y, difference.X);
            //    batch.Draw(electricEffect, (item.finish.PhysicsBody.GetWorldCenter()-(difference/2f)), target, Color.White, angle, new Vector2(50,target.Center.Y), 0.01f, SpriteEffects.None, 0);
            //}
            targetElectricCount+=2;
            if (targetElectricCount + 100 >= electricEffect.Width)
            {
                targetElectricCount = 0;
            }
        }
    }
}
