using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Dtos
{
  public record EmailRequestDTO(String to, String subject, String body)
  {
  }
}
