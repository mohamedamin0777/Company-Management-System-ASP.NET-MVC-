using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GeniricRepository<Employee>, IEmployeeRepository
    {
        private readonly MVCAppDbContext _context;

        public EmployeeRepository(MVCAppDbContext context) : base(context) 
        {
            _context = context;
        }

        public IEnumerable<Employee> GetEmployeesByDepartmentName(string departmentName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> Search(string name)
            => _context.Employees.Where(emp => emp.Name.Trim().ToLower().Contains(name.Trim().ToLower()));
        //public int Add(Employee employee)
        //{
        //    _context.Employees.Add(employee);
        //    return _context.SaveChanges();
        //}

        //public int Delete(Employee employee)
        //{
        //    _context.Employees.Remove(employee);
        //    return _context.SaveChanges();
        //}

        //public IEnumerable<Employee> GetAllEmployees()
        //    => _context.Employees.ToList();

        //public Employee GetEmployeeById(int? id)
        //    => _context.Employees.FirstOrDefault(x => x.Id == id);

        //public int Update(Employee employee)
        //{
        //    _context.Employees.Update(employee);
        //    return _context.SaveChanges();
        //}
    }
}
