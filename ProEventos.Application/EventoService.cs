using ProEventos.Application.Contratos;
using ProEventos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEventos.Persistence.Contratos;
using ProEventos.Application.Dtos;
using AutoMapper;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Models;

namespace ProEventos.Application
{
  public class EventoService : IEventoService
  {

    private readonly IGeralPersist _geralPersist;

    private readonly IEventoPersist _eventoPersist;

    private readonly IMapper _mapper;

    private readonly IEmailServiceClient _emailServiceClient;

    public EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist, IMapper mapper, IEmailServiceClient emailServiceClient)
    {
      _eventoPersist = eventoPersist;

      _geralPersist = geralPersist;

      _mapper = mapper;

      _emailServiceClient = emailServiceClient;
    }

    public async Task<EventoDto> AddEventos(int userId, EventoDto model)
    {
      try
      {
        var evento = _mapper.Map<Evento>(model);
        evento.UserId = userId;

        _geralPersist.Add<Evento>(evento);

        if (await _geralPersist.SaveChangesAsync())
        {
          var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(userId, evento.Id, false);

          return _mapper.Map<EventoDto>(eventoRetorno);
        }
        return null;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }

    public async Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model)
    {
      try
      {
        var evento = await _eventoPersist.GetEventoByIdAsync(userId, eventoId, false);
        if (evento == null) return null;

        // teste de envio realizado. falta terminar a logica
        //envio de email
        // Console.WriteLine("chamado o java pelo c# pra enviar o email");
        // EmailRequestDTO emailRequest = new EmailRequestDTO(evento.email, "Confirmação de Inscrição", "Você foi inscrito no evento com sucesso (projeto C#)!");
        // var result = await _emailServiceClient.sendEmail(emailRequest);
        // if (result == null) throw new Exception("Evento não atualizado! O sistema não conseguiu enviar o e-mail para :" + evento.email);
        //envio de email

        model.Id = evento.Id;
        model.UserId = userId;

        _mapper.Map(model, evento);

        _geralPersist.Update<Evento>(evento);

        if (await _geralPersist.SaveChangesAsync())
        {
          var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(userId, evento.Id, false);

          return _mapper.Map<EventoDto>(eventoRetorno);
        }
        return null;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }

    public async Task<bool> DeleteEvento(int userId, int eventoId)
    {
      try
      {
        var evento = await _eventoPersist.GetEventoByIdAsync(userId, eventoId, false);
        if (evento == null) throw new Exception("Evento para delete não encontrado.");

        _geralPersist.Delete<Evento>(evento);
        return await _geralPersist.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }

    public async Task<PageList<EventoDto>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false)
    {
      try
      {
        var eventos = await _eventoPersist.GetAllEventosAsync(userId, pageParams, includePalestrantes);
        if (eventos == null) return null;

        var resultado = _mapper.Map<PageList<EventoDto>>(eventos);

        resultado.CurrentPage = eventos.CurrentPage;
        resultado.TotalPages = eventos.TotalPages;
        resultado.PageSize = eventos.PageSize;
        resultado.TotalCount = eventos.TotalCount;

        return resultado;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }

    public async Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
    {
      try
      {
        var evento = await _eventoPersist.GetEventoByIdAsync(userId, eventoId, includePalestrantes);
        if (evento == null) return null;

        var resultado = _mapper.Map<EventoDto>(evento);

        return resultado;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }
  }
}
