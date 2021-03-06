﻿using System.Collections.Generic;

namespace Server.Models
{
    public class OverviewViewModel
    {
        public BotAccountViewModel ActiveAccount { get; set; }
        public bool BotIsRunning { get; set; }
        public IEnumerable<string> LogBuffer { get; set; }
    }
}
