using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Braavos.Entities;
using Braavos.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Braavos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBraavosRepository _braavosRepository;

        public BankAccountController(IBraavosRepository braavosRepository) => _braavosRepository = braavosRepository;

        [HttpGet]
        public async Task<ActionResult<List<Nation>>> GetBankAccounts()
        {
            try
            {
                var results = await _braavosRepository.GetCreditAccounts();
                return Ok(results);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
    }
}
