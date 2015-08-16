﻿namespace Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Contracts;
    using GameLogic;
    using Validations;

    [TestClass]
    public class GameLogicTests
    {
        GameLogic gameLogicProvider;

        public GameLogicTests()
        {
            gameLogicProvider = new GameLogic(ValidationProvider.Validator);
        }

        [TestMethod]
        public void TestIfGenerateFieldReturnAByteMatrix()
        {
            var field = gameLogicProvider.GenerateField();
            Assert.AreEqual(typeof(byte[,]), field.GetType());
        }

        [TestMethod]
        public void TestIfGenerateFieldRetunrsAByteMatrixOfCorrectSize()
        {
            var field = gameLogicProvider.GenerateField();

            Assert.AreEqual(5, field.GetLength(0));
            Assert.AreEqual(10, field.GetLength(1));
        }

        [TestMethod]
        public void TestIfGenerateFieldReturnsFieldThatAreSignificantlyDifferentFromEachOther()
        {
            var field1 = gameLogicProvider.GenerateField();
            var field2 = gameLogicProvider.GenerateField();

            var differenceCount = 0;

            for (int i = 0; i < field1.GetLength(0); i++)
            {
                for (int k = 0; k < field1.GetLength(1); k++)
                {
                    if (field1[i, k] != field2[i, k])
                    {
                        differenceCount++;
                    }
                }
            }

            var numberOfCells = field1.GetLength(0) * field1.GetLength(1);

            Assert.IsTrue(differenceCount > numberOfCells / 3);
        }

        [TestMethod]
        public void TestIfGenerateFieldInitializesTheFieldWithCorrectBaloonValues()
        {
            for (int i = 0; i < 10; i++)
            {
                var field = gameLogicProvider.GenerateField();

                foreach (var cell in field)
                {
                    if (cell <= 0 || 5 <= cell)
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [TestMethod]
        public void TestIfGameIsOverReturnsTrueWithAnEmptyField()
        {
            var sampleEmptyField = new byte[5, 10];
            Assert.IsTrue(gameLogicProvider.GameIsOver(sampleEmptyField));
        }

        [TestMethod]
        public void TestIfGameIsOverReturnsFalseWithANonemptyField()
        {
            var rng = new Random();

            for (int i = 0; i < 50; i++)
            {
                var sampleEmptyField = new byte[5, 10];
                sampleEmptyField[rng.Next(0, 5), rng.Next(0, 10)] = (byte)rng.Next(1, 5);

                Assert.IsFalse(gameLogicProvider.GameIsOver(sampleEmptyField));
            }
        }

        [TestMethod]
        public void TestIfGameIsOverReturnsFalseWithFullField()
        {
            var field = gameLogicProvider.GenerateField();

            Assert.IsFalse(gameLogicProvider.GameIsOver(field));
        }

        [TestMethod]
        public void TestIfPopBaloonsPopsTheBaloonsOnTheSameRowAndColumn()
        {
            var field = new byte[5, 10];

            for (int i = 0, j = 5; i < 5; i++)
            {
                field[i, j] = (byte)1;
            }

            for (int i = 0, j = 2; i < 10; i++)
            {
                field[j, i] = (byte)1;
            }

            gameLogicProvider.PopBaloons(field, 2, 5);

            foreach (var cell in field)
            {
                if(cell != 0)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void TestIfPopBaloonsPopsOnlyTheBaloonsOnTheSameRowAndColumn()
        {
            var field = gameLogicProvider.GenerateField();
            var storedField = (byte[,])field.Clone();

            for (int i = 0, j = 5; i < 5; i++)
            {
                field[i, j] = (byte)1;
            }

            for (int i = 0, j = 2; i < 10; i++)
            {
                field[j, i] = (byte)1;
            }

            gameLogicProvider.PopBaloons(field, 2, 5);

            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if(i == 2 || j == 5)
                    {
                        if(field[i, j] != 0)
                        {
                            Assert.Fail();
                        }
                    }
                    else
                    {
                        if(storedField[i, j] == 0)
                        {
                            Assert.Fail();
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void TestIfPopBaloonsPopsOnlyTargetBaloonWhenTheBaloonHasNoNeighborsOfTheSameType()
        {
            var field = new byte[5, 10];

            for (int i = 1; i < 4; i++)
            {
                for (int j = 2; j < 5; j++)
                {
                    field[i, j] = (byte)((i == 2 && j == 3) ? 1 : 2);
                }
            }

            gameLogicProvider.PopBaloons(field, 2, 3);

            for (int i = 1; i < 4; i++)
            {
                for (int j = 2; j < 5; j++)
                {
                    if(i != 2 && j != 3)
                    {
                        Assert.AreEqual(field[i, j], 2);
                    }
                }
            }

            Assert.AreEqual(field[2, 3], 0);
        }
    }
}