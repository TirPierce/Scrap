using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scrap.GameState;

namespace Scrap.UserInterface
{
    enum GamePlayState
    {
        Pause,
        PilotMode,
        BuildMode
    }
    class GameplayScreen:BaseScreen
    {
        GUI mGUI;
        PlayerMatch currentMatch;
        

        GamePlayState mGameState;
        
        
        public GameplayScreen(Game game)
            : base(game)
        {
            mGUI = new GUI(game);
            

            mGameState = GamePlayState.BuildMode;
            currentMatch = new GameState.PlayerMatch(game);
            
            
        }
        public override void Initialize()
        {
            currentMatch.Initialize();
         }
        public override void Update(GameTime time)
        {

           // mGUI.Update(mGameState);
            mGUI.Update();
            currentMatch.Update(time);
            
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            
            currentMatch.Draw(spriteBatch);
            spriteBatch.Begin();
    

            mGUI.Draw(spriteBatch);
            spriteBatch.End();

        }
        public override void LoadContent()
        {
            currentMatch.LoadContent();
            mGUI.LoadContent();

        }
    }
}
