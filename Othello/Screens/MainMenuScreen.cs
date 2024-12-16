using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Othello;

public class MainMenuScreen
{
	private Texture2D _pvpTextureButton;
	private Button _pvpButton;
	private Texture2D _pvcTextureButton;
	private Button _pvcButton;
	private Texture2D _rulesTextureButton;
	private Button _rulesButton;
	private Texture2D _logo;
	private Texture2D _pixelTexture;
	
	public event EventHandler<GameState> GameStateChanged;
	
	public void LoadContent(ContentManager content,  GraphicsDevice graphics)
	{
		_logo = content.Load<Texture2D>("Logo");
		
		_pvpTextureButton = content.Load<Texture2D>("pvpButton");
		_pvpButton = new Button(_pvpTextureButton, new Vector2(100,450));
		_pvpButton.LoadContent(content);
		_pvpButton.Clicked += PVPButton_Clicked;
		
		_pvcTextureButton = content.Load<Texture2D>("pvcButton");
		_pvcButton = new Button(_pvcTextureButton, new Vector2(500,450));
		_pvcButton.LoadContent(content);
		_pvcButton.Clicked += PVCButton_Clicked;
		
		_rulesTextureButton = content.Load<Texture2D>("rulesButton");
		_rulesButton = new Button(_rulesTextureButton, new Vector2(900,450));
		_rulesButton.LoadContent(content);
		_rulesButton.Clicked += RulesButton_Clicked;
		
		_pixelTexture = new Texture2D(graphics, 1, 1);
		_pixelTexture.SetData(new Color[] { Color.White });
	}

	public void Update(GameTime gameTime)
	{
		_pvpButton.Update();
		_pvcButton.Update();
		_rulesButton.Update();
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		Color rectangleColor1 = Color.Black;
		Rectangle rectangle1 = new Rectangle(45, 145, 1315, 615);
		spriteBatch.Draw(_pixelTexture, rectangle1, rectangleColor1*0.45f);
		
		Color rectangleColor2 = Color.DarkOrange;
		Rectangle rectangle2 = new Rectangle(50, 150, 1300, 600);
		spriteBatch.Draw(_pixelTexture, rectangle2, rectangleColor2);
		
		spriteBatch.Draw(_logo, new Vector2(310, 200), Color.White);
		
		_pvpButton.Draw(spriteBatch);
		_pvcButton.Draw(spriteBatch);
		_rulesButton.Draw(spriteBatch);
	}
	
	private void PVPButton_Clicked(object sender, System.EventArgs e)
	{
		GameStateChanged?.Invoke(this, GameState.Playing);
	}
	
	private void PVCButton_Clicked(object sender, System.EventArgs e)
	{
		GameStateChanged?.Invoke(this, GameState.PlayingAI);
	}
	
	private void RulesButton_Clicked(object sender, System.EventArgs e)
	{
		GameStateChanged?.Invoke(this, GameState.Rules);
	}
}
