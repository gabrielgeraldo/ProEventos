using ProEventos.Application.Dtos;
using ProEventos.Application.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Contratos
{
  public interface IEmailServiceClient
  {

    [Post("/send")]
    Task<Result> sendEmail(EmailRequestDTO emailRequest);

  }
}
