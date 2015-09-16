﻿namespace BalloonsPop.Engine.Memento
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    using BalloonsPop.Common.Contracts;

    public class Memento<T> : IMemento<T>
    {
        private T state;

        public Memento()
        {
        }

        public Memento(T currentState)
        {
            this.State = currentState;
        }

        public T State
        {
            get
            {
                return this.DeepClone(this.state);
            }
            set
            {
                this.state = this.DeepClone(value);
            }
        }

        private T DeepClone(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
