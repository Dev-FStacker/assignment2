﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface ITagService
    {
        Task<List<BusinessObject.Tag>> GetAll();
    }
}
