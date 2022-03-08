using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication5.Dtos.Employee;
using WebApplication5.Models;
using WebApplication5.ViewModels;

namespace WebApplication5.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController:Controller
    {
        private  readonly  IEmployeeRepository _employeeRepository;
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IMapper _mapper;

        [Obsolete]
        public HomeController(
            IMapper mapper,
            IEmployeeRepository employeeRepository,
            IHostingEnvironment hostingEnvironment
            )
        {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            _mapper = mapper;

    }
        //[Route("~/Home")]
        [Route("~/")]
        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployees();
            return View(model);        
        }
        [Route("{id?}")]
        public ViewResult Details(int? id)
        {
            var employee = _employeeRepository.getEmployee(id ?? 1);
            var dto = _mapper.Map<EmployeeDto>(employee);
            var vm  = _mapper.Map<HomeDetailsViewModel>(dto);
            return View(vm);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        [Obsolete]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photos != null && model.Photos.Count>0)
                {
                    foreach (IFormFile photo in model.Photos)
                    {
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }
                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Departmet = model.Departmet,
                    PhotosPath = uniqueFileName
                };
                _employeeRepository.Add(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.ID });
            }
            return View();
        }
    }
}
