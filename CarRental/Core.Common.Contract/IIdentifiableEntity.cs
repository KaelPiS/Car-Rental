﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Contract
{
    public interface IIdentifiableEntity
    {
        int EntityID { get; set; }
    }
}
