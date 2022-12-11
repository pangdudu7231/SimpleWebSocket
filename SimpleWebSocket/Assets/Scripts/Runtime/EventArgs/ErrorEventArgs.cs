using System;

namespace SimpleWebSocket
{
    /// <summary>
    ///     websocket发生异常时触发回调的参数
    /// </summary>
    public sealed class ErrorEventArgs : EventArgs
    {
        #region private fields

        private readonly string _message;

        #endregion

        #region public properties

        public string Message => _message;

        #endregion

        #region ctor

        public ErrorEventArgs(Exception exception)
        {
            _message = exception.Message;
        }

        public ErrorEventArgs(string message)
        {
            _message = message;
        }

        #endregion
    }
}