using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Othello;
public enum GameState
{
	MainMenu,
	Playing,
	PlayingAI,
	Rules,
}
	
public class OthelloGame : Game
{
	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;

	private GameState _currentState;
	private MainMenuScreen _mainMenuScreen;
	private GameScreen _gameScreen;
	private RulesScreen _rulesScreen;
	private bool _isPressed;

	public OthelloGame()
	{
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	protected override void Initialize()
	{
		_graphics.PreferredBackBufferWidth = 1400; // szerokość w pikselach
		_graphics.PreferredBackBufferHeight = 900; // wysokość w pikselach
		_graphics.ApplyChanges();
		
		_gameScreen = new GameScreen();
		_mainMenuScreen = new MainMenuScreen();
		_rulesScreen = new RulesScreen();
		_currentState = GameState.MainMenu;
		
		_isPressed = false;

		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);
		
		_gameScreen.LoadContent(Content, GraphicsDevice);
		_mainMenuScreen.LoadContent(Content, GraphicsDevice);
		_rulesScreen.LoadContent(Content, GraphicsDevice);
		
		_mainMenuScreen.GameStateChanged += MainMenuScreen_GameStateChanged;

	}

	protected override void Update(GameTime gameTime)
	{
		if(IsActive)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !_isPressed)
			{
				_isPressed = true;
				if (_currentState == GameState.MainMenu)
					Exit();
				else
				{
					if(_currentState == GameState.Playing || _currentState == GameState.PlayingAI)
						_gameScreen.ReStart();
					_currentState = GameState.MainMenu;
				}
					
			}
			
			if(Keyboard.GetState().IsKeyUp(Keys.Escape))
				_isPressed = false;

			switch (_currentState)
			{
				case GameState.MainMenu:
					_mainMenuScreen.Update(gameTime);
					break;
				case GameState.Playing:
					if(_gameScreen.IsAI) _gameScreen.IsAI = false;
					_gameScreen.Update(gameTime);
					break;
				case GameState.PlayingAI:
					if(!_gameScreen.IsAI) _gameScreen.IsAI = true;
					_gameScreen.Update(gameTime);
					break;
				default:
					break;
			}
		}

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.OrangeRed);

		_spriteBatch.Begin();
		
		switch (_currentState)
		{
			case GameState.MainMenu:
				_mainMenuScreen.Draw(_spriteBatch);
				break;
			case GameState.Playing:
			case GameState.PlayingAI:
				_gameScreen.Draw(_spriteBatch);
				break;
			case GameState.Rules:
				_rulesScreen.Draw(_spriteBatch);
				break;
		}
	
		_spriteBatch.End();

		base.Draw(gameTime);
	}
	
	private void MainMenuScreen_GameStateChanged(object sender, GameState newState)
	{
		// Zmiana stanu gry na nowy stan
		_currentState = newState;
	}
}
