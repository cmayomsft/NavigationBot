using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace NavigationBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        // Will centralize navigation around array of commands, dialogs for each command.
        //  public static readonly string[] NavigationCommands = { "Menu" };

        private const string Menu = "Menu";
        private const string Topic1Option = "Topic 1";

        private const string Topic1_1Option = "Topic 1.1";
        private const string Topic1_2Option = "Topic 1.2";
        private const string Topic1_3Option = "Topic 1.3";

        private const string Topic2Option = "Topic 2";

        private const string Topic2_1Option = "Topic 2.1";
        private const string Topic2_2Option = "Topic 2.2";
        private const string Topic2_3Option = "Topic 2.3";

        private const string Topic3Option = "Topic 3";

        private const string Topic3_1Option = "Topic 3.1";
        private const string Topic3_2Option = "Topic 3.2";
        private const string Topic3_3Option = "Topic 3.3";

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                if (message.Text.Equals(Menu, StringComparison.InvariantCultureIgnoreCase))
                {
                    await this.ShowMenuAsync(context);
                }
                else if (message.Text.Equals(Topic1Option, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Call(new Topic1Dialog(), this.TopicDialogResumeAfterAsync);
                }

                // Topic 1 Sub Topics
                else if (message.Text.Equals(Topic1_1Option, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Call(new Topic1_1Dialog(), this.TopicDialogResumeAfterAsync);
                }
                else if (message.Text.Equals(Topic1_2Option, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Call(new Topic1_2Dialog(), this.TopicDialogResumeAfterAsync);
                }
                else if (message.Text.Equals(Topic1_3Option, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Call(new Topic1_3Dialog(), this.TopicDialogResumeAfterAsync);
                }
                

                else if (message.Text.Equals(Topic2Option, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Call(new Topic2Dialog(), this.TopicDialogResumeAfterAsync);
                }

                // Topic 2 Sub Topics
                else if (message.Text.Equals(Topic2_1Option, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Call(new Topic2_1Dialog(), this.TopicDialogResumeAfterAsync);
                }
                else if (message.Text.Equals(Topic2_2Option, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Call(new Topic2_2Dialog(), this.TopicDialogResumeAfterAsync);
                }
                else if (message.Text.Equals(Topic2_3Option, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Call(new Topic2_3Dialog(), this.TopicDialogResumeAfterAsync);
                }

                else if (message.Text.Equals(Topic3Option, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Call(new Topic3Dialog(), this.TopicDialogResumeAfterAsync);
                }

                // Topic 3 Sub Topics
                else if (message.Text.Equals(Topic3_1Option, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Call(new Topic3_1Dialog(), this.TopicDialogResumeAfterAsync);
                }
                else if (message.Text.Equals(Topic3_2Option, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Call(new Topic3_2Dialog(), this.TopicDialogResumeAfterAsync);
                }
                else if (message.Text.Equals(Topic3_3Option, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Call(new Topic3_3Dialog(), this.TopicDialogResumeAfterAsync);
                }

                else
                {
                    // Needed for now, will be replaced with welcome message feature.
                    await this.ShowMenuAsync(context);
                }
            }
        }
        
        private async Task ShowMenuAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();

            await context.PostAsync("Choose an option below:");

            var menuHeroCard = new HeroCard
            {
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, Topic1Option, value: Topic1Option),
                    new CardAction(ActionTypes.ImBack, Topic2Option, value: Topic2Option),
                    new CardAction(ActionTypes.ImBack, Topic3Option, value: Topic3Option)
                }
            };

            reply.Attachments.Add(menuHeroCard.ToAttachment());

            await context.PostAsync(reply);

            context.Wait(this.ShowMenuResumeAfterAsync);
        }

        private async Task ShowMenuResumeAfterAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            // If we got here, it's because something other than a navigation command happened, which isn't supported on RootDialog.
            await this.StartOverAsync(context, $"I'm sorry, I don't understand '{ message.Text }'.");
        }

        private async Task TopicDialogResumeAfterAsync(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var dialogResult = await result;

                await this.ShowMenuAsync(context);
            }
            catch (Exception)
            {
                await this.ShowMenuAsync(context);
            }
        }

        private async Task StartOverAsync(IDialogContext context, string text)
        {
            var message = context.MakeMessage();
            message.Text = text;
            await this.StartOverAsync(context, message);
        }

        private async Task StartOverAsync(IDialogContext context, IMessageActivity message)
        {
            await context.PostAsync(message);
            await this.ShowMenuAsync(context);
        }
    }
}