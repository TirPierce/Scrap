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
            leftTrigger = new Dictionary<Point,Action<float>>();
            rightTrigger = new Dictionary<Point,Action<float>>();

            aButton = new List<Action<bool>>();
            bButton = new List<Action<bool>>();
            xButton = new List<Action<bool>>();
            yButton = new List<Action<bool>>();

        }
        public PlayerController(ScrapGame game)
        {
            inputManager = InputManager.GetManager();
            this.game = game;
        }
        const float Deadzone = 0.8f;
        const float DiagonalAvoidance = 0.2f;

        public static Direction GetThumbStickDirection(Vector2 gamepadThumbStick)
        {
            // Get the length and prevent something from happening
            // if it's in our deadzone.
            var length = gamepadThumbStick.Length();
            if (length < Deadzone)
                return Direction.None;

            var absX = Math.Abs(gamepadThumbStick.X);
            var absY = Math.Abs(gamepadThumbStick.Y);
            var absDiff = Math.Abs(absX - absY);

            // We don't like values that are too close to each other
            // i.e. borderline diagonal.
            if (absDiff < length * DiagonalAvoidance)
                return Direction.None;

            if (absX > absY)
            {
                if (gamepadThumbStick.X > 0)
                    return Direction.Right;
                else
                    return Direction.Left;
            }
            else
            {
                if (gamepadThumbStick.Y > 0)
                    return Direction.Up;
                else
                    return Direction.Down;
            }
        }
        private Direction GetDpadDirection()
        {
            if (inputManager.PrevGamePadState.DPad.Left == ButtonState.Released
                    && inputManager.GamePadState.DPad.Left == ButtonState.Pressed)
            {
                return Direction.Left;
            }
            else if (inputManager.PrevGamePadState.DPad.Right == ButtonState.Released
                && inputManager.GamePadState.DPad.Right == ButtonState.Pressed)
            {
                return Direction.Right;
            }
            else if (inputManager.PrevGamePadState.DPad.Up == ButtonState.Released
                && inputManager.GamePadState.DPad.Up == ButtonState.Pressed)
            {
                return Direction.Up;
            }
            else if (inputManager.PrevGamePadState.DPad.Down == ButtonState.Released
                && inputManager.GamePadState.DPad.Down == ButtonState.Pressed)
            {
                return Direction.Down;
            }
            else return Direction.None;
        }
        public void Update()
        {
            inputManager.Update();


            

            switch (game.constructBuilder.BuildMode)
            {
                case UserInterface.BuildMode.Seek:
                    #region seek
                    game.constructBuilder.MoveSeek(GetDpadDirection());

                    if (inputManager.PrevGamePadState.Buttons.A == ButtonState.Released
                        && inputManager.GamePadState.Buttons.A == ButtonState.Pressed)
                    {
                        game.constructBuilder.SetSelectedSegmentAndSetBuildModeToSelect();
                    }

                    if (inputManager.PrevGamePadState.Buttons.X == ButtonState.Released
                    && inputManager.GamePadState.Buttons.X == ButtonState.Pressed)
                    {
                        this.game.constructBuilder.BuildMode = UserInterface.BuildMode.ControlBind;
                    }

                    if (inputManager.PrevGamePadState.Buttons.LeftShoulder == ButtonState.Released
                        && inputManager.GamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
                    {
                        game.constructBuilder.BuildMode = UserInterface.BuildMode.Inactive;

                    }

                    break;
                #endregion
                case UserInterface.BuildMode.Selected:
                    #region Selected
                    game.constructBuilder.MoveSelection(GetDpadDirection());

                    if (GetThumbStickDirection(inputManager.GamePadState.ThumbSticks.Right) != Direction.None)
                    {
                        if (GetThumbStickDirection(inputManager.GamePadState.ThumbSticks.Right) != GetThumbStickDirection(inputManager.PrevGamePadState.ThumbSticks.Right))
                        {
                            this.game.constructBuilder.SelectedObjectDirection = GetThumbStickDirection(inputManager.GamePadState.ThumbSticks.Right);
                            //.SelectDirection(());
                        }
                    }
                    if (inputManager.PrevGamePadState.Buttons.A == ButtonState.Released
                        && inputManager.GamePadState.Buttons.A == ButtonState.Pressed)
                    {
                        game.constructBuilder.PlaceSelectedSegmentAtCurrentPositionAndSetModeToSeek();

                    }
                    if (inputManager.PrevGamePadState.Buttons.LeftShoulder == ButtonState.Released
                        && inputManager.GamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
                    {
                        game.constructBuilder.BuildMode = UserInterface.BuildMode.Inactive;

                    }

                    break;
                #endregion
                case UserInterface.BuildMode.ControlBind:
                    #region ControlBind
                    if (inputManager.PrevGamePadState.Buttons.LeftShoulder == ButtonState.Released
                        && inputManager.GamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
                    {
                        this.game.constructBuilder.BuildMode = UserInterface.BuildMode.Seek;
                    }

                    if (inputManager.GamePadState.Triggers.Right > .5f)
                    {
                        if(inputManager.PrevGamePadState.Triggers.Right <= .5f)
                            game.constructBuilder.BindInput(false);
                    }
                    else if (inputManager.GamePadState.Triggers.Left > .5f )
                    {
                        if(inputManager.PrevGamePadState.Triggers.Left <= .5f)
                            game.constructBuilder.BindInput(true);
                    }

                    break;
                #endregion
                case UserInterface.BuildMode.Inactive:
                    if (inputManager.GamePadState.Triggers.Left > .1f)
                    {
                        //ToDo: this should be replaced with event queue
                        this.game.constructBuilder.hudButtonMapping.TriggerInput("TriggerLeft", inputManager.GamePadState.Triggers.Left);
                    }
                    if (inputManager.GamePadState.Triggers.Right > .1f)
                    {

                        this.game.constructBuilder.hudButtonMapping.TriggerInput("TriggerRight", inputManager.GamePadState.Triggers.Right);
                    }
                    if (inputManager.PrevGamePadState.Buttons.LeftShoulder == ButtonState.Released
    && inputManager.GamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
                    {
                        game.constructBuilder.BuildMode = UserInterface.BuildMode.Seek;

                    }
                    break;
                default:
                    break;
            }

            
            foreach (Sensor item in contactList)
            {
                Debug.WriteLine("Hover:" + item.GetOrientationRelativeToSegment().ToString());
            }
            contactList.Clear();
        }

        public void GetAvailableJoins()
        {
            //When segment is held, indicate where it can be placed (unobstructed segment sensors)
        }
        public void SetSegmentLock()
        {
            //Lock Segment in position until orientation is selected
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //if (selectedSegment != null)
            //{
            //    spriteBatch.Draw(pointerClosed, new Rectangle((int)game.camera.ProjectPoint(selectedSegment.body.Position).X - 10, (int)game.camera.ProjectPoint(selectedSegment.body.Position).Y - 10, 20, 20), Color.BlanchedAlmond);
            //}
            //else
            //{
            //    if (inputManager.MouseState.LeftButton == ButtonState.Released)
            //        spriteBatch.Draw(pointer, new Rectangle((int)inputManager.MouseState.Position.X - 10, (int)inputManager.MouseState.Position.Y - 10, 20, 20), Color.BlanchedAlmond);
            //    else
            //        spriteBatch.Draw(pointerClosed, new Rectangle((int)inputManager.MouseState.Position.X - 10, (int)inputManager.MouseState.Position.Y - 10, 20, 20), Color.BlanchedAlmond);
            //}
        }
    }
}
