﻿namespace BaloonsPop.Engine.Commands
{
    public class PrintFieldCommand : PrintCommands
    {
        private byte[,] field;

        public PrintFieldCommand(IPrinter printer, byte[,] field)
            :base(printer)
        {
            this.field = field;
        }

        public override void Execute()
        {
            this.printer.PrintField(field);
        }
    }
}