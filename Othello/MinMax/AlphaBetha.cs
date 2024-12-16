using System;
using System.Collections.Generic;
using System.Linq;

namespace Othello;

public class AlphaBeta
{
	private int _player;

	public (int, int) AlphaBetaSearch(Board state, int player, int maxDepth)
	{
		_player = player;
		int alpha = int.MinValue;
		int beta = int.MaxValue;
		int bestValue = int.MinValue;
		

		List<(int, int)> validMoves = state.GetValidMoves(player);
		(int, int) bestMove = validMoves.First();
		
		if(validMoves.Count != 1)
		{
			foreach (var move in validMoves)
			{
				Board nextState = state.Clone();
				nextState.MakeMove(move.Item1, move.Item2, player);
				int value = Minimax(nextState, maxDepth - 1, alpha, beta, false, -player);

				if (value > bestValue)
				{
					bestValue = value;
					bestMove = move;
				}
				
				alpha = Math.Max(alpha, bestValue);
			}
		}

		return bestMove;
	}

	private int Minimax(Board state, int depth, int alpha, int beta, bool maximizingPlayer, int player)
	{
		List<(int, int)> validMoves = state.GetValidMoves(player);
		
		if (depth == 0 || !validMoves.Any() || state.IsGameOver() )
		{
			return Evaluate(state);
		}

		if (maximizingPlayer)
		{
			int value = int.MinValue;
			foreach (var move in validMoves)
			{
				Board nextState = state.Clone();
				nextState.MakeMove(move.Item1, move.Item2, player);
				value = Math.Max(value, Minimax(nextState, depth - 1, alpha, beta, false, -player));
				alpha = Math.Max(alpha, value);
				if (beta <= alpha)
					break;
			}
			return value;
		}
		else
		{
			int value = int.MaxValue;
			foreach (var move in validMoves)
			{
				Board nextState = state.Clone();
				nextState.MakeMove(move.Item1, move.Item2, player);
				value = Math.Min(value, Minimax(nextState, depth - 1, alpha, beta, true, -player));
				beta = Math.Min(beta, value);
				if (beta <= alpha)
					break;
			}
			return value;
		}
	}

	private int Evaluate(Board board)
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