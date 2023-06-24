#if UNITY_2020_3_OR_NEWER
#define UNITY_ENGINE
#endif

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FacialCaptureSync
{
    /// <summary>
    /// An implemention of facial capture data receiver for iFacialMocap and Facemotion3d.<br/>
    /// https://www.ifacialmocap.com/<br/>
    /// https://www.facemotion3d.info/<br/>
    /// </summary>
    public sealed class FacialCaptureReceiver : IDisposable
    {
        public bool IsRunning => _isRunning;
        
        private readonly IFacialCaptureSource _captureSource;
        
        private bool _isRunning = false;
        private CancellationTokenSource _cancellationTokenSource;
        private UdpClient _udpClient;
        private IPAddress _captureDeviceAddress;
        
#region Ring Buffer
        public int CaptureBufferSize => _bufferSize;
        
        private readonly int _bufferSize; // Buffer size must be a power of two.
        private readonly long _bufferMask;
        private readonly FacialCapture[] _captureBuffer;
        
        private long _bufferHead = 0;
        private long _bufferTail = 0;
#endregion
        
        public FacialCaptureReceiver(IFacialCaptureSource captureDataSource, int bufferSize = 4)
        {
            _captureSource = captureDataSource;

            _bufferSize = MathUtils.CeilingPowerOfTwo(bufferSize); // Buffer size must be a power of two.
            _bufferMask = _bufferSize - 1;
            
            _captureBuffer = new FacialCapture[_bufferSize];
            for (var i = 0; i < _captureBuffer.Length; i++)
            {
                _captureBuffer[i] = new FacialCapture();
            }
        }
        
        public void Dispose()
        {
            Disconnect();
            Stop();
        }
        
        public bool TryDequeueCapture(ref FacialCapture capture)
        {
            if (_bufferHead >= _bufferTail) { return false; }
            
            var index = _bufferHead & _bufferMask;
            _captureBuffer[index].CopyTo(capture);
            _bufferHead++;
            
            return true;
        }
        
        public void Start()
        {
            if (_isRunning) { return; }
            
            _isRunning = true;

            _cancellationTokenSource = new CancellationTokenSource();
            RunReceiverLoop(_cancellationTokenSource.Token);
        }
        
        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            
            _udpClient?.Close();
            _udpClient?.Dispose();
            _udpClient = null;
            
            _isRunning = false;
        }
        
        public void ConnectToCaptureSource(string address)
        {
            if (!_isRunning) { Start(); }
            
            if (_captureSource.UseIndirectConnection) { return; }
            
            if (IPAddress.TryParse(address, out _captureDeviceAddress))
            {
                var data = System.Text.Encoding.UTF8.GetBytes(_captureSource.HandshakeMessage);
                _udpClient.Send(data, data.Length, new IPEndPoint(_captureDeviceAddress, _captureSource.HandshakePort));
                DebugLog($"ConnectToCaptureDevice | {_captureDeviceAddress}:{_captureSource.HandshakePort} |");
            }
            else
            {
                LogError($"Cannot parse the address: {address}");
            }
        }
        
        public void Disconnect()
        {
            if (_captureSource.UseIndirectConnection) { return; }
            var data = System.Text.Encoding.UTF8.GetBytes(_captureSource.StopStreamingMessage);
            _udpClient.Send(data, data.Length, new IPEndPoint(_captureDeviceAddress, _captureSource.HandshakePort));
        }
        
        private async void RunReceiverLoop(CancellationToken cancellationToken = default)
        {
            _udpClient = new UdpClient(_captureSource.ReceiverPort);
            
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = await _udpClient.ReceiveAsync().ConfigureAwait(false);
                OnReceived(result.Buffer);
            }
        }
        
        private void OnReceived(byte[] data)
        {
            try
            {
                var bufferFreeCount = _bufferSize - (int)(_bufferTail - _bufferHead);
                if (bufferFreeCount > 0)
                {
                    var message = System.Text.Encoding.ASCII.GetString(data);
                    var index = _bufferTail & _bufferMask;
                    
                    if (_captureSource.TryParse(message, ref _captureBuffer[index]))
                    {
                        _bufferTail++;
                    }
                }
            }
            catch (Exception e)
            {
                LogError(e);
            }
        }
        
        private static void LogError(object message)
        {
#if UNITY_ENGINE
            UnityEngine.Debug.LogError($"[{nameof(FacialCaptureReceiver)}] {message}");
#else
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{nameof(FacialCaptureReceiver)}] {message}");
            Console.ResetColor();
#endif
        }
        
        [
            System.Diagnostics.Conditional("DEBUG"),
            System.Diagnostics.Conditional("DEVELOPMENT_BUILD"),
            System.Diagnostics.Conditional("UNITY_EDITOR"),
        ]
        private static void DebugLog(object message)
        {
#if UNITY_ENGINE
            UnityEngine.Debug.Log($"[{nameof(FacialCaptureReceiver)}] {message}");
#else
            Console.WriteLine($"[{nameof(FacialCaptureReceiver)}] {message}");
#endif
        }
    }
}