using System.Collections.Generic;

namespace Othello;

public class Board
{
	public readonly int BoardSize = 8;
	private int[,] _board;
	private int _blackCount;
	private int _whiteCount;

	public Board()
	{
		_board = new int[BoardSize, BoardSize];

		// Ustawienie początkowego stanu planszy
		int center = BoardSize / 2;

		SetPosition(center - 1, center - 1, 1); // Gracz 1 (czarny)
		SetPosition(center, center, 1);

		SetPosition(center - 1, center, -1); // Gracz 2 (biały)
		SetPosition(center, center - 1, -1);
		
	}

	public void SetPosition(int x, int y, int color)
	{
		_board[x, y] = color;
		UpdateColorCount();
	}

	public int GetPosition(int x, int y)
	{
		return _board[x, y];
	}

	public int GetBlackCount()
	{
		return _blackCount;
	}

	public int GetWhiteCount()
	{
		return _whiteCount;
	}
	
	public List<(int, int)> GetValidMoves(int color)
	{
		List<(int, int)> validMoves = new List<(int, int)>();

		for (int x = 0; x < BoardSize; x++)
		{
			for (int y = 0; y < BoardSize; y++)
			{
				if (_board[x, y] == 0 && IsValidMove(x, y, color))
				{
					validMoves.Add((x,y));
				}
			}
		}

		return validMoves;
	}
	
	public bool IsGameOver()
	{
		return GetValidMoves(1).Count == 0 && GetValidMoves(-1).Count == 0;
	}

	public bool IsValidMove(int x, int y, int color)
	{
		if (_board[x, y] != 0)
			return false;

		// Sprawdzenie, czy ruch jest możliwy w co najmniej jednym kierunku
		for (int dx = -1; dx <= 1; dx++)
		{
			for (int dy = -1; dy <= 1; dy++)
			{
				if (dx == 0 && dy == 0)
					continue;

				if (CheckDirection(x, y, dx, dy, color))
					return true;
			}
		}

		return false;
	}

	public bool MakeMove(int x, int y, int color)
	{
		if (!IsValidMove(x, y, color))
			return false;
			//throw new InvalidOperationException("Invalid move");

		_board[x, y] = color;

		// Odwrócenie pionków w kierunkach, w których jest to możliwe
		for (int dx = -1; dx <= 1; dx++)
		{
			for (int dy = -1; dy <= 1; dy++)
			{
				if (dx == 0 && dy == 0)
					continue;

				if(CheckDirection(x, y, dx, dy, color))
					FlipPiecesInDirection(x, y, dx, dy, color);
			}
		}

		// Aktualizacja liczników pionków
		UpdateColorCount();
		return true;
	}

	private bool CheckDirection(int x, int y, int dx, int dy, int color)
	{
		int opponentColor = -color;
		int nx = x + dx;
		int ny = y + dy;
		bool foundOpponent = false;

		// Przesunięcie w danym kierunku
		while (nx >= 0 && nx < BoardSize && ny >= 0 && ny < BoardSize && _board[nx, ny] == opponentColor)
		{
			nx += dx;
			ny += dy;
			foundOpponent = true;
		}

		// Sprawdzenie, czy doszliśmy do pustego pola końcowego lub poza planszą
		if (nx < 0 || nx >= BoardSize || ny < 0 || ny >= BoardSize || !foundOpponent)
			return false;

		// Sprawdzenie, czy ostatnie pole końcowe jest w kolorze gracza
		if (_board[nx, ny] == color)
			return true;

		return false;
	}

	private void FlipPiecesInDirection(int x, int y, int dx, int dy, int color)
	{
		int opponentColor = -color;
		int nx = x + dx;
		int ny = y + dy;

		// Przesunięcie w danym kierunku
		while (_board[nx, ny] == opponentColor)
		{
			_board[nx, ny] = color;
			nx += dx;
			ny += dy;
		}
	}

	private void UpdateColorCount()
	{
		_blackCount = 0;
		_whiteCount = 0;

		// Przeliczenie liczby pionków w każdym kolorze
		for (int i = 0; i < BoardSize; i++)
		{
			for (int j = 0; j < BoardSize; j++)
			{
				if (_board[i, j] == 1)
					_blackCount++;
				else if (_board[i, j] == -1)
					_whiteCount++;
			}
		}
	}
	
	public Board Clone()
	{
		Board cloneBoard = new Board();

		for (int i = 0; i < BoardSize; i++)
		{
			for (int j = 0; j < BoardSize; j++)
			{
				cloneBoard._board[i, j] = _board[i, j];
			}
		}
		
		cloneBoard.UpdateColorCount();

		return cloneBoard;
	}
}
