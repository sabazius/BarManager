﻿using System;
using System.Collections.Generic;

namespace BarManager.Models.DTO
{
    public class Location
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Location> location { get; set; }
    }
}
