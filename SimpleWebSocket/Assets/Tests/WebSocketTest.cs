using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SimpleWebSocket;

public class WebSocketTest
{
    private UnityWebSocket _webSocket;
    private Queue<string> _receiveStringsQueue;

    [SetUp]
    public async Task SetUp()
    {
        _webSocket = new UnityWebSocket("wss://echo.websocket.events/");
        _webSocket.OnClose += OnClose;
        _webSocket.OnError += OnError;
        _webSocket.OnReceive += OnReceive;
        _receiveStringsQueue = new Queue<string>();
        //开始时测试连接
        Assert.AreEqual(_webSocket.IsOpen, false);
        await _webSocket.ConnectAsync();
        Assert.AreEqual(_webSocket.IsOpen, true);
    }

    /// <summary>
    ///     测试异步发送消息
    /// </summary>
    [Test]
    public async Task SendAsyncTest()
    {
        var str = "Hello, unity web socket";
        await _webSocket.SendAsync(str);
        await Task.Delay(2000);
        Assert.Contains(str, _receiveStringsQueue);
        _receiveStringsQueue.Clear();
    }

    [TearDown]
    public async Task TearDown()
    {
        //结束时测试断开
        Assert.AreEqual(_webSocket.IsOpen, true);
        await _webSocket.CloseAsync();
        Assert.AreEqual(_webSocket.IsOpen, false);
        _webSocket.OnClose -= OnClose;
        _webSocket.OnError -= OnError;
        _webSocket.OnReceive -= OnReceive;
    }

    #region private functions

    private void OnClose(object sender, CloseEventArgs eventArgs)
    {
    }

    private void OnError(object sender, ErrorEventArgs eventArgs)
    {
    }

    private void OnReceive(object sender, MessageEventArgs eventArgs)
    {
        switch (eventArgs.MessageType)
        {
            case WebSocketMessageType.Text:
                var str = Encoding.UTF8.GetString(eventArgs.Data);
                _receiveStringsQueue.Enqueue(str);
                break;
            case WebSocketMessageType.Binary:
                break;
            case WebSocketMessageType.Close:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}