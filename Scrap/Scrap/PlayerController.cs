using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Scrap.GameElements.Entities;
using System;
using System.Collections.Generic;
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
            courserVolume.IsSensor = true;
            courserVolume.CollisionCategories = Category.Cat10;
            courserVolume.CollidesWith = Category.Cat10;
            
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
                MoveSegment();
            }
        }
        private void SetSelectedSegment(Segment segment)
        {
            selectedSegment = segment;
            selectedSegment.constructElement.Status = ElementStatus.Selected;
            selectedSegment.constructElement.DisableSensors();
        }
        protected void MoveSegment()
        {
            //float length = (mouseWorld - selectedSegment.body.Position).Length();
            //Vector2 direction = (mouseWorld - selectedSegment.body.Position) / length;

            //selectedSegment.body.ApplyLinearImpulse(direction);
            //Mouse.SetPosition((int)game.camera.ProjectPoint(selectedSegment.body.Position).X, (int)game.camera.ProjectPoint(selectedSegment.body.Position).Y);
            selectedSegment.Position = mouseWorld;
        }
        public void OnConstructSensorTriggered(ConstructElement constructElement, Point offSet)
        {
            Vector2 target = ConstructElement.GetSensorOffset(Construct.PointToDirection(offSet - constructElement.offSet));
            Transform t;
            constructElement.segment.body.GetTransform(out t);
            target = Vector2.Transform(target, Quaternion.CreateFromAxisAngle(Vector3.Backward, constructElement.segment.body.Rotation));
            target += constructElement.segment.body.WorldCenter;
            //if ((target - this.selectedSegment.body.Position).Length() < .5f)
            {
                this.selectedSegment.body.Rotation = constructElement.segment.body.Rotation;
                this.selectedSegment.body.Position = target;
                this.selectedSegment.body.LinearVelocity = Vector2.Zero;

                foreach (Segment item in game.entityList)
                {
                    if (item.body == this.selectedSegment.body)
                    {
                        PlaceSegment();
                        //Set Lock here
                        constructElement.construct.AddSegment(item, offSet);
                        break;

                    }
                }


            }
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
