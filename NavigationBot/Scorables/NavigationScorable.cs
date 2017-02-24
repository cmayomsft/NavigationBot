using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Scorables.Internals;
using NavigationBot.Dialogs;
using System.Collections.Generic;
using NavigationBot.Properties;
using System.Linq;

namespace NavigationBot.Scorables
{
    public class NavigationScorable : ScorableBase<IActivity, string, double>
    {
        private readonly IDialogStack stack;

        public NavigationScorable(IDialogStack stack)
        {
            SetField.NotNull(out this.stack, nameof(stack), stack);
        } 

        protected override async Task<string> PrepareAsync(IActivity activity, CancellationToken token)
        {
            var message = activity as IMessageActivity;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                if (message.Text.Equals(Resources.NavigationMenu_Option, StringComparison.InvariantCultureIgnoreCase))
                {
                    return message.Text;
                }
            }

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

            if (message != null)
            {
                // Reset stack back to just RootDialog.
                this.stack.Reset();

                var dialog = new NavigationMenuDialog();

                this.stack.Call(dialog, null);
            }
        }
        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}