﻿namespace BalloonsPop.Core.Commands
{
    using BalloonsPop.Common.Contracts;

    /// <summary>
    /// Implements command to pop ballon.
    /// </summary>
    public class PopBalloonCommand : ICommand
    {
        /// <summary>
        /// The class constructor.
        /// </summary>
        public PopBalloonCommand() 
        {
        }

        /// <summary>
        /// Executes PopBalloonCommand.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IContext context)
        {
            context.LogicProvider.PopBalloons(context.Game.Field, context.UserRow, context.UserCol);
            context.LogicProvider.LetBalloonsFall(context.Game.Field);
            context.Game.IncrementMoves();
        }
    }
}