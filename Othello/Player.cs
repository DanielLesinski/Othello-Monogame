namespace Othello;

public class Player
{
	public int Points{get; set;}
	public int Name {get; private set;}
	
	public Player(int name)
	{
		Points = 2;
		Name = name;
	}
}
