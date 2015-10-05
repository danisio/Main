﻿namespace BalloonsPop.GraphicUserInterface
{
    using Ninject;
    using Ninject.Modules;
    using BalloonsPop.Common.Contracts;
    using BalloonsPop.GraphicUserInterface.Contracts;

    public class WpfModule : NinjectModule
    {
        public WpfModule(IKernel kernel)
        {
            this.AppKernel = kernel;
        }

        public IKernel AppKernel { get; set; }

        public override void Load()
        {
            var window = new MainWindow();

            this.AppKernel.Bind<IEventBasedUserInterface>().ToMethod(ctx => window).InSingletonScope();
            this.AppKernel.Bind<IPrinter>().ToMethod(ctx => window).InSingletonScope();
        }
    }
}