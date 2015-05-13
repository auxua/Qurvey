﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Qurvey.Backend
{
    public class Result
    {
        public Answer Answer { get; set; }

        public int Count { get; set; }

        protected Result()
        {

        }

        public Result(Answer answer, int count) : this()
        {
            this.Answer = answer;
            this.Count = count;
        }
    }
}