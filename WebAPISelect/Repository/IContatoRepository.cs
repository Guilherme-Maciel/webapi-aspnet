using System.Collections.Generic;
using WebAPISelect.Model;

namespace WebAPISelect.Repository
{
    public interface IContatoRepository
    {
        //Assinatura do método GetAll
        IEnumerable<Contato> GetAll();
        //Assinatura método Get
        IEnumerable<Contato> Get(int id);
        //Assinatura método Delete
        IEnumerable<Contato> Delete(int id);
        //Assinatura método Insert
        IEnumerable<Contato> Insert(string name, string email, string number);
        //Assinatura método update
        IEnumerable<Contato> Update(int id, string name, string email, string number);
    }
}
