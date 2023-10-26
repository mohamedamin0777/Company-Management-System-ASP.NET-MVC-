using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, 
                                  ILogger<EmployeeController> logger,
                                  IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index(string SearchValue = "")
        {
            IEnumerable<Employee> employees;
            IEnumerable<EmployeeViewModels> mappedEmployees;
            if (string.IsNullOrEmpty(SearchValue))
            {
                employees = _unitOfWork.EmployeeRepository.GetAll();
                mappedEmployees = _mapper.Map<IEnumerable<EmployeeViewModels>>(employees);
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.Search(SearchValue);
                mappedEmployees = _mapper.Map<IEnumerable<EmployeeViewModels>>(employees);
            }

            return View(mappedEmployees);
        }

        public IActionResult Create()
        {
            ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModels employeeVM)
        {
            //employee.Department = _unitOfWork.DepartmentRepository.GetById(employee.DepartmentId);
            //ModelState["Department"].ValidationState = ModelValidationState.Valid;
            if (ModelState.IsValid)
            {
                try
                {
                    //Employee employee = new Employee
                    //{
                    //    Name = employeeVM.Name,
                    //    Address = employeeVM.Address,
                    //    DepartmentId = employeeVM.DepartmentId,
                    //    EmailAddress = employeeVM.EmailAddress,
                    //    HireDate = employeeVM.HireDate,
                    //    IsActive = employeeVM.IsActive,
                    //    Salary = employeeVM.Salary

                    //};

                    var employee = _mapper.Map<Employee>(employeeVM);
                    employee.ImageUrl = DocumentSettings.UploudFile(employeeVM.Image, "Images");
                    _unitOfWork.EmployeeRepository.Add(employee);
                    TempData["Message"] = "Department Created Successfully";
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();

            return View(employeeVM);
        }

        public IActionResult Details(int? id)
        {
            try
            {
                if (id is null)
                    return RedirectToAction("Error", "Home");

                var employee = _unitOfWork.EmployeeRepository.GetById(id);
                var mappedEmployee = _mapper.Map<EmployeeViewModels>(employee);
                if (employee is null)
                    return RedirectToAction("Error", "Home");

                return View(mappedEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Update(int? id)
        {
            ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
            if (id is null)
                return RedirectToAction("Error", "Home");

            var employee = _unitOfWork.EmployeeRepository.GetById(id);
            var mappedEmployee = _mapper.Map<EmployeeViewModels>(employee);
            if (employee is null)
                return RedirectToAction("Error", "Home");

            return View(mappedEmployee);
        }

        [HttpPost]
        public IActionResult Update(int id, EmployeeViewModels employeeVM)
        {
            //ModelState["Department"].ValidationState = ModelValidationState.Valid;
            if (id != employeeVM.Id)
                return RedirectToAction("Error", "Home");

            try
            {
                if (ModelState.IsValid)
                {
                    var employee = _mapper.Map<Employee>(employeeVM);
                    employee.ImageUrl = DocumentSettings.UploudFile(employeeVM.Image, "Images");
                    _unitOfWork.EmployeeRepository.Update(employee);
                    ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
                    return View(employeeVM);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id is null)
                return RedirectToAction("Error", "Home");

            var employee = _unitOfWork.EmployeeRepository.GetById(id);
            var mappedEmployee = _mapper.Map<EmployeeViewModels>(employee);

            if (mappedEmployee is null)
                return RedirectToAction("Error", "Home");

            _unitOfWork.EmployeeRepository.Delete(employee);
            //DocumentSettings.DeleteFile(employee.ImageUrl);
            return RedirectToAction("Index");
        }
    }
}
