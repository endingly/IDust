namespace IDust.Base;

public enum LogKeyWord
{
    PLC,
    Vision,
    Calibrating,
    UI
}

public static class LogKeyWordExtension
{
    public static string GetString(this LogKeyWord keyWord)
    {
        return keyWord switch
        {
            LogKeyWord.PLC => "PLC",
            LogKeyWord.Vision => "Vision",
            LogKeyWord.Calibrating => "Calibrating",
            LogKeyWord.UI => "UI",
            _ => "Unknow",
        };
    }
}
