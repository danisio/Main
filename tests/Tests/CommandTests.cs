﻿namespace Tests
{
    using System;
    using BalloonsPop.Common.Contracts;
    using BalloonsPop.Engine.Commands;
    using BalloonsPop.Engine.Contexts;
    using Tests.MockClasses;


    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CommandTests
    {
        private ICommandFactory commandFactory;

        private IContext context;

        public CommandTests()
        {
            this.commandFactory = new CommandFactory();
        }

        [TestMethod]
        public void TestIfNonNullObjectIsReturnedWithValidCommandKey()
        {
            var commands = new string[] { "message", "exit", "undo", "pop", "restart", "field", "save", "top" };

            foreach (var key in commands)
            {
                var cmd = this.commandFactory.CreateCommand(key);

                Assert.AreNotEqual(null, cmd);
            }
        }

        [TestMethod]
        public void TestIfAnExceptionIsThrowWithInvalidCommandKeys()
        {
            var invalidKeys = new string[] { "jksdfjds", "redo", "quit", "pop_all", "cheat", "throw", "" };

            foreach (var key in invalidKeys)
            {
                try
                {
                    this.commandFactory.CreateCommand(key);
                }
                catch (ArgumentException)
                {
                    continue;
                }

                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestIfPopBalloonsCommandCallsTheNeededMethodsFromGameModelAndGameLogic()
        {
            var mockLogic = new MockLogic();
            var mockGame = new GameMock();

            var context = new Context() 
            {
                LogicProvider = mockLogic,
                Game = mockGame
            };

            var popCmd = this.commandFactory.CreateCommand("pop");

            popCmd.Execute(context);

            Assert.AreEqual(1, mockLogic.Calls["PopBalloons"]);
            Assert.AreEqual(1, mockLogic.Calls["LetBalloonsFall"]);
            Assert.AreEqual(1, mockGame.Calls["IncrementMoves"]);
        }

        [TestMethod]
        public void TestIfRestartCommandCallsTheNeededMethodsFromGameModelAndGameLogic()
        {
            var mockLogic = new MockLogic();
            var mockGame = new GameMock();

            var context = new Context() 
            {
                LogicProvider = mockLogic,
                Game = mockGame
            };

            var restartCmd = this.commandFactory.CreateCommand("restart");

            restartCmd.Execute(context);

            Assert.AreEqual(1, mockLogic.Calls["GenerateField"]);
            Assert.AreEqual(1, mockGame.Calls["ResetMoves"]);
        }

        [TestMethod]
        public void TestIfPrintFieldCommandCallsThePrintFieldMethodOfTheUI()
        {
            this.context = new Context() { Printer = new MockPrinter(), Game = new GameMock() };

            var printFieldCommand = this.commandFactory.CreateCommand("field");

            printFieldCommand.Execute(context);

            Assert.AreEqual(1, (this.context.Printer as MockPrinter).methodCallCounts["field"]);
        }

        [TestMethod]
        public void TestIfPrintHighscoreCommandCallsThePrintHighscoreMethodOfTheUI()
        {
            this.context = new Context() { Printer = new MockPrinter(), Game = new GameMock() };

            var printHighscoreCommand = this.commandFactory.CreateCommand("top");

            printHighscoreCommand.Execute(context);

            Assert.AreEqual(1, (this.context.Printer as MockPrinter).methodCallCounts["highscore"]);
        }

        [TestMethod]
        public void TestIfPrintMessageCommandCallsThePrintMessageMethodOfTheUI()
        {
            this.context = new Context() { Printer = new MockPrinter(), Game = new GameMock() };

            var printMessageCommand = this.commandFactory.CreateCommand("message");

            printMessageCommand.Execute(this.context);

            Assert.AreEqual(1, (this.context.Printer as MockPrinter).methodCallCounts["message"]);
        }

        [TestMethod]
        public void TestIfSaveCommandUsesTheMementoSetter()
        {
            this.context = new Context() { Game = new GameMock(), Memento = new MementoMock()};

            var saveCommand = this.commandFactory.CreateCommand("save");

            saveCommand.Execute(this.context);

            Assert.AreEqual(1, (this.context.Memento as MementoMock).CallsToSetCount);
        }
    }
}