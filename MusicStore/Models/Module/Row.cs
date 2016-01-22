﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.Models.Module
{
    public abstract class Row
    {
        [DBItem]
        public int Id { get; set; }
        public RowState State { get; set; } = RowState.Unchanged;
    }
}