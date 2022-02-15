using System;
using System.Collections.Generic;
using System.Text;

namespace BlogCore.Domain.DomainServices.Entitys
{
    public class AdverUserEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
