using API.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DataAccess.Repositorios
{
    public interface iContactoUsuario
    {
        public Task<List<ContactoUsuario>> ConsultarContactos(string idUsuario, int idContacto);
    }
}
