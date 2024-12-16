using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Othello;

public class RulesScreen
{
	private SpriteFont _font;
	private Texture2D _pixelTexture;
	private string _text;
	
	public void LoadContent(ContentManager content,  GraphicsDevice graphics)
	{
		_font = content.Load<SpriteFont>("galleryfont");
		_pixelTexture = new Texture2D(graphics, 1, 1);
		_pixelTexture.SetData(new Color[] { Color.White });
		
		try
        {
            using (StreamReader sr = new StreamReader("Content/rules.txt"))
            {
                _text = sr.ReadToEnd();
            }
        }
        catch
        {
            _text = "Rules file not found";
        }
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		Color rectangleColor1 = Color.Black;
		Rectangle rectangle1 = new Rectangle(45, 145, 1315, 615);
		spriteBatch.Draw(_pixelTexture, rectangle1, rectangleColor1*0.45f);
		
		Color rectangleColor2 = Color.DarkOrange;
		Rectangle rectangle2 = new Rectangle(50, 150, 1300, 600);
		spriteBatch.Draw(_pixelTexture, rectangle2, rectangleColor2);
		
		spriteBatch.DrawString(_font, "RULES", new Vector2(600, 150), Color.Brown, 0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0f);
		spriteBatch.DrawString(_font, _text, new Vector2(100, 230), Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
		spriteBatch.DrawString(_font, "Press ESC to back to the Menu", new Vector2(480, 700), Color.DimGray);
	}
}
