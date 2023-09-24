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
            message = ErrorCode.Fail.ToMessage();
            errorCode = ErrorCode.Fail;
        }
        public RunResult(string msg)
        {
            isSuccess = false;
            message = msg;
            errorCode = ErrorCode.Fail;
        }

        public RunResult(ErrorCode ec, string msg)
        {
            isSuccess = false;
            message = msg;
            errorCode = ec;
        }

        public RunResult(bool flag, ErrorCode ec, string msg)
        {
            isSuccess = flag;
            message = msg;
            errorCode = ec;
        }
        #endregion

        #region static methods
        public static RunResult CreateSuccessResult()
        {
            return new RunResult(true, ErrorCode.OK, ErrorCode.OK.ToMessage());
        }

        public static RunResult CreateSuccessResult(string msg)
        {
            return new RunResult(true, ErrorCode.OK, msg);
        }
        #endregion

        #region public methods
        public void SetSuccess(string successMsg)
        {
            isSuccess = true;
            message = successMsg;
            errorCode = ErrorCode.OK;
        }
        #endregion
    }
}