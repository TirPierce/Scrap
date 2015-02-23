using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scrap.Entities;
using Scrap.GameState;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
//using Scrap.Physics;

namespace Scrap.UserInterface
{
    enum UIState
    {
        free, dragging, binding
    };
    class UIManager
    {
        Vector2 lastMousePos;
        UIState currentState;
        Entity currentHeld;
        List<Entity> buttonA, buttonB, buttonC, buttonD, worldEntities;
        Invention invention;
        Camera camera;
        //PhysicsEngineMain physics;
        public UIManager(List<Entity> worldEntities, Invention invention, Camera camera)//, PhysicsEngineMain physics)
        {
            this.invention = invention;
            this.worldEntities = worldEntities;
            this.camera = camera;
            //this.physics = physics;
            buttonA = new List<Entity>();
            buttonB = new List<Entity>();
            buttonC = new List<Entity>();
            buttonD = new List<Entity>();
        }
        public void Update()
        {
            Matrix inverseViewMatrix = Matrix.Invert(camera.Transformation);

            Vector2 worldMousePosition = Vector2.Transform(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), inverseViewMatrix);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {

                if (currentState == UIState.free)
                {
                    foreach (Entity ent in worldEntities)
                    {
                        //if (ent.Picked(worldMousePosition))
                        //{
                        //    currentHeld = ent;
                        //    currentState = UIState.dragging;
                        //    break;

                        //}
                    }
                }
                else if (currentState == UIState.dragging)
                {
                    //physics.Pause();
                    //currentHeld.physicsBody.Position = worldMousePosition;
                }



                lastMousePos = worldMousePosition;
            }
            else if(currentState == UIState.dragging)
            {
                //hack
                if (invention.PlaceablePoint(worldMousePosition))
                {
                    //change to placement
                    invention.AttachBody(currentHeld, worldMousePosition, 0f);//3rd argument should be rotation from placement
                    //invention.Contact(currentHeld.physicsBody);
                }
                else
                {

                    foreach (Entity ent in invention.entityList)
                    {
                        //ent.physicsBody.SetAngularVelocity(0);
                        //ent.physicsBody.SetLinearVelocity(Vector2.Zero);
                    }
                    //physics.Resume();
                }
                currentState = UIState.free;//should be binding
                //place at last mouse pos
            }   


           

        }
        public void Draw(SpriteBatch batch)
        {
            if (currentState == UIState.dragging)
            {
                currentHeld.DrawUI(batch, lastMousePos, 0f); 
                foreach (Entity ent in invention.entityList)
                {
                    ent.Draw(batch);
                }
            }
            else
            {
                if (currentState == UIState.binding)
                {
                    currentHeld.Draw(batch);
                }
            }
        }
    }
}
