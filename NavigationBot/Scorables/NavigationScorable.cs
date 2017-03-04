using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Builder.Dialogs;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 1998

namespace NavigationBot.Scorables
{
    public class NavigationScorable : ScorableBase<IActivity, string, double>
    {
        private Action currentNavAction = null;

        public Dictionary<string, Action> NavigationCommands { get;  }

        public NavigationScorable()
        {
            NavigationCommands = new Dictionary<string, Action>();
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