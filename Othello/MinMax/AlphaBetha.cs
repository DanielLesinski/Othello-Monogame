using System;
using System.Collections.Generic;
using System.Linq;
using Othello.MinMax;

namespace Othello;

public class AlphaBeta
{
	private int _player;
	private Evaluation _evaluation;

	public (int, int) AlphaBetaSearch(Board state, int player, int maxDepth)
	{
		_player = player;
		int alpha = int.MinValue;
		int beta = int.MaxValue;
		int bestValue = int.MinValue;
		
		_evaluation = new Evaluation(_player);

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
			return _evaluation.Evaluate(state);
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
}