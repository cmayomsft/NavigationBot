using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;
using NavigationBot.Dialogs;
using Microsoft.Bot.Builder.Scorables;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System.Threading;
using System.Text.RegularExpressions;

namespace NavigationBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, CreateDecoratedRootDialog);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private IDialog<object> CreateDecoratedRootDialog()
        {
            var root = new RootDialog();

            var scorable = Actions
                .Bind(async (IDialogStack stack, IMessageActivity activity, CancellationToken token) =>
                {
                    stack.Reset();

                    var newRoot = new RootDialog();
                    await stack.Forward(newRoot, null, activity, token);
                })
                .When(new Regex(@"(?i)menu")) // Tried wildcard RegEx, @".*", to see if the issue was with matching, still didn't work.
                .Normalize();

            return root.WithScorable(scorable);
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}