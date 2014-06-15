using Bebbs.Harmonize.With.Component;
using Bebbs.Harmonize.With.Message;
using Bebbs.Harmonize.With.Messaging.Client;
using Bebbs.Harmonize.With.Messaging.Via.SignalR.Client;
using Caliburn.Micro;
using Symphonize.For.Windows.Store.Xaml;
using System;
using System.Reactive.Subjects;
using System.Windows.Input;

namespace Symphonize.For.Windows.Store.ViewModels
{
    public class MainViewModel : Screen
    {
        private static readonly IIdentity Identity = new Identity(Guid.NewGuid().ToString());

        private string _connectionString = "http://localhost:8999";
        private string _hubName = "HarmonizeHub";
        private Subject<IMessage> _consumer;

        private DelegateCommand _connect;

        public MainViewModel()
        {
            _consumer = new Subject<IMessage>();
            _connect = new DelegateCommand(ExecuteConnect);
        }

        private async void ExecuteConnect()
        {
            IEndpoint endpoint = Factory.Default.Create(_connectionString, _hubName);
            await endpoint.Initialize();

            Entity entity = new Entity(new Identity("Test"), null, null, null);

            endpoint.Register(Identity, entity, _consumer);
        }

        public ICommand ConnectCommand
        {
            get { return _connect; }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                if (value != _connectionString)
                {
                    _connectionString = value;

                    NotifyOfPropertyChange(() => ConnectionString);
                }
            }
        }        

        public string HubName
        {
            get { return _hubName; }
            set
            {
                if (value != _hubName)
                {
                    _hubName = value;

                    NotifyOfPropertyChange(() => ConnectionString);
                }
            }
        }
    }
}
