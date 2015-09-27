﻿namespace BalloonsPop.GameModels
{
    using System;
    using BalloonsPop.Common.Contracts;
    using BalloonsPop.Common.Gadgets;
    using System.Linq;

    public class GameModel : IGameModel
    {
        private IBalloon[,] field;

        private int userMovesCount;

        public GameModel()
        {
            this.field = new IBalloon[5, 10];
            this.userMovesCount = 0;
        }

        public IBalloon[,] Field
        {
            get
            {
                return this.field;
            }

            set
            {
                // TODO: validations here, logging here
                this.field = value;
            }
        }

        public int UserMovesCount
        {
            get
            {
                return this.userMovesCount;
            }
        }

        public void ResetUserMoves()
        {
            this.userMovesCount = 0;
        }

        public void IncrementMoves()
        {
            this.userMovesCount++;
        }

        public IGameModel Clone()
        {
            var clonedField = new QueriableMatrix<IBalloon>(this.field)
                                        .Select(balloon => new Balloon() { Number = balloon.Number, IsPopped = balloon.IsPopped })
                                        .ToMatrix(this.field.GetLength(0), this.field.GetLength(1));

            return new GameModel() { field = clonedField };
        }
    }
}