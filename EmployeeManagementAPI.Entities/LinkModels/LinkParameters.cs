using Microsoft.AspNetCore.Http;
using EmployeeManagementAPI.SharedResources;

namespace CodeMaze.Entities
{
    public record LinkParameters(EmployeeParameters EmployeeParameters, HttpContext Context);
}
