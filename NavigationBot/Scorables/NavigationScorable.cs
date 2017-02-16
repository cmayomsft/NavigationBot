using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Scorables.Internals;
using NavigationBot.Dialogs;

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
                if (message.Text.Equals("Menu", StringComparison.InvariantCultureIgnoreCase) ||

                    message.Text.Equals("Topic 1", StringComparison.InvariantCultureIgnoreCase) ||

                    message.Text.Equals("Topic 1.1", StringComparison.InvariantCultureIgnoreCase) ||
                    message.Text.Equals("Topic 1.2", StringComparison.InvariantCultureIgnoreCase) ||
                    message.Text.Equals("Topic 1.3", StringComparison.InvariantCultureIgnoreCase) ||

                    message.Text.Equals("Topic 2", StringComparison.InvariantCultureIgnoreCase) ||

                    message.Text.Equals("Topic 2.1", StringComparison.InvariantCultureIgnoreCase) ||
                    message.Text.Equals("Topic 2.2", StringComparison.InvariantCultureIgnoreCase) ||
                    message.Text.Equals("Topic 2.3", StringComparison.InvariantCultureIgnoreCase) ||

                    message.Text.Equals("Topic 3", StringComparison.InvariantCultureIgnoreCase) ||

                    message.Text.Equals("Topic 3.1", StringComparison.InvariantCultureIgnoreCase) ||
                    message.Text.Equals("Topic 3.2", StringComparison.InvariantCultureIgnoreCase) ||
                    message.Text.Equals("Topic 3.3", StringComparison.InvariantCultureIgnoreCase))
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
                // Reset stack back to just including RootDialog.
                this.stack.Reset();

                // Forward navigation message/command to Root Dialog for processing (show menu, show dialog, etc.).
                var root = new RootDialog();

                await this.stack.Forward(root, null, message, token);
            }
        }
        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}