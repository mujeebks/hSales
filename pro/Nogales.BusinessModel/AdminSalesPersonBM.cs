using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.BusinessModel
{
    public class AdminSalesPersonBM
    {

    }

    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAccess { get; set; }
    }
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAccess { get; set; }
    }

    public class UserModuleAccess
    {
        public int UserId { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
    }
    public class UserCategoryAccess
    {
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class UserDetails
    {
        public List<Category> Categories { get; set; }
        public List<Module> Modules { get; set; }
        public Module DisplayModule { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public bool IsRestrictedModuleAccess { get; set; }
        public bool IsRestrictedCategoryAccess { get; set; }
    }

    public class UserAccessModuleAndCategory
    {
        public List<Category> Categories { get; set; }
        public List<Module> Modules { get; set; }
        public string DisplayModule { get; set; }
        public bool IsRestrictedCategoryAccess { get; set; }
        public bool IsRestrictedModuleAccess { get; set; }
    }
}
