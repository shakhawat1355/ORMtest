using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMtest.Entities
{
    public class Session
    {
        public int sessionID { get; set; }

        public int DurationInHour { get; set; }
        public string LearningObjective { get; set; }

    }
}
