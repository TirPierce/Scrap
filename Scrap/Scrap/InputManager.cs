using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scrap
{
    public class InputManager
    {
        KeyboardState prevKeyState = Keyboard.GetState();
        MouseState prevMouseState = Mouse.GetState();
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
        public InputManager()
        {
        }
        public void Update()
        {
            prevKeyState = curKeyState;
            prevMouseState = curMouseState;
            curKeyState = Keyboard.GetState();
            curMouseState = Mouse.GetState();
            
        }
        public bool WasKeyReleased(Keys key)
        {
            return (prevKeyState.IsKeyDown(key) && curKeyState.IsKeyDown(key));
        }
        public int ScroleWheelDelta()
        {
            return curMouseState.ScrollWheelValue - prevMouseState.ScrollWheelValue;
        }
    }
}
