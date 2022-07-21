using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BM.Lib.Domains
{
    public class ExcepcionSistema : Exception
    {
        public int ErrorCode { get; set; }

        public ExcepcionSistema()
            : base()
        {
            this.ErrorCode = 999;
        }

        public ExcepcionSistema(string message)
            : base(message)
        {
            this.ErrorCode = 999;
        }

        public ExcepcionSistema(string message, int errorCode)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public ExcepcionSistema(string message, int errorCode, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorCode = errorCode;
        }

        public ExcepcionSistema(string message, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorCode = 999;
        }

        public ExcepcionSistema(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
            this.ErrorCode = 999;
        }
    }
}
