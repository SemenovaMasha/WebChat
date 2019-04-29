using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SignalR_Identity.Services
{
    public class RemoveOldMessagesService:IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _provider;

        public RemoveOldMessagesService(IServiceProvider serviceProvider)
        {
            _provider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(RemoveOldMessages, null, TimeSpan.Zero,
                TimeSpan.FromHours(1));
            //TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        private void RemoveOldMessages(object state)
        {
            using (IServiceScope scope = _provider.CreateScope())
            {
                MessageService MessageService = scope.ServiceProvider.GetRequiredService<MessageService>();
                MessageService.RemoveOldMessages();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
