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
    public class Topic1Dialog : IDialog<object>
    {
        private string[] subtopics = { "Topic 1.1", "Topic 1.2", "Topic 1.3" };

        public async Task StartAsync(IDialogContext context)
        {
            await this.ShowMenuAsync(context);
        }

        private async Task ShowMenuAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            
            for (int i = 0; i < subtopics.Length; i++)
            {
                var groupHeroCard = new HeroCard
                {
                    Title = subtopics[i],
                    Text = $"To learn more about { subtopics[i] }, click below...",

                    Images = new List<CardImage>() {
                        new CardImage($"https://placeholdit.imgix.net/~text?txtsize=28&txt={ subtopics[i] }%201&w=320&h=160") },

                    Buttons = new List<CardAction>
                    {
                        new CardAction(ActionTypes.ImBack, subtopics[i], value: subtopics[i])
                    }
                };

                reply.Attachments.Add(groupHeroCard.ToAttachment());
            }

            await context.PostAsync(reply);

            context.Wait(this.ShowMenuResumeAfterAsync);
        }

        private async Task ShowMenuResumeAfterAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            // Start here: Wire up retries. Wire up launching sub dialogs with "more".

            await this.StartOverAsync(context, $"'{ message.Text }' isn't ready yet.");
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