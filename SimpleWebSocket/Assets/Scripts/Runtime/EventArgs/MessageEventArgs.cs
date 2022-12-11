using System;
using System.Net.WebSockets;

namespace SimpleWebSocket
{
    /// <summary>
    ///     websocket处理消息时触发回调的参数
    /// </summary>
    public sealed class MessageEventArgs : EventArgs
    {
        #region private fields

        private readonly WebSocketMessageType _messageType;
        private readonly byte[] _data;

        #endregion

        #region public properties

        public WebSocketMessageType MessageType => _messageType;

        public byte[] Data => _data;

        #endregion

        #region ctor

        public MessageEventArgs(WebSocketMessageType messageType, byte[] data)
        {
            _messageType = messageType;
            _data = data;
        }

        #endregion
    }
}