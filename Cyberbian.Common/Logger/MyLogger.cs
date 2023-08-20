using Cyberbian.Common.ORM;
using CyberbianSite.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberbian.Common.Logger
{
    public class MyLogger
    {
        private string _connectionString { get; set; }

        public MyLogger(string connString)
        {
            _connectionString = connString;
        }

        public void Log(string MsgSource, string Payload = null, Exception ex = null)
        {
            try
            {
                SyslogORM orm = new SyslogORM(_connectionString);
                Syslog syslog = new Syslog();
                syslog.MsgSource = MsgSource;
                if (Payload != null)
                {
                    syslog.Payload = Payload;
                }
                if (ex != null)
                {
                    syslog.MsgText = ex.Message + Environment.NewLine + ex.StackTrace;
                }
                syslog = orm.Create(syslog);
            }
            catch { }

        }
    }
}
