using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;

        public DepartmentController(
            //IDepartmentRepository departmentRepository, 
            ILogger<DepartmentController> logger,
            IMapper mapper,
            IUnitOfWork unitOfWork)
            //IEmployeeRepository employeeRepository)
        {
            //_departmentRepository = departmentRepository;
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
        }

        public IActionResult Index(string SearchValue = "")
        {
            IEnumerable<Department> departments;
            IEnumerable<DepartmentViewModels> mappedDepartments;
            if (string.IsNullOrEmpty(SearchValue))
            {
                departments = _unitOfWork.DepartmentRepository.GetAll();
                mappedDepartments = _mapper.Map<IEnumerable<DepartmentViewModels>>(departments);
            }
            else
            {
                departments = _unitOfWork.DepartmentRepository.Search(SearchValue);
                mappedDepartments = _mapper.Map<IEnumerable<DepartmentViewModels>>(departments);
            }
            //ViewData["Message"] = "Hello from view data";
            //ViewBag.MessageBag = "Hello from view bag";

            //TempData.Keep("Message");
            return View(mappedDepartments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DepartmentViewModels departmentVM)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var department = _mapper.Map<Department>(departmentVM);
                    _unitOfWork.DepartmentRepository.Add(department);
                    TempData["Message"] = "Department Created Successfully";
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return View(departmentVM);
        }

        public IActionResult Details(int? id)
        {
            try
            {
                if (id is null)
                    return RedirectToAction("Error", "Home");

                var department = _unitOfWork.DepartmentRepository.GetById(id);
                var mappedDepartment = _mapper.Map<DepartmentViewModels>(department);
                if (department is null)
                    return RedirectToAction("Error", "Home");

                return View(mappedDepartment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Update(int? id)
        {
            if (id is null)
                return RedirectToAction("Error", "Home");

            var department = _unitOfWork.DepartmentRepository.GetById(id);
            var mappedDepartment = _mapper.Map<DepartmentViewModels>(department);
            if (department is null)
                return RedirectToAction("Error", "Home");

            return View(mappedDepartment);
        }

        [HttpPost]
        public IActionResult Update(int id, DepartmentViewModels departmentVM)
        {
            if (id != departmentVM.Id)
                return RedirectToAction("Error", "Home");

            try
            {
                if (ModelState.IsValid)
                {
                    var department = _mapper.Map<Department>(departmentVM);
                    _unitOfWork.DepartmentRepository.Update(department);
                    return RedirectToAction("Index");
                }
                else
                    return View(departmentVM);
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

            var department = _unitOfWork.DepartmentRepository.GetById(id);
            var mappedDepartment = _mapper.Map<DepartmentViewModels>(department);

            if (mappedDepartment is null)
                return RedirectToAction("Error", "Home");

            _unitOfWork.DepartmentRepository.Delete(department);
            return RedirectToAction("Index");
        }
    }
}
