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
    public class DepartmentRepository : GeniricRepository<Department>, IDepartmentRepository
    {
        private readonly MVCAppDbContext _context;

        public DepartmentRepository(MVCAppDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Department> Search(string name)
            => _context.Departments.Where(dept => dept.Name.Trim().ToLower().Contains(name.Trim().ToLower()));

        //public int Add(Department department)
        //{
        //    _context.Departments.Add(department);
        //    return _context.SaveChanges();
        //}

        //public int Delete(Department department)
        //{
        //    _context.Departments.Remove(department);
        //    return _context.SaveChanges();
        //}

        //public IEnumerable<Department> GetAllDepartments()
        //=> _context.Departments.ToList();

        //public Department GetDepartmentById(int? id)
        //=> _context.Departments.FirstOrDefault(x => x.Id == id);

        //public int Update(Department department)
        //{
        //    _context.Departments.Update(department);
        //    return _context.SaveChanges();
        //}
    }
}
