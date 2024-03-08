using Microsoft.AspNetCore.SignalR;

namespace PROJECT_PRN221
{
    public class HubServer : Hub
    {
        public void RefreshData()
        {
            Clients.All.SendAsync("ReloadData");
        }
    }
}
