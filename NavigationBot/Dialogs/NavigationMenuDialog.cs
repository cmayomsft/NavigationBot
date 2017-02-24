using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using NavigationBot.Properties;

namespace NavigationBot.Dialogs
{
    [Serializable]
    public class NavigationMenuDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await this.ShowMenuAsync(context);
        }

        private async Task ShowMenuAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();

            var menuHeroCard = new HeroCard
            {
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, Resources.Topic1_Option, value: Resources.Topic1_Option),
                    new CardAction(ActionTypes.ImBack, Resources.Topic2_Option, value: Resources.Topic2_Option),
                    new CardAction(ActionTypes.ImBack, Resources.Topic3_Option, value: Resources.Topic3_Option)
                }
            };
            reply.Attachments.Add(menuHeroCard.ToAttachment());

            await context.PostAsync("Choose an option below:");

            await context.PostAsync(reply);

            context.Wait(this.ShowMenuResumeAfterAsync);
        }

        private async Task ShowMenuResumeAfterAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            // If we got here, it's because something other than a navigation command happened, and this dialog only supports navigation commands.
            await this.StartOverAsync(context, $"I'm sorry, I don't understand '{ message.Text }'.");
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