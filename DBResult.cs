using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PublicDBUtil
{
    public class DBResult
    {
        public bool IsSuccessed { get; set; }
        public DataTable Table { get; set; }
        public DataSet DataSet { get; set; }
        public Object DataObject { get; set; }
        public string Message { get; set; }
        public int AffectRowCount { get; set; }
    }
}
