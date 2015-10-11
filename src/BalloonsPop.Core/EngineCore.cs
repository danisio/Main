﻿namespace BalloonsPop.Core
{
    using System;

    using BalloonsPop.Common.Contracts;
    using BalloonsPop.Common.Gadgets;

    public class EngineCore
    {
        #region Constants
        protected const string WrongInputMessage = "Wrong input! Try Again!";
        protected const string Save = "save";
        protected const string Pop = "pop";
        protected const string GameOver = "gameover";
        protected const string Field = "field";
        protected const string Message = "message";

        protected const int IndexOfRowDigit = 0;
        protected const int IndexOfColumnDigit = 2;
        #endregion

        private IUserInputValidator validator;

        private ICommandFactory commandFactory;

        private IContext context;

        private ILogger logger;

        protected EngineCore(IContext ctx, IUserInputValidator inputValidator, ICommandFactory cmdFactory, ILogger logger)
        {
            this.Context = ctx;
            this.Validator = inputValidator;
            this.commandFactory = cmdFactory;
            this.Context.LogicProvider.RandomizeBalloonField(this.Context.Game.Field);
            this.logger = logger;
        }

        protected IContext Context
        {
            get
            {
                return this.context;
            }

            set
            {
                this.context = value;
            }
        }

        protected ICommandFactory CommandFactory
        {
            get
            {
                return this.commandFactory;
            }

            set
            {
                this.commandFactory = value;
            }
        }

        protected IUserInputValidator Validator
        {
            get
            {
                return this.validator;
            }

            set
            {
                this.validator = value;
            }
        }

        protected virtual ICommand GetCommandList(string userCommand)
        {
            ICommand cmd = null;

            bool isValidPopMove = this.validator.IsValidUserMove(userCommand)
                                    && !this.Context.Game.Field[userCommand[0].ToInt32(), userCommand[2].ToInt32()].IsPopped;

            try
            {
                if (isValidPopMove)
                {
                    this.context.UserRow = userCommand[IndexOfRowDigit].ToInt32();
                    this.context.UserCol = userCommand[IndexOfColumnDigit].ToInt32();
                    cmd = this.commandFactory.CreateCommand(Pop);


                }
                else if (this.CommandFactory.ContainsKey(userCommand.ToLower()))
                {
                    cmd = this.CommandFactory.CreateCommand(userCommand.ToLower());
                }
                else
                {
                    this.context.Message = WrongInputMessage;
                    cmd = this.commandFactory.CreateCommand(Message);
                }
            }
            catch (ArgumentException e)
            {
                var errorLog = string.Format("{0}{1}{2}{3}",
                                                 "The command factory failed to create the command."
                                                 , Environment.NewLine,
                                                 e.Message,
                                                 e.StackTrace);
                this.logger.Error(errorLog);

                throw e;
            }

            return cmd;
        }
    }
}