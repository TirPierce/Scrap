using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Scrap.GameElements.Building;
using Scrap.GameElements.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap
{
    public class PlayerController
    {
        InputManager inputManager;
        ScrapGame game;
        Texture2D pointer;
        Texture2D pointerClosed;
        Vector2 mouseWorld;
        Body courserVolume;
        public Segment selectedSegment = null;
        //Scrap.GameElements.Entities.Direction validDirections;
        public void LoadContent()
        {
            pointer = game.Content.Load<Texture2D>("Pointer");
            pointerClosed = game.Content.Load<Texture2D>("PointerClosed");
            courserVolume = BodyFactory.CreateRoundedRectangle(((ScrapGame)game).world, 1f, 1f, .2f, .2f, 5, 2f, this);
            courserVolume.IsSensor = false;
            courserVolume.CollisionCategories = Category.Cat10;
            courserVolume.CollidesWith = Category.Cat10;
            courserVolume.OnCollision += OnCollide;
            
        }
        public bool OnCollide(Fixture a, Fixture b, Contact contact) 
        {
            if (selectedSegment != null) 
            { 
                OnConstructSensorTriggered(selectedSegment.constructElement, b.UserData as Sensor);
            }
            return false; 
        }
        public PlayerController(ScrapGame game)
        {
            
            inputManager = InputManager.GetManager();
            this.game = game;
        }
        public void Update()
        {
            inputManager.Update();

            if (inputManager.WasKeyReleased(Keys.Q))game.camera.Rotate(-.005f);
            if (inputManager.WasKeyReleased(Keys.E))game.camera.Rotate(+.005f);
            if (inputManager.WasKeyReleased(Keys.D))game.camera.Position += new Vector2(1f, 0f);
            if (inputManager.WasKeyReleased(Keys.A))game.camera.Position += new Vector2(-1f, 0f);
            if (inputManager.WasKeyReleased(Keys.W))game.camera.Position += new Vector2(0f, -1f);
            if (inputManager.WasKeyReleased(Keys.S))game.camera.Position += new Vector2(0f, 1f);
            if (inputManager.WasKeyReleased(Keys.Space))game.camera.Position = new Vector2(0f, 0f);
            
            
            game.camera.Zoom(inputManager.ScroleWheelDelta() * .01f);//ToDo: Camera Controls need to be changed
            mouseWorld = (game.camera.MousePick(inputManager.MouseState.Position));
            courserVolume.Position = mouseWorld;
            if (inputManager.MouseState.LeftButton == ButtonState.Pressed && inputManager.PrevMouseState.LeftButton == ButtonState.Released)
            {
                if (!game.gui.MouseClick(mouseWorld))
                {
                    foreach (var entity in this.game.entityList)
                    {
                        if (entity.IsPointContained(ref mouseWorld))
                        {
                            if (entity.constructElement == null)
                            {
                                SetSelectedSegment(entity);
                                break;
                            }
                            if ((entity.constructElement != null && entity.constructElement.Draggable()))
                            {
                                if (entity.constructElement.construct != null && entity.constructElement.construct.KeyObject != entity)
                                    entity.constructElement.RemoveFromConstruct();
                                SetSelectedSegment(entity);
                                break;
                            }
                        }
                    }
                }
            }
            else if (selectedSegment != null && inputManager.MouseState.LeftButton == ButtonState.Released && inputManager.PrevMouseState.LeftButton == ButtonState.Pressed)
            {
                 ReleaseSegment();
            }

            if (selectedSegment != null)
            {
                DragSelectedSegment();
            }
        }
        private void SetSelectedSegment(Segment segment)
        {
            selectedSegment = segment;
            selectedSegment.constructElement.Status = ElementStatus.Selected;
            selectedSegment.constructElement.DisableSensors();
        }
        protected void DragSelectedSegment()
        {
            //float length = (mouseWorld - selectedSegment.body.Position).Length();
            //Vector2 direction = (mouseWorld - selectedSegment.body.Position) / length;

            //selectedSegment.body.ApplyLinearImpulse(direction);
            //Mouse.SetPosition((int)game.camera.ProjectPoint(selectedSegment.body.Position).X, (int)game.camera.ProjectPoint(selectedSegment.body.Position).Y);
            selectedSegment.Position = mouseWorld;
        }
        private void OnConstructSensorTriggered(ConstructElement constructElement, Sensor sensor)
        {
            Debug.WriteLine("OnConstructSensorTriggered by " + constructElement.segment.ToString());
            Debug.WriteLine("OnConstructSensorTriggered on sensor " + sensor.direction.ToString());
            Debug.WriteLine("OnConstructSensorTriggered on segment:" + sensor.constructElement.offSet.ToString());
            
            //ToDo: Refactor- PlaceSegment is almost useless. 
            this.selectedSegment.body.Rotation = sensor.body.Rotation;
            this.selectedSegment.body.Position = sensor.body.Position;
            this.selectedSegment.body.LinearVelocity = sensor.body.LinearVelocity;

            sensor.constructElement.construct.AddSegmentAtSensorPosition(selectedSegment, sensor);
            PlaceSegment();

        }
        public void GetAvailableJoins()
        {
            //When segment is held, indicate where it can be placed (unobstructed segment sensors)
        }
        public void SetSegmentLock()
        {
            //Lock Segment in position until orientation is selected
        }
        protected void ReleaseSegment()
        {
            selectedSegment.constructElement.Status = ElementStatus.Free;
            selectedSegment = null;
        }
        public void PlaceSegment()
        {
            selectedSegment.constructElement.Status = ElementStatus.Locked;
            selectedSegment = null;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (selectedSegment != null)
            {
                spriteBatch.Draw(pointerClosed, new Rectangle((int)game.camera.ProjectPoint(selectedSegment.body.Position).X - 10, (int)game.camera.ProjectPoint(selectedSegment.body.Position).Y - 10, 20, 20), Color.BlanchedAlmond);
            }
            else
            {
                if (inputManager.MouseState.LeftButton == ButtonState.Released)
                    spriteBatch.Draw(pointer, new Rectangle((int)inputManager.MouseState.Position.X - 10, (int)inputManager.MouseState.Position.Y - 10, 20, 20), Color.BlanchedAlmond);
                else
                    spriteBatch.Draw(pointerClosed, new Rectangle((int)inputManager.MouseState.Position.X - 10, (int)inputManager.MouseState.Position.Y - 10, 20, 20), Color.BlanchedAlmond);
            }
        }
    }
}
