using System;
using System.Collections.Generic;

namespace Othello.MinMax;

public class Evaluation
{
	private int _player;
	public Evaluation(int player)
	{
		_player = player;
	}
	
	public int Evaluate(Board board)
	{
		int playerScore = 0;
		int opponentScore = 0;

		int[,] weights = new int[8, 8] {
			{ 100, -20, 10, 5, 5, 10, -20, 100 },
			{ -20, -50, -2, -2, -2, -2, -50, -20 },
			{ 10, -2, 1, 1, 1, 1, -2, 10 },
			{ 5, -2, 1, 0, 0, 1, -2, 5 },
			{ 5, -2, 1, 0, 0, 1, -2, 5 },
			{ 10, -2, 1, 1, 1, 1, -2, 10 },
			{ -20, -50, -2, -2, -2, -2, -50, -20 },
			{ 100, -20, 10, 5, 5, 10, -20, 100 }
		};
		
		for (int i = 0; i < board.BoardSize; i++)
		{
			for (int j = 0; j < board.BoardSize; j++)
			{
				int position = board.GetPosition(i, j);
				if (position == _player)
					playerScore += weights[i, j];
				else if (position == -_player)
					opponentScore += weights[i, j];
			}
		}

		//ilość pionków na planszy
		int playerPieceCount = GetPieceCount(board, _player);
		int opponentPieceCount = GetPieceCount(board, -_player);
		
		//dostępność ruchów
		int playerMobility = GetMobility(board, _player);
		int opponentMobility = GetMobility(board, -_player);
		
		//Kontrola rogów planszy
		int playerCorners = GetCorners(board, _player);
		int opponentCorners = GetCorners(board, -_player);
		
		//kontrola krawędzi
		int playerEdges = GetEdges(board, _player);
		int opponentEdges = GetEdges(board, -_player);
		
		int totalPlayerScore = playerScore + playerPieceCount + playerMobility + playerCorners + playerEdges;
		int totalOpponentScore = opponentScore + opponentPieceCount + opponentMobility + opponentCorners + opponentEdges;
		
		return totalPlayerScore - totalOpponentScore;
	}
	
	
	private int GetPieceCount(Board board, int player)
	{
		if(player == 1)
			return board.GetBlackCount();
		else
			return board.GetWhiteCount();
	}
	
	private int GetMobility(Board board, int player)
	{
		List<(int, int)> validMoves = board.GetValidMoves(player);
		return validMoves.Count;
	}
	
	private int GetCorners(Board board, int player)
	{
		int cornersControlled = 0;

		//lewy górny róg
		if (board.GetPosition(0, 0) == player)
			cornersControlled++;

		//prawy górny róg
		if (board.GetPosition(0, board.BoardSize - 1) == player)
			cornersControlled++;

		//lewy dolny róg
		if (board.GetPosition(board.BoardSize - 1, 0) == player)
			cornersControlled++;

		//prawy dolny róg
		if (board.GetPosition(board.BoardSize - 1, board.BoardSize - 1) == player)
			cornersControlled++;

		return cornersControlled;
	}
	
	private int GetEdges(Board board, int player)
	{
		int edgesControlled = 0;

		//górna krawędź
		for (int col = 0; col < board.BoardSize; col++)
		{
			if (board.GetPosition(0, col) == player)
			{
				edgesControlled++;
			}
		}

		//dolna krawędź
		for (int col = 0; col < board.BoardSize; col++)
		{
			if (board.GetPosition(board.BoardSize - 1, col) == player)
			{
				edgesControlled++;
			}
		}

		//lewa krawędź
		for (int row = 0; row < board.BoardSize; row++)
		{
			if (board.GetPosition(row, 0) == player)
			{
				edgesControlled++;
			}
		}

		//prawa krawędź
		for (int row = 0; row < board.BoardSize; row++)
		{
			if (board.GetPosition(row, board.BoardSize - 1) == player)
			{
				edgesControlled++;
			}
		}

		return edgesControlled;
	}
}
