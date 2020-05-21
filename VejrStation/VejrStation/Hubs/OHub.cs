using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VejrStation.Hubs
{
    public class OHub : Hub
    {
        public async Task observationUpdate(string temp)
        {
            await Clients.All.SendAsync("recieveObservation",temp);
            
        }
    }
}
