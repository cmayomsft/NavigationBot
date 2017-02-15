﻿using Microsoft.Bot.Builder.Dialogs;
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

        private async Task ShowMenuResumeAfterAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text == Topic1_1Option)
            {
                context.Call(new Topic1_1Dialog(), Topic1_1DialogResumeAfterAsync);
            }
            else if (message.Text == Topic1_2Option)
            {
                context.Call(new Topic1_2Dialog(), Topic1_2DialogResumeAfterAsync);
            }
            else if (message.Text == Topic1_3Option)
            {
                context.Call(new Topic1_3Dialog(), Topic1_3DialogResumeAfterAsync);
            }
            else
            {
                await this.StartOverAsync(context, $"I'm sorry, I don't understand '{ message.Text }'.");
            }
        }

        private async Task Topic1_3DialogResumeAfterAsync(IDialogContext context, IAwaitable<object> result)
        {
            await this.ShowMenuAsync(context);
        }

        private async Task Topic1_2DialogResumeAfterAsync(IDialogContext context, IAwaitable<object> result)
        {
            await this.ShowMenuAsync(context);
        }

        private async Task Topic1_1DialogResumeAfterAsync(IDialogContext context, IAwaitable<object> result)
        {
            await ShowMenuAsync(context);
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