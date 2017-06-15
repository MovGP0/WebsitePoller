using System;
using JetBrains.Annotations;
using WebsitePoller.Workflow;

namespace WebsitePoller
{
    public sealed class TownCrierFactory : ITownCrierFactory
    {
        [NotNull]
        private Func<ITownCrier> TownCrierFactoryMethod { get; }

        [NotNull]
        private IExecuteWorkFlowCommand Command { get; }

        public TownCrierFactory([NotNull]Func<ITownCrier> townCrierFactoryMethod, [NotNull]IExecuteWorkFlowCommand command)
        {
            TownCrierFactoryMethod = townCrierFactoryMethod;
            Command = command;
        }

        [NotNull]
        public ITownCrier Invoke()
        {
            var townCrier = TownCrierFactoryMethod();
            townCrier.Pray += ExecuteCommand;
            return townCrier;
        }

        private void ExecuteCommand(object sender, EventArgs e)
        {
            if (Command.CanExecute(e))
            {
                Command.Execute(e);
            }
        }
    }
}