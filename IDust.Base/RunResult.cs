using System.Reflection.Metadata;

namespace IDust.Base
{
    /// <summary>
    /// 各动作运行的结果
    /// </summary>
    public class RunResult
    {
        #region members
        public bool isSuccess;
        public string message;
        public ErrorCode errorCode;
        #endregion

        #region ctor
        public RunResult()
        {
            isSuccess = false;
            message = ErrorCode.Unkown.GetString();
            errorCode = ErrorCode.Unkown;
        }

        public RunResult(ErrorCode er)
        {
            message = er.GetString();
            errorCode = er;
            if (er >= ErrorCode.PlcConnected)
            {
                isSuccess = true;
            }
            else
            {
                isSuccess = false;
            }
        }
        #endregion

        #region public method
        public void Reset(ErrorCode er)
        {
            message = er.GetString();
            errorCode = er;
            if (er >= ErrorCode.PlcConnected)
            {
                isSuccess = true;
            }
            else
            {
                isSuccess = false;
            }
        }
        #endregion
    }
}