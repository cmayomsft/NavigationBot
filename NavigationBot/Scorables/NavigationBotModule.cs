using Autofac;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Scorables;
using Microsoft.Bot.Connector;
using NavigationBot.Scorables;
using System.Threading;

namespace NavigationBot
{
    public class NavigationBotModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //What is c? Can I use it to add all commands here, in one place?

            builder
                .Register(c => new NavigationScorable(c.Resolve<IDialogStack>()))
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();
        }
    }
}