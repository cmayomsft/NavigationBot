using Autofac;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Scorables;
using Microsoft.Bot.Connector;
using NavigationBot.Dialogs;
using NavigationBot.Properties;
using NavigationBot.Scorables;

namespace NavigationBot
{
    public class NavigationBotModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .Register(c => BuildNavigationScorable(c))
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();
        }

        private NavigationScorable BuildNavigationScorable(IComponentContext c)
        {
            var stack = c.Resolve<IDialogStack>();
            var nav = new NavigationScorable();

            nav.NavigationCommands.Add(Resources.NavigationMenu_Option, () =>
            {
                stack.Reset();

                var dialog = new NavigationMenuDialog();

                stack.Call(dialog, null);
            });

            nav.NavigationCommands.Add(Resources.Topic1_Option, () =>
            {
                stack.Reset();

                var dialog = new Topic1Dialog();

                stack.Call(dialog, null);
            });

            nav.NavigationCommands.Add(Resources.Topic1_1_Option, () =>
            {
                stack.Reset();

                var dialog = new Topic1_1Dialog();

                stack.Call(dialog, null);
            });

            nav.NavigationCommands.Add(Resources.Topic1_2_Option, () =>
            {
                stack.Reset();

                var dialog = new Topic1_2Dialog();

                stack.Call(dialog, null);
            });

            nav.NavigationCommands.Add(Resources.Topic1_3_Option, () =>
            {
                stack.Reset();

                var dialog = new Topic1_3Dialog();

                stack.Call(dialog, null);
            });

            nav.NavigationCommands.Add(Resources.Topic2_Option, () =>
            {
                stack.Reset();

                var dialog = new Topic2Dialog();

                stack.Call(dialog, null);
            });

            nav.NavigationCommands.Add(Resources.Topic2_1_Option, () =>
            {
                stack.Reset();

                var dialog = new Topic2_1Dialog();

                stack.Call(dialog, null);
            });

            nav.NavigationCommands.Add(Resources.Topic2_2_Option, () =>
            {
                stack.Reset();

                var dialog = new Topic2_2Dialog();

                stack.Call(dialog, null);
            });

            nav.NavigationCommands.Add(Resources.Topic2_3_Option, () =>
            {
                stack.Reset();

                var dialog = new Topic2_3Dialog();

                stack.Call(dialog, null);
            });

            nav.NavigationCommands.Add(Resources.Topic3_Option, () =>
            {
                stack.Reset();

                var dialog = new Topic3Dialog();

                stack.Call(dialog, null);
            });

            nav.NavigationCommands.Add(Resources.Topic3_1_Option, () =>
            {
                stack.Reset();

                var dialog = new Topic3_1Dialog();

                stack.Call(dialog, null);
            });

            nav.NavigationCommands.Add(Resources.Topic3_2_Option, () =>
            {
                stack.Reset();

                var dialog = new Topic3_2Dialog();

                stack.Call(dialog, null);
            });

            nav.NavigationCommands.Add(Resources.Topic3_3_Option, () =>
            {
                stack.Reset();

                var dialog = new Topic3_3Dialog();

                stack.Call(dialog, null);
            });
        
            return nav;
        }
    }
}