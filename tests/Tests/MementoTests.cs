﻿namespace Tests
{
    using System;
    using BalloonsPop.Common.Validators;
    using BalloonsPop.Engine;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using BalloonsPop.Engine.Memento;
    using BalloonsPop.Common.Contracts;
    using BalloonsPop.Common.Gadgets;
    using System.Linq;
    using Tests.MockClasses;
    using System.Runtime.Serialization;

    [TestClass]
    public class MementoTests
    {
        private readonly IStateSaver<IGameModel> memento = new Saver<IGameModel>();
        private readonly IGameLogicProvider logic = new GameLogic(MatrixValidator.GetInstance);

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestIfMementoStateThrowsExceptionWhenNoStateIsProvidedButAStateIsRequeste()
        {
            var state = this.memento.GetState();
        }

        [TestMethod]
        public void TestIfMementoReturnsTheSameStateItAccepter()
        {
            var game = new Game(logic.GenerateField());

            this.memento.SaveState(game);

            var stateFromMemento = this.memento.GetState();

            var areEqual = new QueriableMatrix<IBalloon>(game.Field)
                                .Join(new QueriableMatrix<IBalloon>(stateFromMemento.Field), x => x, y => y, (x, y) => (x.isPopped == y.isPopped) && (x.Number == y.Number))
                                .All(x => x);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestIfConstructorWithParametersHasTheSameBehaviorAsTheSetter()
        {
            var game = new Game(logic.GenerateField());

            this.memento.SaveState(game);
            var memento2 = new Saver<IGameModel>();
            memento2.SaveState(game);

            var stateFromMemento = this.memento.GetState();

            var areEqual = new QueriableMatrix<IBalloon>(memento2.GetState().Field)
                                .Join(new QueriableMatrix<IBalloon>(stateFromMemento.Field), x => x, y => y, (x, y) => (x.isPopped == y.isPopped) && (x.Number == y.Number))
                                .All(x => x);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestIfMementoProvidesDeepCopy()
        {
            var game = new Game(logic.GenerateField());

            this.memento.SaveState(game);

            var stateFromMemento = this.memento.GetState();

            game.Field[0, 0].isPopped = true;

            Assert.IsFalse(ReferenceEquals(game.Field, stateFromMemento.Field));

            bool areEqual = true;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if(game.Field[i,j].isPopped != stateFromMemento.Field[i, j].isPopped)
                    {
                        areEqual = false;
                    }
                }
            }

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void TestIfGetterEncapsulatesTheCurrentState()
        {
            var game = new Game(this.logic.GenerateField());

            this.memento.SaveState(game);
            var state = this.memento.GetState();

            state.Field[0, 0].Number = 99;

            Assert.AreNotEqual(game.Field[0, 0].Number, 99);

            var moves = state.UserMovesCount;

            game.IncrementMoves();
            var moves2 = game.UserMovesCount;

            Assert.AreNotEqual(moves, moves2);

        }
    }
}