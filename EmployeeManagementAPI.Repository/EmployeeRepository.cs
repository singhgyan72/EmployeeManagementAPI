using EmployeeManagementAPI.Contracts;
using EmployeeManagementAPI.Entities.Models;
using EmployeeManagementAPI.Repository.Extensions;
using EmployeeManagementAPI.SharedResources;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Repository
{
    internal sealed class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters parameters, bool trackChanges)
        {
            var employees = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
                                .FilterEmployees(parameters.MinAge, parameters.MaxAge)
                                .Search(parameters.SearchTerm)
                                .Sort(parameters.OrderBy)
                                   .OrderBy(e => e.Name)
                                   .ToListAsync();

            return PagedList<Employee>.ToPagedList(employees, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges) =>
            await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee) => Delete(employee);
    }

}
