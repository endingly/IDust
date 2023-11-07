using Microsoft.VisualBasic;
using System.Text;
using System.Timers;

namespace IDust.Base;

public enum LogKeyword
{
    PLC,
    Vision,
    Calibrating,
    UI,
    Camera,
    VisionTool
}

public enum LogLevel
{
    Debug,
    Info,
    Warn,
    Error,
    Fatal
}

public static class LogExtension
{
    public static string GetString(this LogKeyword keyWord)
    {
        return keyWord switch
        {
            LogKeyword.PLC => "PLC",
            LogKeyword.Vision => "Vision",
            LogKeyword.Calibrating => "Calibrating",
            LogKeyword.UI => "UI",
            LogKeyword.Camera => "Camera",
            LogKeyword.VisionTool => "VisionTool",
            _ => "Unknow",
        };
    }

    public static string GetString(this LogLevel level)
    {
        return level switch
        {
            LogLevel.Debug => "Debug",
            LogLevel.Info => "Info",
            LogLevel.Warn => "Warn",
            LogLevel.Error => "Error",
            LogLevel.Fatal => "Fatal",
            _ => "Unknow",
        };
    }


}

public class Logger:IDisposable
{
    private LogKeyword _keyWord;
    private string outPutPath;
    private FileStream? stream;
    private bool _fileStreamEnable = true;
    public bool FileStreamEnable
    {
        get
        {
            return _fileStreamEnable;
        }
        set
        {
            // 值不相等，即有变化
            if (_fileStreamEnable != value)
            {
                // 如果是开
                if (value == true)
                {
                    stream = File.Open(outPutPath, FileMode.OpenOrCreate | FileMode.Append);
                }
                else
                {
                    stream?.Dispose();
                }
                _fileStreamEnable = value;
            }
        }
    }
    public bool ConsoleEnable { get; set; } = true;

    public Logger(LogKeyword keyWord, string DateTimeStr)
    {
        _keyWord = keyWord;
        outPutPath = $"{Environment.CurrentDirectory}/Log/{DateTimeStr}/{keyWord.GetString()}.log";
        if (_fileStreamEnable)
        {
            stream = File.Open(outPutPath, FileMode.OpenOrCreate | FileMode.Append);
        }
    }

    #region Log Methods
    public void Debug(string message)
    {
#if DEBUG
        WriteLog(LogLevel.Debug, message);
#endif
    }

    public void Info(string message)
    {
        WriteLog(LogLevel.Info, message);
    }

    public void Warn(string message)
    {
        WriteLog(LogLevel.Warn, message);
    }

    public void Error(string message)
    {
        WriteLog(LogLevel.Error, message);
    }

    public void Fatal(string message)
    {
        WriteLog(LogLevel.Fatal, message);
    }

    public void Debug(string message, Exception ex)
    {
#if DEBUG
        WriteLog(LogLevel.Debug, message, ex);
#endif
    }

    public void Info(string message, Exception ex)
    {
        WriteLog(LogLevel.Info, message, ex);
    }

    public void Warn(string message, Exception ex)
    {
        WriteLog(LogLevel.Warn, message, ex);
    }

    public void Error(string message, Exception ex)
    {
        WriteLog(LogLevel.Error, message, ex);
    }

    public void Fatal(string message, Exception ex)
    {
        WriteLog(LogLevel.Fatal, message, ex);
    }

    public void Debug(string message, string SupInformation, Exception ex)
    {
        #if DEBUG
        WriteLog(LogLevel.Debug, message, SupInformation, ex);
        #endif
    }

    public void Info(string message, string SupInformation, Exception ex)
    {
        WriteLog(LogLevel.Info, message, SupInformation, ex);
    }

    public void Warn(string message, string SupInformation, Exception ex)
    {
        WriteLog(LogLevel.Warn, message, SupInformation, ex);
    }

    public void Error(string message, string SupInformation, Exception ex)
    {
        WriteLog(LogLevel.Error, message, SupInformation, ex);
    }

    public void Fatal(string message, string SupInformation, Exception ex)
    {
        WriteLog(LogLevel.Fatal, message, SupInformation, ex);
    }

    public void Debug(string message, string SupInformation)
    {
        #if DEBUG
        WriteLog(LogLevel.Debug, message, SupInformation);
        #endif
    }

    public void Info(string message, string SupInformation)
    {
        WriteLog(LogLevel.Info, message, SupInformation);
    }

    public void Warn(string message, string SupInformation)
    {
        WriteLog(LogLevel.Warn, message, SupInformation);
    }

    public void Error(string message, string SupInformation)
    {
        WriteLog(LogLevel.Error, message, SupInformation);
    }

    public void Fatal(string message, string SupInformation)
    {
        WriteLog(LogLevel.Fatal, message, SupInformation);
    }
    #endregion

    #region private methods
    private void WriteLog(LogLevel level, string message)
    {
        string str = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} [{_keyWord.GetString()}] [{level.GetString()}] {message}";
        if (ConsoleEnable)
        {
            Console.WriteLine(str);
        }
        if (FileStreamEnable)
        {
            stream?.Write(Encoding.UTF8.GetBytes(str));
            stream?.Flush();
        }
    }

    private void WriteLog(LogLevel level, string message, Exception ex)
    {
        string str = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} [{_keyWord.GetString()}] [{level.GetString()}] {message} -> {ex.Message}";
        if (ConsoleEnable)
        {
            Console.WriteLine(str);
        }
        if (FileStreamEnable)
        {
            stream?.Write(Encoding.UTF8.GetBytes(str));
            stream?.Flush();
        }
    }

    private void WriteLog(LogLevel level, string message, string SupInformation, Exception ex)
    {
        string str = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} [{_keyWord.GetString()}] [{level.GetString()}] {message} {SupInformation} -> {ex.Message}";
        if (ConsoleEnable)
        {
            Console.WriteLine(str);
        }
        if (FileStreamEnable)
        {
            stream?.Write(Encoding.UTF8.GetBytes(str));
            stream?.Flush();
        }
    }

    private void WriteLog(LogLevel level, string message, string SupInformation)
    {
        string str = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} [{_keyWord.GetString()}] [{level.GetString()}] {message} {SupInformation}";
        if (ConsoleEnable)
        {
            Console.WriteLine(str);
        }
        if (FileStreamEnable)
        {
            stream?.Write(Encoding.UTF8.GetBytes(str));
            stream?.Flush();
        }
    }

    #endregion

    public void Dispose()
    {
        stream?.Flush();
        stream?.Dispose();
    }
}

public static class Log
{
    private static Dictionary<LogKeyword, Logger> _loggers = new Dictionary<LogKeyword, Logger>();
    private static string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
    private static System.Timers.Timer timer = new System.Timers.Timer(DateTime.Now.AddDays(1) - DateTime.Now);
    private static bool _init = false;

    public static Logger GetLogger(LogKeyword kw)
    {
        if (!_init)
        {
            Init();
        }
        if (_loggers.TryGetValue(kw, out Logger? value))
        {
            return value;
        }
        else
        {
            var logger = new Logger(kw, dateTime);
            _loggers.Add(kw, logger);
            return logger;
        }
    }

    private static void Init()
    {
        timer.Elapsed += Tommrow;
        timer.Start();
        if (!Path.Exists($"./Log/{dateTime}"))
        {
            Directory.CreateDirectory($"./Log/{dateTime}");
        }
    }

    private static void Tommrow(object? sender, ElapsedEventArgs args)
    {
        dateTime = DateTime.Now.ToString("yyyy-MM-dd");
        timer.Interval = (DateTime.Now.AddDays(1) - DateTime.Now).TotalMilliseconds;
        if (!Path.Exists($"./Log/{dateTime}"))
        {
            Directory.CreateDirectory($"./Log/{dateTime}");
        }
        bool flag = false;
        foreach (var item in _loggers)
        {
            flag = item.Value.FileStreamEnable;
            item.Value.FileStreamEnable = flag;
        }
    }
}