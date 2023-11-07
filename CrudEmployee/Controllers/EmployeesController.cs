using CrudEmployee.Data;
using CrudEmployee.Models;
using CrudEmployee.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CrudEmployee.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeDbContext employeeDbContext;

        public EmployeesController(EmployeeDbContext employeeDbContext)
        {
            this.employeeDbContext = employeeDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
           var employees = await employeeDbContext.Employees.ToListAsync();

            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateofBirth = addEmployeeRequest.DateofBirth,
            };
            await employeeDbContext.Employees.AddAsync(employee);
            await employeeDbContext.SaveChangesAsync();
            return RedirectToAction("Add");

        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await employeeDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateofBirth = employee.DateofBirth,
                };
                return await Task.Run(()=>View("View",viewModel));
            }
            
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var emplooye = await employeeDbContext.Employees.FindAsync(model.Id);
            if (emplooye != null)
            {
                emplooye.Name = model.Name;
                emplooye.Email = model.Email;
                emplooye.Salary = model.Salary;
                emplooye.DateofBirth = model.DateofBirth;
                emplooye.Department = model.Department;
                await employeeDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
           

            return RedirectToAction("Index");
        }

        [HttpPost]
        
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await employeeDbContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                employeeDbContext.Remove(employee);
                await employeeDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
