using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BLL.Hubs
{
    public class CategoryHub : Hub
    {
        public async Task SendUpdate()
        {
            await Clients.All.SendAsync("ReceiveCategoryUpdate");
        }
    }
}
