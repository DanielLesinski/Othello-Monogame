using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Othello;

public class GameScreen
{
	private Texture2D _boardTexture;
	private Texture2D _disc;
	private SpriteFont _font;
	private SoundEffect _effect;
	private Texture2D _pixelTexture;
	
	private int _tileSize;
	private Board _board;
	private Player _whitePlayer;
	private Player _blackPlayer;
	private Player _activePlayer;
	private List<(int,int)> _moves;
	private bool _hint;
	private bool _isMoveDone;
	private bool _isGameOver;
	private bool _isSearchingMove;
	private AlphaBeta _ab;
	
	public bool IsAI {get; set;}
	
	public void LoadContent(ContentManager content, GraphicsDevice graphics)
	{
		_boardTexture = content.Load<Texture2D>("siatka");
		_disc = content.Load<Texture2D>("disc");
		_font = content.Load<SpriteFont>("galleryFont");
		_effect = content.Load<SoundEffect>("switch");
		
		_pixelTexture = new Texture2D(graphics, 1, 1);
		_pixelTexture.SetData(new Color[] { Color.White });
		
		_ab = new AlphaBeta();
		
		ReStart();
	}
	
	public void ReStart()
	{
		_board = new Board();
		_whitePlayer = new Player(-1);
		_blackPlayer = new Player(1);
		_activePlayer = _blackPlayer; // Początkowo czarny gracz zaczyna
		_tileSize = 100;
		_moves = new List<(int, int)>();

		_isMoveDone = false;
		_hint = false;
		_isSearchingMove = false;
		
		_isGameOver = false;
		
	}

	public void Update(GameTime gameTime)
	{

		if (_board.IsGameOver())
			_isGameOver = true;
		
		if(!_isGameOver)
		{
			if(_isMoveDone)
			{
				_activePlayer = (_activePlayer == _blackPlayer) ? _whitePlayer : _blackPlayer;
				_isMoveDone = false;
			}
			
			_moves = _board.GetValidMoves(_activePlayer.Name);
				if(!_moves.Any())
					_isMoveDone = true;
			
			if(IsAI && _activePlayer == _whitePlayer)
			{
				if (!_isSearchingMove && !_isMoveDone)
				{
					int depth = 5;
					object depthObj = depth;
					Thread backgroundThread = new Thread(new ParameterizedThreadStart(FindMoveInBackground));
					backgroundThread.Start(depthObj);
					_isSearchingMove = true;
				}
			}
			else
			{
				_isSearchingMove = false;
				
				MouseState mouseState = Mouse.GetState();
				
				if(mouseState.RightButton == ButtonState.Pressed)
					_hint = true;
				if(mouseState.RightButton == ButtonState.Released)
					_hint = false;
				
				if (mouseState.LeftButton == ButtonState.Pressed)
				{
					// Konwertowanie pozycji myszy na indeksy pola na planszy
					float mouseX = mouseState.X;
					float mouseY = mouseState.Y;

					float boardX = (mouseX - 300) / _tileSize; // Odjęcie przesunięcia planszy
					float boardY = (mouseY - 50) / _tileSize;
					
					// Wykonanie ruchu na planszy
					if (boardX >= 0 && boardX < _board.BoardSize && boardY >= 0 && boardY < 8)
					{
						// Sprawdzenie, czy pole jest puste
						if (_board.GetPosition((int)boardX, (int)boardY) == 0)
						{
							_isMoveDone = _board.MakeMove((int)boardX,(int)boardY, _activePlayer.Name);
							if(_isMoveDone)
							{
								_blackPlayer.Points = _board.GetBlackCount();
								_whitePlayer.Points = _board.GetWhiteCount();
								_effect.Play();
							}
								
						}
					}
				}
			}
		}
		
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		
		spriteBatch.Draw(_disc, new Vector2(110,350), Color.White);
		spriteBatch.Draw(_disc, new Vector2(110,450), Color.Black);
		spriteBatch.DrawString(_font, "POINTS", new Vector2(105, 315), Color.Black);
		spriteBatch.DrawString(_font, _whitePlayer.Points.ToString("00"), new Vector2(144, 381), Color.Black);
		spriteBatch.DrawString(_font, _blackPlayer.Points.ToString("00"), new Vector2(144, 481), Color.White);
		
		spriteBatch.Draw(_boardTexture, new Vector2(300,50), Color.White);
		
		spriteBatch.DrawString(_font, "TURN", new Vector2(1207, 365), Color.Black);
		Color col = (_activePlayer == _blackPlayer) ? Color.Black : Color.White;
		spriteBatch.Draw(_disc, new Vector2(1200,400), col);
		
		for(int i = 0; i < _board.BoardSize; i++)
		{
			for(int j = 0; j < _board.BoardSize; j++)
			{
				if(_hint && _moves.Contains((i,j)))
				{
					int x = 300 + i * _tileSize;
					int y = 50 + j * _tileSize;
					//spriteBatch.Draw(_disc, new Rectangle(x+35, y+35, 30, 30), Color.Green * 0.45f);
					spriteBatch.Draw(_pixelTexture, new Rectangle(x+1, y+1, 97, 97), Color.Green * 0.45f);
				}

				int player = _board.GetPosition(i, j);
				if(player != 0)
				{
					Color color = (player == _blackPlayer.Name) ? Color.Black : Color.White;
					Vector2 position = new Vector2(300 + i * _tileSize, 50 + j * _tileSize);
					spriteBatch.Draw(_disc, position, color);
				}
			}
		}
		
		if(_isGameOver)
		{
			Color rectangleColor1 = Color.Black;
			Rectangle rectangle1 = new Rectangle(395, 295, 615, 315);
			spriteBatch.Draw(_pixelTexture, rectangle1, rectangleColor1*0.45f);
			
			Color rectangleColor2 = Color.DarkOrange;
			Rectangle rectangle2 = new Rectangle(400, 300, 600, 300);
			spriteBatch.Draw(_pixelTexture, rectangle2, rectangleColor2);
			
			spriteBatch.DrawString(_font, "GAME OVER", new Vector2(500, 350), Color.Brown, 0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0f);
			if(_blackPlayer.Points > _whitePlayer.Points)
				spriteBatch.DrawString(_font, "BLACK WINS", new Vector2(550, 420), Color.Brown, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
			else if(_blackPlayer.Points < _whitePlayer.Points)
				spriteBatch.DrawString(_font, "WHITE WINS", new Vector2(550, 420), Color.Brown, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
			else
				spriteBatch.DrawString(_font, "DRAW", new Vector2(630, 420), Color.Brown, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
			spriteBatch.DrawString(_font, "Press ESC to back to the Menu", new Vector2(480, 500), Color.DimGray);

		}
	}
	
	private void FindMoveInBackground(object depthObj)
	{
		int depth = (int)depthObj;
		var move = _ab.AlphaBetaSearch(_board, _activePlayer.Name, depth);
		Thread.Sleep(300);
		_isMoveDone = _board.MakeMove(move.Item1, move.Item2, _activePlayer.Name);
		if(_isMoveDone)
		{
			_blackPlayer.Points = _board.GetBlackCount();
			_whitePlayer.Points = _board.GetWhiteCount();
			_effect.Play();
		}
	}
}
