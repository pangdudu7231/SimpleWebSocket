namespace SimpleWebSocket
{
    /// <summary>
    ///     Log的基类
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        ///     断言条件
        /// </summary>
        /// <param name="condition"></param>
        void Assert(bool condition);

        /// <summary>
        ///     断言条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        void Assert(bool condition, object message);

        /// <summary>
        ///     断言条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void AssertFormat(bool condition, string format, params object[] args);

        /// <summary>
        ///     打印调试信息
        /// </summary>
        /// <param name="message"></param>
        void Log(object message);

        /// <summary>
        ///     打印调试信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void LogFormat(string format, params object[] args);

        /// <summary>
        ///     打印警告信息
        /// </summary>
        /// <param name="message"></param>
        void LogWarning(object message);

        /// <summary>
        ///     打印警告信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void LogWarningFormat(string format, params object[] args);

        /// <summary>
        ///     打印错误信息
        /// </summary>
        /// <param name="message"></param>
        void LogError(object message);

        /// <summary>
        ///     打印错误信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void LogErrorFormat(string format, params object[] args);
    }
}