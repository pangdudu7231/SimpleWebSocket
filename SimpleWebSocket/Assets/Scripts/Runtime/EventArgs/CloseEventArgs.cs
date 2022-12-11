using System;
using System.Net.WebSockets;

namespace SimpleWebSocket
{
    /// <summary>
    ///     websocket关闭时触发回调的参数
    /// </summary>
    public sealed class CloseEventArgs : EventArgs
    {
        #region private fields

        private readonly WebSocketCloseStatus _closeStatus;
        private readonly string _message;

        #endregion

        #region public properties

        public WebSocketCloseStatus CloseStatus => _closeStatus;

        public string Message => _message;

        #endregion

        #region ctor

        public CloseEventArgs(string message) : this(WebSocketCloseStatus.Empty, message)
        {
        }

        public CloseEventArgs(WebSocketCloseStatus closeStatus, string message)
        {
            _closeStatus = closeStatus;
            _message = message;
        }

        #endregion
    }
}