using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementAPI.Entities.Models
{
    public class Company
    {
        //Id property is going to be mapped with a CompanyId name in the database.
        [Column("CompanyId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Company name is a required field.")] 
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")] 
        public string? Name { get; set; }

        [Required(ErrorMessage = "Company address is a required field.")] 
        [MaxLength(60, ErrorMessage = "Maximum length for the Address is 60 characters.")] 
        public string? Address { get; set; }

        public string? Country { get; set; }

        //navigational properties
        public ICollection<Employee>? Employees { get; set; }
    }
}
