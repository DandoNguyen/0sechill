using _0sechill.Data;
using _0sechill.Hubs.Interfaces;
using _0sechill.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace _0sechill.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MediaHub : Hub<IHubCustomClient>
    {
        private const string SYSTEM_NOTIFICATION_CONST = "System Notification";
        private readonly ApiDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public MediaHub(
            ApiDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        /// <summary>
        /// This is to get connection id
        /// </summary>
        /// <returns></returns>
        public string GetConnectionId() => Context.ConnectionId;

        /// <summary>
        /// this is to disconnect the user
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Clients.Caller.MediaNotify(from: SYSTEM_NOTIFICATION_CONST, Message: "Call Ended", signal: "");
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// this is to connect the user
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            Clients.Caller.MediaNotify(Message: "Call Started", signal: "", from: SYSTEM_NOTIFICATION_CONST);
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// this is to call other user
        /// </summary>
        /// <param name="receiverEmail"></param>
        /// <param name="signal"></param>
        /// <returns></returns>
        [HubMethodName("CallUser")]
        public async Task CallUserAsync(string receiverEmail, string signal)
        {
            var receiver = await userManager.FindByEmailAsync(receiverEmail);
            var receiverConnectionId = receiver.currentHubConnectionId;
            if (string.IsNullOrEmpty(receiverConnectionId))
            {
                await Clients.Caller.MediaNotify(from: SYSTEM_NOTIFICATION_CONST, Message: "User Offline", signal: "");
            }
            var caller = await userManager.FindByIdAsync(Context.UserIdentifier);
            await Clients.Client(receiverConnectionId).MediaNotify(caller.Email, signal: signal, Message: $"Call from {caller.UserName}");
        }

        /// <summary>
        /// this is to answer call
        /// </summary>
        /// <returns></returns>
        [HubMethodName("AnswerCall")]
        public async Task AnswerCallAsync(string callerEmail, string signal)
        {
            var caller = await userManager.FindByEmailAsync(callerEmail);
            var callerConnectionId = caller.currentHubConnectionId;
            if (string.IsNullOrEmpty(callerConnectionId))
            {
                await Clients.Caller.MediaNotify(
                    from: SYSTEM_NOTIFICATION_CONST, 
                    Message: "System couldn't connect\nContact Tech Support (loz Dung)", 
                    signal: "");
            }
            var receiver = await userManager.FindByIdAsync(Context.UserIdentifier);
            await Clients.Client(callerConnectionId).MediaNotify(
                from: receiver.Email, signal: signal, Message: "Call Accepted!");
        }
    }
}
