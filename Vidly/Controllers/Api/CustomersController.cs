using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data.Entity;
using System.Net.Http;
using System.Web.Http;
using Vidly.DTO;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;

        public CustomersController(IMapper mapper)
        {
            _context = new ApplicationDbContext();
            _mapper = mapper;
        }

        // GET /api/customers
        public IHttpActionResult GetCustomers()
        {
            var customersDTOs = _context.Customers
                .Include(c => c.MembershipType)
                .ToList()
                .Select(_mapper.Map<Customer, CustomerDTO>);

            return Ok(customersDTOs);
        }

        // GET /api/customers/1
        [HttpGet]
        public IHttpActionResult GetCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Customer, CustomerDTO>(customer));
        }

        // POST /api/customers
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDTO customerDTO)
        {
            Customer createdCustomer = null;

            try
            {
                if (ModelState.IsValid == false)
                {
                    return BadRequest();
                }

                createdCustomer = _context.Customers.Add(_mapper.Map<CustomerDTO, Customer>(customerDTO));
                _context.SaveChanges();

                customerDTO.Id = createdCustomer.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (createdCustomer == null)
            {
                return InternalServerError();
            }

            return Created(new Uri($"{Request.RequestUri}/{customerDTO.Id}"), customerDTO);
        }

        // PUT /api/customers/{id}
        [HttpPut]
        public void UpdateCustomer(int id, CustomerDTO customerDTO)
        {
            if (ModelState.IsValid == false)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _mapper.Map(customerDTO, customerInDb);

            _context.SaveChanges();
        }

        // DELETE /api/customers/{id}
        [HttpDelete]
        public void DeleteCustomer(int id)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _context.Customers.Remove(customerInDb);
            _context.SaveChanges();
        }
    }
}
