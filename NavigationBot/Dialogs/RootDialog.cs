using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Scorables;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System.Threading;
using System.Text.RegularExpressions;
using Autofac;
using Chronic;

namespace NavigationBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public RootDialog()
        {
            var scorable = Actions
                .Bind(async (string expression, IDialogStack stack, IMessageActivity activity, CancellationToken token) =>
                {
                    var dialog = new Topic1Dialog();
                    stack.Reset();
                    await stack.Forward(dialog, null, null, token);
                })
                .When(new Regex(@"(?gi)menu")) // Tried wildcard RegEx, @".*", to see if the issue was with matching, still didn't work.
                .Normalize();

            this.WithScorable(scorable);

            using (var container = Build(Options.ResolveDialogFromContainer))
            {
                var builder = new ContainerBuilder();
                builder
                    .RegisterInstance(this)
                    .As<IDialog<object>>();
                builder.Update(container);
            }
        }
        //delegate void NavigationDelegate(IDialogContext context, IMessageActivity message);

        //private Dictionary<string, NavigationDelegate> NavigationCommands = new Dictionary<string, NavigationDelegate> {
        //    { "Menu", async (c, m) => { await ShowMenuAsync(c); } },
        //    /*{ "Topic 1", "" },
        //    { "Topic 1.1", "" },
        //    { "Topic 1.2", "" },
        //    { "Topic 1.3", "" },
        //    { "Topic 2", "" },
        //    { "Topic 2.1", "" },
        //    { "Topic 2.2", "" },
        //    { "Topic 2.3", "" },
        //    { "Topic 3", "" },
        //    { "Topic 3.1", "" },
        //    { "Topic 3.2", "" },
        //    { "Topic 3.3", "" },*/
        //};

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                //if (message.Text.Equals(Menu, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    await this.ShowMenuAsync(context);
                //}
                //else if (message.Text.Equals(Topic1Option, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    context.Call(new Topic1Dialog(), this.TopicDialogResumeAfterAsync);
                //}

                //// Topic 1 Sub Topics
                //else if (message.Text.Equals(Topic1_1Option, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    context.Call(new Topic1_1Dialog(), this.TopicDialogResumeAfterAsync);
                //}
                //else if (message.Text.Equals(Topic1_2Option, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    context.Call(new Topic1_2Dialog(), this.TopicDialogResumeAfterAsync);
                //}
                //else if (message.Text.Equals(Topic1_3Option, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    context.Call(new Topic1_3Dialog(), this.TopicDialogResumeAfterAsync);
                //}
                

                //else if (message.Text.Equals(Topic2Option, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    context.Call(new Topic2Dialog(), this.TopicDialogResumeAfterAsync);
                //}

                //// Topic 2 Sub Topics
                //else if (message.Text.Equals(Topic2_1Option, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    context.Call(new Topic2_1Dialog(), this.TopicDialogResumeAfterAsync);
                //}
                //else if (message.Text.Equals(Topic2_2Option, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    context.Call(new Topic2_2Dialog(), this.TopicDialogResumeAfterAsync);
                //}
                //else if (message.Text.Equals(Topic2_3Option, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    context.Call(new Topic2_3Dialog(), this.TopicDialogResumeAfterAsync);
                //}

                //else if (message.Text.Equals(Topic3Option, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    context.Call(new Topic3Dialog(), this.TopicDialogResumeAfterAsync);
                //}

                //// Topic 3 Sub Topics
                //else if (message.Text.Equals(Topic3_1Option, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    context.Call(new Topic3_1Dialog(), this.TopicDialogResumeAfterAsync);
                //}
                //else if (message.Text.Equals(Topic3_2Option, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    context.Call(new Topic3_2Dialog(), this.TopicDialogResumeAfterAsync);
                //}
                //else if (message.Text.Equals(Topic3_3Option, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    context.Call(new Topic3_3Dialog(), this.TopicDialogResumeAfterAsync);
                //}

                //else
                //{
                //    // Needed for now, will be replaced with welcome message feature.
                //    await this.ShowMenuAsync(context);
                //}
            }
        }
        
        private async Task ShowMenuAsync(IDialogContext context)
        {
            //var reply = context.MakeMessage();

            //await context.PostAsync("Choose an option below:");

            //var menuHeroCard = new HeroCard
            //{
            //    Buttons = new List<CardAction>
            //    {
            //        new CardAction(ActionTypes.ImBack, Topic1Option, value: Topic1Option),
            //        new CardAction(ActionTypes.ImBack, Topic2Option, value: Topic2Option),
            //        new CardAction(ActionTypes.ImBack, Topic3Option, value: Topic3Option)
            //    }
            //};

            //reply.Attachments.Add(menuHeroCard.ToAttachment());

            //await context.PostAsync(reply);

            //context.Wait(this.ShowMenuResumeAfterAsync);
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