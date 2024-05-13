using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Othello;

public class Button
{
	private Texture2D _texture;
	private Vector2 _position;
	private Rectangle _rect;
	private Color _color = Color.White;
	private SoundEffect _effect;
	private MouseState currentMouseState;
	private MouseState previousMouseState;
	
	public event EventHandler Clicked;
	
	public Button(Texture2D texture, Vector2 position)
	{
		_texture = texture;
		_position = position;
		_rect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
	}
	
	public void LoadContent(ContentManager content)
	{
		_effect = content.Load<SoundEffect>("click");
	}
	
	public void Update()
	{
		currentMouseState = Mouse.GetState();
		Rectangle cursor = new Rectangle(currentMouseState.Position.X, currentMouseState.Position.Y, 1, 1);
		
		if(cursor.Intersects(_rect))
		{
			_color = Color.DarkGray;
			
			if(currentMouseState.LeftButton == ButtonState.Released
			 && previousMouseState.LeftButton == ButtonState.Pressed)
			{
				_effect.Play();
				Clicked?.Invoke(this, EventArgs.Empty);
			}
		}
		else
		{
			_color = Color.White;
		}
		previousMouseState = currentMouseState;
	}
	
	public void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(_texture,_position,_color);
	}
}
