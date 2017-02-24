using Autofac;
using Autofac.Core;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Scorables;
using Microsoft.Bot.Connector;
using NavigationBot.Dialogs;
using NavigationBot.Properties;
using NavigationBot.Scorables;
using System.Threading;

namespace NavigationBot
{
    public class NavigationBotModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            // This does not resolve, throws an exception.

            var stack = Conversation.Container.Resolve<IDialogStack>();
            var navigationScorable = new NavigationScorable(stack);

            navigationScorable.NavigationCommands.Add(Resources.NavigationMenu_Option, () =>
                {
                    stack.Reset();

                    var dialog = new NavigationMenuDialog();

                    stack.Call(dialog, null);
                });


            /*},

            { Resources.Topic1_Option, () =>
                {
                    this.stack.Reset();

                    var dialog = new Topic1Dialog();

                    this.stack.Call(dialog, null);
                }
            },
            { Resources.Topic1_1_Option, () =>
                {
                    this.stack.Reset();

                    var dialog = new Topic1_1Dialog();

                    this.stack.Call(dialog, null);
                }
            },
            { Resources.Topic1_2_Option, () =>
                {
                    this.stack.Reset();

                    var dialog = new Topic1_2Dialog();

                    this.stack.Call(dialog, null);
                }
            },
            { Resources.Topic1_3_Option, () =>
                {
                    this.stack.Reset();

                    var dialog = new Topic1_3Dialog();

                    this.stack.Call(dialog, null);
                }
            },

            { Resources.Topic2_Option, () =>
                {
                    this.stack.Reset();

                    var dialog = new Topic2Dialog();

                    this.stack.Call(dialog, null);
                }
            },
            { Resources.Topic2_1_Option, () =>
                {
                    this.stack.Reset();

                    var dialog = new Topic2_1Dialog();

                    this.stack.Call(dialog, null);
                }
            },
            { Resources.Topic2_2_Option, () =>
                {
                    this.stack.Reset();

                    var dialog = new Topic2_2Dialog();

                    this.stack.Call(dialog, null);
                }
            },
            { Resources.Topic2_3_Option, () =>
                {
                    this.stack.Reset();

                    var dialog = new Topic2_3Dialog();

                    this.stack.Call(dialog, null);
                }
            },

            { Resources.Topic3_Option, () =>
                {
                    this.stack.Reset();

                    var dialog = new Topic3Dialog();

                    this.stack.Call(dialog, null);
                }
            },
            { Resources.Topic3_1_Option, () =>
                {
                    this.stack.Reset();

                    var dialog = new Topic3_1Dialog();

                    this.stack.Call(dialog, null);
                }
            },
            { Resources.Topic3_2_Option, () =>
                {
                    this.stack.Reset();

                    var dialog = new Topic3_2Dialog();

                    this.stack.Call(dialog, null);
                }
            },
            { Resources.Topic3_3_Option, () =>
                {
                    this.stack.Reset();

                    var dialog = new Topic3_3Dialog();

                    this.stack.Call(dialog, null);
                }
            },
        }; */

            builder
                .Register(c => navigationScorable)
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();
        }
    }
}