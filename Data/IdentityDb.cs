﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CB.Identidade.Api.Data
{
    public class IdentityDb : IdentityDbContext
    {
        public IdentityDb(DbContextOptions<IdentityDb> options) : base(options) { }
    }
}
