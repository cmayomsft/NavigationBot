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
        private const string Menu = "Menu";
        private const string Topic1Option = "Topic 1";
        private const string Topic2Option = "Topic 2";
        private const string Topic3Option = "Topic 3";

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
                    context.Call(new Topic1Dialog(), this.Topic1DialogResumeAfterAsync);
                }
                else if (message.Text.Equals(Topic2Option, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Call(new Topic2Dialog(), this.Topic2DialogResumeAfterAsync);
                }
                else if (message.Text.Equals(Topic3Option, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Call(new Topic3Dialog(), this.Topic3DialogResumeAfterAsync);
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

            // If we got here, it's because something other than a navigation command happened, which isn't possible on RootDialog.
            await context.PostAsync($"I'm sorry, I don't understand '{ message.Text }'.");

            await this.ShowMenuAsync(context);
        }

        private async Task Topic1DialogResumeAfterAsync(IDialogContext context, IAwaitable<object> result)
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

        private async Task Topic2DialogResumeAfterAsync(IDialogContext context, IAwaitable<object> result)
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

        private async Task Topic3DialogResumeAfterAsync(IDialogContext context, IAwaitable<object> result)
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