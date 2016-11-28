using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelEditor
{
    public class InputManager
    {
        KeyboardState prevKeyState = Keyboard.GetState();
        MouseState prevMouseState = Mouse.GetState();
        GamePadState prevGamePadState = new GamePadState();
        public GamePadState PrevGamePadState
        {
            get{return prevGamePadState;}
        }
        GamePadState curGamePadState;
        public GamePadState GamePadState
        {
            get{return curGamePadState;}
        }
        public MouseState PrevMouseState
        {
            get { return prevMouseState; }
        }
        KeyboardState curKeyState = Keyboard.GetState();

        public KeyboardState KeyState
        {
          get { return curKeyState; }
        }
        MouseState curMouseState = Mouse.GetState();

        public MouseState MouseState
        {
          get { return curMouseState; }
        }



        private static InputManager instance;
        private InputManager()
        {
        }
        public static InputManager GetManager()
        {
            if(instance == null)
            {
                instance = new InputManager();
            }
            return instance;
        }
        public void Update()
        {
            prevKeyState = curKeyState;
            prevMouseState = curMouseState;
            prevGamePadState = curGamePadState;
            curKeyState = Keyboard.GetState();
            curMouseState = Mouse.GetState();
            curGamePadState = GamePad.GetState(PlayerIndex.One);


        }
        
        public bool WasKeyPressed(Keys key)
        {
            return (curKeyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key));
        }
        public bool WasKeyReleased(Keys key)
        {
            return (prevKeyState.IsKeyDown(key) && curKeyState.IsKeyUp(key));
        }
        public int ScroleWheelDelta()
        {
            return curMouseState.ScrollWheelValue - prevMouseState.ScrollWheelValue;
        }
    }
}
