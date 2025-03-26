using EmployeeManagementAPI.Entities.Models;
using System.Dynamic;
namespace EmployeeManagementAPI.Contracts;

public interface IDataShaper<T>
{
    IEnumerable<Entity> ShapeData(IEnumerable<T> entities, string fieldsString);
    Entity ShapeData(T entity, string fieldsString);
}
