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

            await this.ShowMenuAsync(context);
        }
        
        private async Task ShowMenuAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();

            await context.PostAsync("Choose an option below:");

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

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

            if (message.Text == Topic1Option)
            {
                context.Call(new Topic1Dialog(), this.Topic1DialogResumeAfterAsync);
            }
            else if (message.Text == Topic2Option)
            {
                await this.StartOverAsync(context, $"'{ message.Text }' isn't ready yet.");
            }
            else if (message.Text == Topic3Option)
            {
                await this.StartOverAsync(context, $"'{ message.Text }' isn't ready yet.");
            }
            else
            {
                await this.StartOverAsync(context, $"I'm sorry, but I don't understand '{ message.Text }'.");
            }
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