using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap
{

    class PlayerController
    {
        InputManager inputManager;
        ScrapGame game;
        Texture2D pointer;
        Texture2D pointerClosed;
        Vector2 mouseWorld;
        public void LoadContent()
        {
            
            pointer = game.Content.Load<Texture2D>("Pointer");
            pointerClosed = game.Content.Load<Texture2D>("PointerClosed");
        }
        public PlayerController(ScrapGame game)
        {
            inputManager = new InputManager();
            this.game = game;
        }
        public void Update()
        {
            inputManager.Update();

            if (inputManager.WasKeyReleased(Keys.Q))
            {
                game.camera.Rotate(-.005f);
            }
            if (inputManager.WasKeyReleased(Keys.E))
            {
                game.camera.Rotate(+.005f);
            }
            if (inputManager.WasKeyReleased(Keys.D))
            {
                game.camera.Position += new Vector2(1f, 0f);
            }
            if (inputManager.WasKeyReleased(Keys.A))
            {
                game.camera.Position += new Vector2(-1f, 0f);
            }
            if (inputManager.WasKeyReleased(Keys.W))
            {
                game.camera.Position += new Vector2(0f, -1f);
            }
            if (inputManager.WasKeyReleased(Keys.S))
            {
                game.camera.Position += new Vector2(0f, 1f);
            }
            if (inputManager.WasKeyReleased(Keys.Space))
            {
                game.camera.Position = new Vector2(0f, 0f);
            }
            game.camera.Zoom(inputManager.ScroleWheelDelta() * .01f);//ToDo: Camera Controls need to be changed
            mouseWorld = (game.camera.MousePick(inputManager.MouseState.Position));
            if (inputManager.MouseState.LeftButton == ButtonState.Pressed)
            {
                //ToDo: add currently selected object and state
                foreach(var entity in this.game.entityList)
                {
                    if(entity.TestEntity(ref mouseWorld))
                        entity.Position = mouseWorld;
                
                }
                //if (fixture != null)
                //    fixture.Body.Position = mouseWorld;
                
            }  
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if(inputManager.MouseState.LeftButton == ButtonState.Released)
                spriteBatch.Draw(pointer, new Rectangle((int)inputManager.MouseState.Position.X - 10, (int)inputManager.MouseState.Position.Y - 10, 20, 20), Color.BlanchedAlmond);
            else
                spriteBatch.Draw(pointerClosed, new Rectangle((int)inputManager.MouseState.Position.X - 10, (int)inputManager.MouseState.Position.Y - 10, 20, 20), Color.BlanchedAlmond);
            
                
        
        }
    }
}
