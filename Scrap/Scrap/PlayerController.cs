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
    //if connected before then wait til it {leaves sensor or is released} before snapping back
    //
    //
    //
    //
    public class PlayerController
    {
        InputManager inputManager;
        ScrapGame game;
        Texture2D pointer;
        Texture2D pointerClosed;
        Vector2 mouseWorld;
        Body courserVolume;

        GamePadState gamePadState;
        //bool SegmentReleased = false;
        public Segment selectedSegment = null;

        List<Sensor> contactList = new List<Sensor>();

        //Button callback lists
        public Dictionary<Point,Action<float>> leftTrigger;
        public Dictionary<Point, Action<float>> rightTrigger;

        List<Action<bool>> aButton;
        List<Action<bool>> bButton;
        List<Action<bool>> xButton;
        List<Action<bool>> yButton;


        public void LoadContent()
        {

            
            pointer = game.Content.Load<Texture2D>("Pointer");
            pointerClosed = game.Content.Load<Texture2D>("PointerClosed");
            courserVolume = BodyFactory.CreateRoundedRectangle(((ScrapGame)game).world, 1f, 1f, .2f, .2f, 5, 2f, this);
            courserVolume.IsSensor = true;
            courserVolume.CollisionCategories = Category.Cat10;
            courserVolume.CollidesWith = Category.Cat10;
            courserVolume.OnCollision += courserVolume_OnCollision;
            courserVolume.OnSeparation += courserVolume_OnSeparation;
            leftTrigger = new Dictionary<Point,Action<float>>();
            rightTrigger = new Dictionary<Point,Action<float>>();

            aButton = new List<Action<bool>>();
            bButton = new List<Action<bool>>();
            xButton = new List<Action<bool>>();
            yButton = new List<Action<bool>>();

        }
        
        void courserVolume_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            //if (selectedSegment != null && fixtureB.Body.UserData as Sensor != null)
            //{
             //   wasInPlace = true;
                Debug.WriteLine("courserVolume_OnSeparation: Triggered");
            //}

        }

        public bool courserVolume_OnCollision(Fixture a, Fixture b, Contact contact)
        {
            if (selectedSegment != null)
            {
                contactList.Add(b.UserData as Sensor);
            }

            //if (selectedSegment != null && !SegmentReleased)
            //{
            //    //OnConstructSensorTriggered(selectedSegment.constructElement, b.UserData as Sensor);
            //    Debug.WriteLine("courserVolume_OnCollision: Triggered");
            //}

            return false;
        }
        private void OnSegmentReleasedInSensor(ConstructElement constructElement, Sensor sensor)
        {
            Debug.WriteLine("OnConstructSensorTriggered by " + constructElement.segment.ToString());
            Debug.WriteLine("OnConstructSensorTriggered on sensor " + sensor.GetOrientationRelativeToSegment().ToString());
            Debug.WriteLine("OnConstructSensorTriggered on segment:" + sensor.constructElement.offSet.ToString());

            //ToDo: Refactor- PlaceSegment is almost useless. 
            this.selectedSegment.body.Rotation = sensor.body.Rotation;
            this.selectedSegment.body.Position = sensor.constructElement.segment.Position - (sensor.constructElement.segment.Position -sensor.body.Position)*2;
            this.selectedSegment.body.LinearVelocity = sensor.body.LinearVelocity;

            this.game.buildMode = true;
            this.game.hudButtonMapping.AddSegment(selectedSegment);
            sensor.constructElement.construct.AddSegmentAtSensorPosition(selectedSegment, sensor);
            PlaceSegment();
        }
        public PlayerController(ScrapGame game)
        {
            inputManager = InputManager.GetManager();
            this.game = game;
        }
        public void Update()
        {
            

            inputManager.Update();

            this.game.hudButtonMapping.TriggerInput("TriggerLeft", gamePadState.Triggers.Left);
            this.game.hudButtonMapping.TriggerInput("TriggerRight", gamePadState.Triggers.Right);
            //leftTrigger.Values.ToList().ForEach(o => o.Invoke(gamePadState.Triggers.Left));
            //rightTrigger.Values.ToList().ForEach(o => o.Invoke(gamePadState.Triggers.Right));

            //aButton = new List<Action<bool>>();
            //bButton = new List<Action<bool>>();
            //xButton = new List<Action<bool>>();
            //yButton = new List<Action<bool>>();
            if (inputManager.PrevGamePadState.DPad.Left == ButtonState.Released 
                && inputManager.GamePadState.DPad.Left == ButtonState.Pressed)
            {
                game.hudConstruct.MoveSelection(Direction.Left);
            }
            if (inputManager.PrevGamePadState.DPad.Right == ButtonState.Released 
                && inputManager.GamePadState.DPad.Right == ButtonState.Pressed)
            {
                game.hudConstruct.MoveSelection(Direction.Right);
            }
            if (inputManager.PrevGamePadState.DPad.Up == ButtonState.Released 
                && inputManager.GamePadState.DPad.Up == ButtonState.Pressed)
            {
                game.hudConstruct.MoveSelection(Direction.Up);
            }
            if (inputManager.PrevGamePadState.DPad.Down == ButtonState.Released 
                && inputManager.GamePadState.DPad.Down == ButtonState.Pressed)
            {
                game.hudConstruct.MoveSelection(Direction.Down);
            }

            if (inputManager.PrevGamePadState.Buttons.A == ButtonState.Released
                && inputManager.GamePadState.Buttons.A == ButtonState.Pressed)
            {
                game.hudConstruct.SelectOrPlace();
            }

            if (inputManager.WasKeyReleased(Keys.Q)) game.camera.Rotate(-.005f);
            if (inputManager.WasKeyReleased(Keys.E)) game.camera.Rotate(+.005f);
            if (inputManager.WasKeyReleased(Keys.D)) game.camera.Position += new Vector2(1f, 0f);
            if (inputManager.WasKeyReleased(Keys.A)) game.camera.Position += new Vector2(-1f, 0f);
            if (inputManager.WasKeyReleased(Keys.W)) game.camera.Position += new Vector2(0f, -1f);
            if (inputManager.WasKeyReleased(Keys.S)) game.camera.Position += new Vector2(0f, 1f);
            if (inputManager.WasKeyReleased(Keys.Space)) game.camera.Position = new Vector2(0f, 0f);


            if (inputManager.PrevGamePadState.Buttons.B == ButtonState.Released
    && inputManager.GamePadState.Buttons.B == ButtonState.Pressed)
            {
                this.game.buildMode = !this.game.buildMode;
            }

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
                            if ((entity.constructElement != null && entity.constructElement.Draggable()))
                            {
                                if (entity.constructElement.construct != null && entity.constructElement.construct.KeyObject != entity) 
                                {
                                    List<Point> adjacentElements = entity.constructElement.adjacentElements;
                                    entity.constructElement.RemoveFromConstruct();
                                    //SegmentReleased = true;
                                }
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
            foreach (Sensor item in contactList)
            {
                Debug.WriteLine("Hover:" + item.GetOrientationRelativeToSegment().ToString());
            }
            contactList.Clear();
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
            if (contactList.Count > 0)
            {//ToDo: join to each adjacet object
                OnSegmentReleasedInSensor(selectedSegment.constructElement, contactList[0]);
            }
            else 
            {
                selectedSegment.constructElement.Status = ElementStatus.Free;
                this.game.hudButtonMapping.RemoveSegment(selectedSegment);
                selectedSegment = null;
                

            }
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
