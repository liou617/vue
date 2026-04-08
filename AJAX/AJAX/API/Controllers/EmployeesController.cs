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

        // Post: api/Employees/Filter
        [HttpPost("Filter")]
        //public IEnumerable<Employee> GetEmployees()
        public async Task<IEnumerable<EmployeeDTO>> FilterEmployees([FromBody] string keyword)
        {
            int EmployeeId;
            int.TryParse(keyword, out EmployeeId);
            return _context.Employees.Where(e=>
            e.EmployeeId==EmployeeId||
            e.FirstName.Contains(keyword) ||
            e.LastName.Contains(keyword) ||
            e.Title.Contains(keyword)||
            e.BirthDate.ToString().Contains(keyword) ||
            e.HireDate.ToString().Contains(keyword) ||
			e.Address.ToString().Contains(keyword) ||
			e.City.ToString().Contains(keyword) ||
			e.PostalCode.ToString().Contains(keyword) ||
			e.Country.ToString().Contains(keyword) ||
			e.HomePhone.ToString().Contains(keyword) 
			).Select(e=>new EmployeeDTO { 
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Title = e.Title,
                BirthDate = e.BirthDate,
                HireDate = e.HireDate,
                Address = e.Address,
                City = e.City,
                PostalCode = e.PostalCode,
                Country = e.Country,
                HomePhone = e.HomePhone
			});      
            //return await _context.Employees.ToListAsync();      //需要時間, 需要空間
        }


        //GET api/Employees/1
        [HttpGet("{id}")]
        public async Task<FileResult> GetPhoto(int id)
        {
            string Filename = Path.Combine("StatiocFiles", "images", "NoLmage.jpg");
            Employee? Emp = await _context.Employees.FindAsync(id);
            byte[] ImageContent = Emp?.Photo ?? System.IO.File.ReadAllBytes(Filename);
            return File(ImageContent,"image/jpeg");

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
                Emp.BirthDate = employeeDTO.BirthDate;
                Emp.HireDate = employeeDTO.HireDate;
                Emp.Address = employeeDTO.Address;
                Emp.City = employeeDTO.City;
                Emp.PostalCode = employeeDTO.PostalCode;
                Emp.Country = employeeDTO.Country;
                Emp.HomePhone = employeeDTO.HomePhone;
                if (employeeDTO.Photo != null) 
                {
                    using (BinaryReader br = new BinaryReader(employeeDTO.Photo.OpenReadStream()))
                    { 
                    Emp.Photo = br.ReadBytes((int)employeeDTO.Photo.Length);
					}                          
                }
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
