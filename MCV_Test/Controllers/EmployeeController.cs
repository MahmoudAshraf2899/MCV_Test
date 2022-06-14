using MCV_Test.DTO;
using MCV_Test.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MCV_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get All Employees
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employee = await _context.Employees.
                                    OrderBy(e => e.Name).
                                    Include(d => d.Department).
                                    ToListAsync();
                if (employee is null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            //Dead Line Blocked me to Use Logging .. So I Left the exception Empty.
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Get Employee By Id
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        [HttpGet("{identifier}")]
        public async Task<IActionResult> GetEmployeeById(int identifier)
        {
            try
            {
                var employee = await _context.Employees
                                           .Include(d => d.Department)
                                           .FirstOrDefaultAsync(e => e.UniqueIdentifier == identifier);
                if (employee is null)
                {
                    return NotFound($"This Identifier is not exist ! {identifier}");
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Add New Employee
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromForm] EmployeeDTO dto)
        {
            try
            {
                DateTime localDate = DateTime.Now;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (dto.HiringDate > localDate)
                {
                    return BadRequest("Date Cannot Be Bigger Than Today's Date !");
                }
                Employee? employee = new Employee
                {

                    BirthDate = dto.BirthDate,
                    HiringDate = dto.HiringDate,
                    Name = dto.Name,
                    Title = dto.Title,
                    DepartmentId = dto.DepartmentId,

                };


                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                //Increase Number Of Employees in the Department by 1 
                Department? departmentSize = await _context.Departments
                                        .Where(d => d.Id == employee.DepartmentId)
                                        .FirstOrDefaultAsync();
                if (departmentSize is not null)
                {
                    departmentSize.NumberOfEmployees++;
                    await _context.SaveChangesAsync();
                }
                return Ok(employee);


            }
            catch (Exception ex)
            {

                throw;
            }
        }


        /// <summary>
        /// Update Employee
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        [HttpPut("{identifier}")]
        public async Task<IActionResult> UpdateEmployee(int identifier, [FromForm] EmployeeDTO dto)
        {
            DateTime localDate = DateTime.Now;
            var employee = await _context.Employees
                                           .Include(d => d.Department)
                                           .FirstOrDefaultAsync(e => e.UniqueIdentifier == identifier);

            if (employee is null)
            {
                return NotFound($"No Employee Exist with this identifier {identifier}");
            }
            //Increase Number Of Employees in the new Department by 1 
            // and decrement The Old Department by 1
            if (employee.DepartmentId != dto.DepartmentId)
            {

                Department? newdepartmentSize = await _context.Departments
                                        .Where(d => d.Id == dto.DepartmentId)
                                        .FirstOrDefaultAsync();

                Department? OlddepartmentSize = await _context.Departments
                                      .Where(d => d.Id == employee.DepartmentId)
                                      .FirstOrDefaultAsync();

                if (newdepartmentSize is not null && OlddepartmentSize is not null)
                {
                    newdepartmentSize.NumberOfEmployees++;
                    OlddepartmentSize.NumberOfEmployees--;
                }
            }
            if (dto.HiringDate > localDate)
            {
                return BadRequest("Date Cannot Be Bigger Than Today's Date !");
            }
            employee.HiringDate = dto.HiringDate;
            employee.BirthDate = dto.BirthDate;
            employee.Name = dto.Name;
            employee.Title = dto.Title;
            employee.DepartmentId = dto.DepartmentId;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return Ok(employee);
        }
        
        /// <summary>
        /// Delete Employee
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        [HttpDelete("{identifier}")]
        public async Task<IActionResult> DeleteEmployee(int identifier)
        {
            try
            {
                var employee = await _context.Employees
                                                   .Include(d => d.Department)
                                                   .FirstOrDefaultAsync(e => e.UniqueIdentifier == identifier);

                if (employee is null)
                {
                    return NotFound($"No Employee Exist with this identifier {identifier}");
                }
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                //Decrement The Employee Size Of Employee's Department by 1
                Department? departmentSize = await _context.Departments
                                     .Where(d => d.Id == employee.DepartmentId)
                                     .FirstOrDefaultAsync();

                if (departmentSize is not null)
                {
                    departmentSize.NumberOfEmployees--;
                }
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
