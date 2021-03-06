﻿using Microsoft.Bot.Builder.Dialogs;
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

#pragma warning disable 1998

namespace NavigationBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            await this.ShowNavigationMenuAsync(context);
        }

        private async Task ShowNavigationMenuAsync(IDialogContext context)
        {
           context.Call(new NavigationMenuDialog(), ResumeAfterNavigationMenuDialog);
        }

        private async Task ResumeAfterNavigationMenuDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var dialogResult = await result;
            }
            catch (Exception)
            {
                
            }
            finally
            {
                await this.ShowNavigationMenuAsync(context);
            }
        }
    }
}