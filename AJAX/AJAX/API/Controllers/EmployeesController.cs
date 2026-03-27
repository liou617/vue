using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using API.DTO;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    [EnableCors("MVC")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public EmployeesController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        //public IEnumerable<Employee> GetEmployees()
        public async Task<IEnumerable<EmployeeDTO>> GetEmployees()
        {
            return _context.Employees.Select(e=>new EmployeeDTO { 
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Title = e.Title,
            });      
            //return await _context.Employees.ToListAsync();      //需要時間, 需要空間
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<EmployeeDTO> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return null;
            }
            return new EmployeeDTO
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Title = employee.Title,
            };
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ResultDTO> PutEmployee(int id, EmployeeDTO employeeDTO)
        {
            if (id != employeeDTO.EmployeeId)
            {
                return new ResultDTO
                {
                    Ok=false,
                    Code=400
                };
            }
            Employee Emp = await _context.Employees.FindAsync(id);
            if (Emp == null)
            {
                return new ResultDTO
                {
                    Ok = false,
                    Code = 404
                };
            }
            else
            {
                Emp.FirstName = employeeDTO.FirstName;
                Emp.LastName = employeeDTO.LastName;
                Emp.Title = employeeDTO.Title;
                _context.Entry(Emp).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return new ResultDTO
                    {
                        Ok = false,
                        Code = 500
                    };
                }
                return new ResultDTO
                {
                    Ok = true,
                    Code = 204
                };
            }                
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ResultDTO> PostEmployee(EmployeeDTO employeeDTO)
        {
            Employee Emp = new Employee
            {
                EmployeeId = 0,
                FirstName = employeeDTO.FirstName,
                LastName = employeeDTO.LastName,
                Title = employeeDTO.Title,
            };
            _context.Employees.Add(Emp);
            await _context.SaveChangesAsync();
            employeeDTO.EmployeeId = Emp.EmployeeId;
            return new ResultDTO
            {
                Ok = true,
                Code=204
            };
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ResultDTO> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return new ResultDTO
                {
                    Ok = false,
                    Code = 404
                };
            }
            try
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return new ResultDTO
                {
                    Ok = false,
                    Code = 500
                };
            }
            return new ResultDTO
            {
                Ok = true,
                Code=204
            };
        }
    }
}
