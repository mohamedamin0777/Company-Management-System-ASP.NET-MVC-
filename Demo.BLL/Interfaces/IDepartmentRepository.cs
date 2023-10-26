using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IDepartmentRepository : IGeniricRepository<Department>
    {
        IEnumerable<Department> Search(string name);
        //Department GetDepartmentById(int? id);
        //IEnumerable<Department> GetAllDepartments();
        //int Add(Department department);
        //int Update(Department department);
        //int Delete(Department department);
    }
}
