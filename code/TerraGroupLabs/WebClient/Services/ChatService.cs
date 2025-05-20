// WebClient/Services/ChatService.cs
using SpacetimeDB;
using SpacetimeDB.Types;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WebClient.Services
{
    public class ChatService : IDisposable, INotifyPropertyChanged
    {
        private DbConnection? _conn;
        private Identity? _localIdentity;
        const string HOST = "http://localhost:3000"; // Or read from config
        const string DB_NAME = "tg-labs-chat";

        public ObservableCollection<MessageViewModel> Messages { get; } = new();
        public ObservableCollection<UserViewModel> Users { get; } = new();

        public bool IsConnected => _conn?.IsActive ?? false;
        public string? CurrentUsername { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action? ConnectionStateChanged;


        public ChatService()
        {
            // Initialize AuthToken. The SDK handles storage in browser's localStorage for WASM
            AuthToken.Init("TerraGroupLabsChat"); // Use a unique key for your app
        }

        public async Task ConnectAsync()
        {
            if (_conn != null && _conn.IsActive) return;

            _conn = DbConnection.Builder()
                .WithUri(HOST)
                .WithModuleName(DB_NAME)
                .WithToken(AuthToken.Token)
                .OnConnect(OnConnected)
                .OnConnectError(OnConnectError)
                .OnDisconnect(OnDisconnected)
                .Build();

            RegisterCallbacks(_conn);
            // In Blazor WASM, FrameTick is typically handled by the SDK's internal loop
            // or by JavaScript interop if necessary. For most cases, you don't call it manually like in the console app.
            // The SDK should manage its own processing.

            // We don't have a separate processing thread in Blazor WASM.
            // Callbacks will be invoked by the SDK.
        }

        private void OnConnected(DbConnection conn, Identity identity, string authToken)
        {
            _localIdentity = identity;
            AuthToken.SaveToken(authToken);

            Console.WriteLine($"Connected with identity: {identity}"); // Log to browser console

            conn.SubscriptionBuilder()
                .OnApplied(OnSubscriptionApplied)
                .SubscribeToAllTables(); // Or specific queries

            ConnectionStateChanged?.Invoke();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConnected)));
            UpdateCurrentUsername();
        }

        private void OnSubscriptionApplied(SubscriptionEventContext ctx)
        {
            Console.WriteLine("Subscription Applied. Loading initial data.");
            Messages.Clear();
            Users.Clear();

            foreach (var user in ctx.Db.User.Iter().OrderBy(u => u.Name ?? u.Identity.ToString()))
            {
                Users.Add(new UserViewModel(user));
            }
            foreach (var message in ctx.Db.Message.Iter().OrderBy(m => m.Sent))
            {
                AddMessageToList(ctx.Db, message);
            }
            UpdateCurrentUsername();
        }


        private void OnConnectError(Exception e)
        {
            Console.Error.WriteLine($"Error while connecting: {e}");
            ConnectionStateChanged?.Invoke();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConnected)));
        }

        private void OnDisconnected(DbConnection conn, Exception? e)
        {
            Console.WriteLine(e != null ? $"Disconnected abnormally: {e}" : "Disconnected normally.");
            _conn = null;
            _localIdentity = null;
            Messages.Clear(); // Clear data on disconnect
            Users.Clear();
            ConnectionStateChanged?.Invoke();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConnected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentUsername)));
        }

        private void RegisterCallbacks(DbConnection conn)
        {
            conn.Db.User.OnInsert += User_OnInsert;
            conn.Db.User.OnUpdate += User_OnUpdate;
            conn.Db.Message.OnInsert += Message_OnInsert;

            conn.Reducers.OnSetName += Reducer_OnSetNameEvent;
            conn.Reducers.OnSendMessage += Reducer_OnSendMessageEvent;
        }

        private void User_OnInsert(EventContext ctx, User insertedValue)
        {
            var existingUser = Users.FirstOrDefault(u => u.Identity == insertedValue.Identity);
            if (existingUser == null)
            {
                Users.Add(new UserViewModel(insertedValue));
                Console.WriteLine($"{UserNameOrIdentity(insertedValue)} is online (inserted)");
            }
            else // Should not happen if primary key is unique but good for safety
            {
                existingUser.UpdateFrom(insertedValue);
            }
            if (insertedValue.Identity == _localIdentity) UpdateCurrentUsername();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Users)));
        }

        private void User_OnUpdate(EventContext ctx, User oldValue, User newValue)
        {
            var userVM = Users.FirstOrDefault(u => u.Identity == newValue.Identity);
            if (userVM != null)
            {
                var oldName = UserNameOrIdentity(oldValue);
                userVM.UpdateFrom(newValue);
                if (oldValue.Name != newValue.Name)
                {
                    Console.WriteLine($"{oldName} renamed to {newValue.Name}");
                }
                if (oldValue.Online != newValue.Online)
                {
                    Console.WriteLine($"{UserNameOrIdentity(newValue)} is now {(newValue.Online ? "online" : "offline")}");
                }
            }
            else // User might have been created and updated in quick succession before insert was processed
            {
                Users.Add(new UserViewModel(newValue));
            }
            if (newValue.Identity == _localIdentity) UpdateCurrentUsername();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Users)));
        }

        private void Message_OnInsert(EventContext ctx, Message insertedValue)
        {
            if (ctx.Event is not Event<Reducer>.SubscribeApplied) // Avoid double-printing initial messages
            {
                AddMessageToList(ctx.Db, insertedValue);
            }
        }

        private void AddMessageToList(RemoteTables tables, Message message)
        {
            var sender = tables.User.Identity.Find(message.Sender);
            var senderName = sender != null ? UserNameOrIdentity(sender) : "unknown";
            Messages.Add(new MessageViewModel(message, senderName, message.Sender == _localIdentity));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Messages)));
        }


        private void Reducer_OnSetNameEvent(ReducerEventContext ctx, string name)
        {
            if (ctx.Event.CallerIdentity == _localIdentity && ctx.Event.Status is Status.Failed(var error))
            {
                Console.Error.WriteLine($"Failed to change name to {name}: {error}");
                // You'd typically show this error to the user in the UI
            }
            // If successful, User_OnUpdate will handle the name change display
        }

        private void Reducer_OnSendMessageEvent(ReducerEventContext ctx, string text)
        {
            if (ctx.Event.CallerIdentity == _localIdentity && ctx.Event.Status is Status.Failed(var error))
            {
                Console.Error.WriteLine($"Failed to send message {text}: {error}");
                // Show error to user
            }
        }

        public void SendMessage(string text)
        {
            if (_conn != null && _conn.IsActive && !string.IsNullOrWhiteSpace(text))
            {
                _conn.Reducers.SendMessage(text);
            }
        }

        public void SetName(string name)
        {
            if (_conn != null && _conn.IsActive && !string.IsNullOrWhiteSpace(name))
            {
                _conn.Reducers.SetName(name);
            }
        }

        private string UserNameOrIdentity(User user) => user.Name ?? user.Identity.ToString()[..8];
        private string UserNameOrIdentity(UserViewModel user) => user.Name ?? user.Identity.ToString()[..8];

        private void UpdateCurrentUsername()
        {
            var localUser = Users.FirstOrDefault(u => u.Identity == _localIdentity);
            CurrentUsername = localUser != null ? UserNameOrIdentity(localUser) : (_localIdentity?.ToString().Substring(0, 8) ?? "Connecting...");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentUsername)));
        }

        public void Dispose()
        {
            _conn?.Disconnect();
        }
    }

    // Helper ViewModels for Blazor data binding
    public class MessageViewModel
    {
        public SpacetimeDB.Timestamp Sent { get; }
        public string Text { get; }
        public string SenderName { get; }
        public bool IsLocalUser { get; }
        public string FormattedTimestamp => Sent.ToString().Substring(0, 19);


        public MessageViewModel(Message message, string senderName, bool isLocalUser)
        {
            Sent = message.Sent;
            Text = message.Text;
            SenderName = senderName;
            IsLocalUser = isLocalUser;
        }
    }

    public class UserViewModel : INotifyPropertyChanged
    {
        public SpacetimeDB.Identity Identity { get; }
        private string? _name;
        public string? Name
        {
            get => _name;
            set { _name = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name))); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName))); }
        }
        private bool _online;
        public bool Online
        {
            get => _online;
            set { _online = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Online))); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName))); }
        }

        public string DisplayName => Name ?? Identity.ToString()[..8];

        public event PropertyChangedEventHandler? PropertyChanged;

        public UserViewModel(User user)
        {
            Identity = user.Identity;
            UpdateFrom(user);
        }

        public void UpdateFrom(User user)
        {
            Name = user.Name;
            Online = user.Online;
        }
    }
}
