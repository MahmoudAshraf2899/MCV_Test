using MCV_Test.DTO;
using MCV_Test.Helpers;
using MCV_Test.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MCV_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        int totalEmployees = 0;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Departments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            try
            {
                List<Department>? department = await _context.Departments
                                               .OrderBy(e => e.Name)
                                                .ToListAsync();
                if (department is null)
                {
                    return NotFound();
                }
                return Ok(department);
            }
            //Dead Line Blocked me to Use Logging .. So I Left the exception Empty.
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Department by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentbyId(int id)
        {

            try
            {
                Department? department = await _context.Departments
                                               .FirstOrDefaultAsync(d => d.Id == id);
                if (department is null)
                {
                    return NotFound($"This Id is not exist ! {department}");
                }
                return Ok(department);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Add New Department
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("AddDepartment")]
        public async Task<IActionResult> AddDepartment([FromForm] DepartmentDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Department? department = new Department
                {

                    Name = dto.Name,
                    NumberOfEmployees = totalEmployees,
                };
                await _context.Departments.AddAsync(department);
                await _context.SaveChangesAsync();
                return Ok(department);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Update Department
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromQuery] DepartmentDTO dto)
        {
            try
            {
                Department? department = await _context.Departments
                                                   .FirstOrDefaultAsync(d => d.Id == id);

                if (department is null)
                {
                    return NotFound($"No Department Exist with this identifier {id}");
                }

                department.Name = dto.Name;

                _context.Departments.Update(department);
                await _context.SaveChangesAsync();
                return Ok(department);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        /// <summary>
        /// Delete Department
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                Department? department = await _context.Departments
                                                           .FirstOrDefaultAsync(e => e.Id == id);

                if (department is null)
                {
                    return NotFound($"No Department Exist with this identifier {id}");
                }
                //Warninng : If We Remove Department it will remove all Employees in this Department ..
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }





        /// <summary>
        /// Departments Paging By Sending Page Number and Page Size 
        /// </summary>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        [HttpGet("GetAllDepartmentsPaging")]
        public async Task<IActionResult> GetAllDepartmentsPaging([FromQuery] QueryParameters? queryParameters)
        {
            try
            {
                if (queryParameters.Page == 0)
                {
                    var deps = await _context.Departments.ToListAsync();
                    return Ok(deps);
                }
                else
                {
                    var department = await _context.Departments.Skip(queryParameters.Size * (queryParameters.Page - 1))
                                                                        .Take(queryParameters.Size)
                                                                          .OrderBy(p => p.Name)
                                                                          .ToListAsync();

                    if (department == null)
                        return NotFound();


                    return Ok(department);
                }


            }
            //Dead Line Blocked me to Use Logging .. So I Left the exception Empty.
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        [HttpGet("FilterDepartments")]
        public async Task<IActionResult> FilterDepartments([FromQuery] DepartmentQueryParameters  queryParameters)
        {
            IQueryable<Department> departments = _context.Departments;

            //Filter With Number Of Employee..
            if (queryParameters.MinNumberOfEmployee != null &&
                queryParameters.MaxNumberOfEmployee != null)                 
            {
                departments = departments.Where(
                    d => d.NumberOfEmployees >= queryParameters.MinNumberOfEmployee.Value &&
                        d.NumberOfEmployees <= queryParameters.MaxNumberOfEmployee.Value);
            }

            ////Filter With Department Name
            //if (!string.IsNullOrEmpty(queryParameters.Name))
            //{
            //    departments = departments.Where(d=>d.Name == queryParameters.Name);
            //}

            //Search by Department Name
            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                departments = departments.Where(
                    d => d.Name.ToLower().Contains(queryParameters.Name.ToLower()));
                    
            }
            departments = departments
                            .Skip(queryParameters.Size * (queryParameters.Page - 1))
                            .Take(queryParameters.Size);

            return Ok(await departments.ToListAsync());
        }

    }
}
