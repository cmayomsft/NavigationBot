using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NavigationBot.Dialogs
{
    [Serializable]
    public class NavigationMenuDialog : IDialog<object>
    {
        private const string Topic1_1Option = "Topic 1.1";
        private const string Topic1_2Option = "Topic 1.2";
        private const string Topic1_3Option = "Topic 1.3";

        public async Task StartAsync(IDialogContext context)
        {
            await this.ShowMenuAsync(context);
        }

        private async Task ShowMenuAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            reply.Attachments.Add(CreateHeroCardAttachment(Topic1_1Option));
            reply.Attachments.Add(CreateHeroCardAttachment(Topic1_2Option));
            reply.Attachments.Add(CreateHeroCardAttachment(Topic1_3Option));

            await context.PostAsync(reply);

            context.Wait(this.ShowMenuResumeAfterAsync);
        }

        private async Task ShowMenuResumeAfterAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            // Everything in dialog is navigation, so anything returned here not picked up by global message handler is not understood.
            // Should implement retries here.
            await this.StartOverAsync(context, $"I'm sorry, I don't understand '{ message.Text }'.");
        }

        private Attachment CreateHeroCardAttachment(string title)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Text = $"To learn more about { title }...",

                Images = new List<CardImage>() {
                        new CardImage($"https://placeholdit.imgix.net/~text?txtsize=28&txt={ HttpContext.Current.Server.UrlEncode(title) }&w=320&h=160") },

                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, title, value: title)
                }
            };

            return heroCard.ToAttachment();
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