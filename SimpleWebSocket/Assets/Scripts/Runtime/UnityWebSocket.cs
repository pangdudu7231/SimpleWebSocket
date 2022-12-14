using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWebSocket
{
    /// <summary>
    ///     WebSocket类
    /// </summary>
    public sealed class UnityWebSocket : IDisposable
    {
        private const int RECEIVE_BUFF_SIZE = 1024; //接收数据buff的大小

        #region private fields

        private readonly string _address; //地址
        private readonly CancellationTokenSource _cts;
        private readonly ClientWebSocket _webSocket;
        private bool _isSending; //是否正在发送消息

        #endregion

        #region public events

        /// <summary>
        ///     websocket关闭时的回调事件
        /// </summary>
        public event EventHandler<CloseEventArgs> OnClose;

        /// <summary>
        ///     websocket发生异常时的回调事件
        /// </summary>
        public event EventHandler<ErrorEventArgs> OnError;

        /// <summary>
        ///     websocket收到消息时的回调事件
        /// </summary>
        public event EventHandler<MessageEventArgs> OnReceive;

        #endregion

        #region public properties

        /// <summary>
        ///     websocket是否打开了
        /// </summary>
        public bool IsOpen => _webSocket.State == WebSocketState.Open;

        /// <summary>
        ///     websocket是否关闭了
        /// </summary>
        public bool IsClosed => _webSocket.State == WebSocketState.Closed;

        #endregion

        #region ctor

        public UnityWebSocket(string address)
        {
            _address = address;
            _cts = new CancellationTokenSource();
            _webSocket = new ClientWebSocket();
        }

        #endregion

        #region public functions

        /// <summary>
        ///     异步连接websocket
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task ConnectAsync()
        {
            if (_webSocket.State == WebSocketState.Open)
                throw new InvalidOperationException("Web socket is opened.");

            var uri = new Uri(_address);
            await _webSocket.ConnectAsync(uri, _cts.Token);
            StartReceiveMessageAsync();
        }

        /// <summary>
        ///     异步发送消息
        /// </summary>
        /// <param name="dataList"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task SendAsync(params byte[][] dataList)
        {
            if (_webSocket.State != WebSocketState.Open)
                throw new InvalidOperationException("Web socket is not opened.");

            while (_isSending) await Task.Delay(200); //等待上一次数据发送完毕
            _isSending = true;
            try
            {
                foreach (var data in dataList)
                    await _webSocket.SendAsync(data, WebSocketMessageType.Binary, true, _cts.Token);
            }
            finally
            {
                _isSending = false;
            }
        }

        /// <summary>
        ///     异步发送消息
        /// </summary>
        /// <param name="messages"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task SendAsync(params string[] messages)
        {
            if (_webSocket.State != WebSocketState.Open)
                throw new InvalidOperationException("Web socket is not opened.");

            while (_isSending) await Task.Delay(200); //等待上一次数据发送完毕
            _isSending = true;
            try
            {
                foreach (var msg in messages)
                    await _webSocket.SendAsync(Encoding.UTF8.GetBytes(msg), WebSocketMessageType.Text, true,
                        _cts.Token);
            }
            finally
            {
                _isSending = false;
            }
        }

        /// <summary>
        ///     异步关闭
        /// </summary>
        public async Task CloseAsync()
        {
            if (_webSocket.State != WebSocketState.Open)
                throw new InvalidOperationException("Web socket is not opened.");

            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "NormalClosure", _cts.Token);
        }

        public void Dispose()
        {
            _cts?.Dispose();
            _webSocket?.Dispose();
        }

        #endregion

        #region private functions

        /// <summary>
        ///     开始接收消息
        /// </summary>
        private async void StartReceiveMessageAsync()
        {
            var buffer = new ArraySegment<byte>(new byte[RECEIVE_BUFF_SIZE]);
            var isClosed = false;
            using var stream = new MemoryStream();
            try
            {
                while (!isClosed)
                {
                    var result = await _webSocket.ReceiveAsync(buffer, _cts.Token);
                    await stream.WriteAsync(buffer.Array, buffer.Offset, result.Count);
                    if (!result.EndOfMessage) continue;

                    var data = stream.ToArray();
                    stream.SetLength(0); //清空
                    var messageType = result.MessageType;
                    switch (messageType)
                    {
                        case WebSocketMessageType.Text:
                        case WebSocketMessageType.Binary:
                            OnReceive?.Invoke(this, new MessageEventArgs(messageType, data));
                            break;
                        case WebSocketMessageType.Close:
                            isClosed = true;
                            var closeStatus = WebSocketCloseStatus.Empty;
                            if (result.CloseStatus.HasValue) closeStatus = result.CloseStatus.Value;
                            OnClose?.Invoke(this, new CloseEventArgs(closeStatus, result.CloseStatusDescription));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new ErrorEventArgs(ex));
                OnClose?.Invoke(this, new CloseEventArgs(WebSocketCloseStatus.NormalClosure, ex.Message));
            }
            finally
            {
                stream.Close();
                _cts.Cancel();
                Dispose();
            }
        }

        #endregion
    }
}