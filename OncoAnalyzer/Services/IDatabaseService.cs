﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OncoAnalyzer.Services
{
    public interface IDatabaseService
    {
        SqlConnection GetConnection();
    }
}
