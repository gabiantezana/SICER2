using System;

namespace SICER.EXCEPTION
{
    public class SapException : Exception
    {
        public int SapErrorCode { get; set; }
        public string SapErrorMessage { get; set; }
        public string Message { get; set; }
        public SapExceptionEntity SapExceptionEntity { get; set; }

        public SapException(string message = null) : base(message)
        {
            this.Message = message;
            SapExceptionEntity = new SapExceptionEntity();
        }

        public SapException(string message, Exception inner) : base(message, inner) { }

        public SapException(SapExceptionEntity sapException, string message = null)
            : base(sapException.ErrorMessage)
        {
            SapErrorCode = sapException.ErrorCode;
            SapErrorMessage = sapException.ErrorMessage;
            Message = sapException.Message;
        }
    }
    //Para evitar referencia circular
    public class SapExceptionEntity
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Message { get; set; }
    }
}
