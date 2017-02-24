using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using NavigationBot.Properties;

#pragma warning disable 1998

namespace NavigationBot.Dialogs
{
    [Serializable]
    public class Topic1_3Dialog : IDialog<object>
    {
        private const string more = "more";

        public async Task StartAsync(IDialogContext context)
        {
            PromptDialog.Choice(context, FirstPromptResumeAfter, new[] { more }, "Topic 1.3 Dialog dialog text...",
                "I'm sorry, I don't understand. Please try again.");
        }

        private async Task FirstPromptResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var choice = await result;

                if (choice == more)
                {
                    PromptDialog.Choice(context, SecondPromptResumeAfter, new[] { more }, "Topic 1.3 Dialog second dialog text...",
                        "I'm sorry, I don't understand. Please try again.");
                }
            }
            catch (TooManyAttemptsException)
            {
                context.Fail(new TooManyAttemptsException("Too many attempts."));
            }
        }

        private async Task SecondPromptResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var choice = await result;

                if (choice == more)
                {
                    PromptDialog.Choice(context, ThirdPromptResumeAfter, new[] { more }, "Topic 1.3 Dialog third dialog text...",
                        "I'm sorry, I don't understand. Please try again.");
                }
            }
            catch (TooManyAttemptsException)
            {
                context.Fail(new TooManyAttemptsException("Too many attempts."));
            }
        }

        private async Task ThirdPromptResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var choice = await result;

                if (choice == more)
                {
                    PromptDialog.Choice(context, FourthPromptResumeAfter, new[] { Resources.NavigationMenu_Option, Resources.Topic1_Option }, "Topic 1.3 Dialog is done. What do you want to do next?...",
                        "I'm sorry, I don't understand. Please try again.");
                }
            }
            catch (TooManyAttemptsException)
            {
                context.Fail(new TooManyAttemptsException("Too many attempts."));
            }
        }

        private async Task FourthPromptResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var choice = await result;

                if (choice == Resources.NavigationMenu_Option || choice == Resources.Topic1_Option)
                {
                    context.Done<object>(null);
                }
            }
            catch (TooManyAttemptsException)
            {
                context.Fail(new TooManyAttemptsException("Too many attempts."));
            }
        }
    }
}