using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ORMtest.Entities
{
    public class Topic
    {

        public int topicID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public List<Session> Sessions { get; set; }

    }
}
