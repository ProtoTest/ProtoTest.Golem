using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Golem.DBManagement.Entities
{
    [Serializable]
    public class BaseEntity
    {
        //public BaseEntity();

        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
        public DateTime LastModifyDate { get; set; }
        public int LastModifyUserId { get; set; }

    }
}
