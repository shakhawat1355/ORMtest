﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMtest.Entities
{
    public class Address
    {
        public int addressID { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

    }
}
