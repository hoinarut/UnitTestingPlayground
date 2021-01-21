﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IamService.DataAccess
{
    public class UserRole
    {        
        public int UserId { get; set; }        
        public int RoleId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }
    }
}
