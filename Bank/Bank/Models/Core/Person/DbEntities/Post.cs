﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? CompanyId { get; set; }

        public Company Company { get; set; }

        public List<Person> People { get; set; }
    }
}
