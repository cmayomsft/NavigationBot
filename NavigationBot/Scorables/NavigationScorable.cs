using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Scorables.Internals;
using NavigationBot.Dialogs;
using NavigationBot.Properties;
using Microsoft.Bot.Builder.Dialogs;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 1998

namespace NavigationBot.Scorables
{
    public class NavigationScorable : ScorableBase<IActivity, string, double>
    {
        private readonly IDialogStack stack;
        private Action currentNavAction;

        //private readonly Dictionary<string, Action> navigationCommands;
        public Dictionary<string, Action> NavigationCommands { get;  }

        public NavigationScorable(IDialogStack stack)
        {
            SetField.NotNull(out this.stack, nameof(stack), stack);
            currentNavAction = null;

            NavigationCommands = new Dictionary<string, Action>();

            /* navigationCommands = new Dictionary<string, Action>()
            {
                { Resources.NavigationMenu_Option, () => 
                    {
                        this.stack.Reset();

                        var dialog = new NavigationMenuDialog();

                        this.stack.Call(dialog, null);
                    }
                },

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
        } 

        protected override async Task<string> PrepareAsync(IActivity activity, CancellationToken token)
        {
            var message = activity as IMessageActivity;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                var commands = from command in NavigationCommands
                                 where message.Text.Equals(command.Key, StringComparison.InvariantCultureIgnoreCase)
                                 select command.Value;

                if (commands.Any())
                {
                    currentNavAction = commands.First();
                    return message.Text;
                }
            }

            currentNavAction = null;
            return null;
        }

        protected override bool HasScore(IActivity item, string state)
        {
            return state != null;
        }

        protected override double GetScore(IActivity item, string state)
        {
            return 1.0;
        }

        protected override async Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            var message = item as IMessageActivity;

            if (message != null && currentNavAction != null)
            {
                currentNavAction();
            }
        }
        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}