namespace SimpleWebSocket
{
    /// <summary>
    ///     日志的工具类
    /// </summary>
    public static class LoggerUtility
    {
        #region private static fields

        private static ILogger _logger; //日志打印类

        #endregion

        #region public static functions

        /// <summary>
        ///     设置日志打印类
        /// </summary>
        /// <param name="logger"></param>
        public static void SetLogger(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     断言条件
        /// </summary>
        /// <param name="condition"></param>
        public static void Assert(bool condition)
        {
            _logger?.Assert(condition);
        }

        /// <summary>
        ///     断言条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        public static void Assert(bool condition, object message)
        {
            _logger?.Assert(condition, message);
        }

        /// <summary>
        ///     断言条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void AssertFormat(bool condition, string format, params object[] args)
        {
            _logger.AssertFormat(condition, format, args);
        }

        /// <summary>
        ///     打印调试信息
        /// </summary>
        /// <param name="message"></param>
        public static void Log(object message)
        {
            _logger?.Log(message);
        }

        /// <summary>
        ///     打印调试信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void LogFormat(string format, params object[] args)
        {
            _logger?.LogFormat(format, args);
        }

        /// <summary>
        ///     打印警告信息
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning(object message)
        {
            _logger?.LogWarning(message);
        }

        /// <summary>
        ///     打印警告信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void LogWarningFormat(string format, params object[] args)
        {
            _logger?.LogWarningFormat(format, args);
        }

        /// <summary>
        ///     打印错误信息
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(object message)
        {
            _logger?.LogError(message);
        }

        /// <summary>
        ///     打印错误信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void LogErrorFormat(string format, params object[] args)
        {
            _logger?.LogErrorFormat(format, args);
        }

        #endregion
    }
}