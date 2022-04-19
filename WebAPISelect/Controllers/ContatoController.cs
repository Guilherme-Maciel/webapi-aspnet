using WebAPISelect.Model;
using WebAPISelect.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebAPISelect.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ContatoController : ControllerBase
    {
        //Dependêcia
        private readonly IContatoRepository _contatoRepository;


        public ContatoController(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }
        [HttpGet]
        [Produces(typeof(Contato))]

        //Executa a query SELECT
        public IActionResult Get()
        {
            var contatos = _contatoRepository.GetAll();

            if (contatos.Count() == 0)
                return NoContent();

            return Ok(contatos);

        }
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var deleteContato = _contatoRepository.Delete(id);

            return Ok(deleteContato);
        }

        [HttpPost]
        public IActionResult Insert(string name, string email, string number)
        {
            var insertContato = _contatoRepository.Insert(name, email, number);

            return Ok(insertContato);
        }
        [HttpPut("update/{id}")]
        public IActionResult Update(int id, string name, string email, string number)
        {
            var updateContato = _contatoRepository.Update(id, name, email, number);

            return Ok(updateContato);
        }
       [HttpGet("{id}")]
       public IActionResult Get(int id)
        {
            var selectContato = _contatoRepository.Get(id);

            if (selectContato.Count() == 0)
                return NoContent();

            return Ok(selectContato);


        }

       

    }
}
