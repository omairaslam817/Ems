using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Infrastructure.Authentication
{
    public class HasPermissionAttribute:AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission)
            :base(policy:permission.ToString())
        {
            
        }
    }
}
